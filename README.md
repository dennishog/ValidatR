# ValidatR
Simple attribute based validations where validation rule values are fetched through Func to let the consumer decide the values based on field, validationtype and a parameter which can be sent in the ValidateAsync or fetched through a parameter resolver.
## Use cases
* Validation rules needs to be different based on model property value (using parameter resolver)
  * Example: All validation rules needs to be different based on a property value
  * Example: Validation rules differ for CreateCustomerRequest depending on country
* Validation rules needs to be different based on external value (the parameter is provided in the validate method)
  * Example: Vary rules based on countryCode sent in route to controller

## ValidateAttribute
To add validation rules to a model use the ```[Validate(ValidatorType | ValidatorType, "id")]``` attribute.
Example:
```csharp
public class CreateCustomerRequest 
{
    [Validate(ValidatorType.Regex | ValidatorType.Required, "CreateCustomerRequest.FirstName")]
    public string FirstName { get; set; }

    [Validate(ValidatorType.Required, "CreateCustomerRequest.Address")]
    public Address Address { get; set; }
    ...
}

public class Address
{
    [Validate(ValidatorType.Required | ValidatorType.MaxLength, "Address.Street")]
    public string Street { get; set; }
}
```
Validate attribute can also be used on complex types, but in that case only required is supported on the nested type in the class used to trigger validation.
**OBS! Validation will only be performed on properties using the Validate attribute, and no exception will be thrown if no Validate attribute exists in a class used in the ValidateAsync method**

## Example registration
```csharp
builder.Services.AddValidatR<string>((name, type, parameter) =>
{
    return type switch
    {
        ValidatorType.Regex => @"\d\d",
        ValidatorType.MaxLength => "3",
        _ => throw new InvalidOperationException()
    };
}).AddParameterResolver<CreateCustomerRequest>(x => x.LastName);
```

If there i a need to use a service in the func (as in the example application), use the IApplicationBuilder extension below (The func in the AddValidatR is optional).
```csharp
app.UseValidatR<string>((name, type, parameter) =>
{
    var storageService = app.Services.GetRequiredService<IStorageService>();
    return storageService.GetValidationRuleValue(name, type, parameter);
});
```

## Validation
There are 3 ways to validate
* Injecting ```IValidator<TParameter>```, where TParameter is the type of the key used in the func
* Injecting ```IValidator```, requires adding Parameter resolvers for the types being validated otherwise an exception will be thrown.
* Using the middleware (experimental and are low performance since it requires one middleware/type at the moment).

## Registering parameter resolvers
The AddValidatR extension method returns an interface so you can add resolvers.
```csharp
builder.Services.AddValidatR<string>().AddParameterResolver<CreateCustomerRequest>(x => x.LastName);
```