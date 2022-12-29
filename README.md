# ValidatR
Simple attribute based validations where validation rule values are fetched through Func to let the consumer decide the values based on field, validationtype and a parameter which can be sent in the ValidateAsync or fetched through a parameter resolver.
## Use cases
* Validation rules needs to be different based on model property value (using parameter resolver)
  * Example: All validation rules needs to be different based on a property value
  * Example: Validation rules differ for CreateCustomerRequest depending on country
* Validation rules needs to be different based on external value (the parameter is provided in the validate method)
  * Example: Vary rules based on countryCode sent in route to controller

## ValidateAttribute
To add validation rules to a model use the ```[Validate("id", ValidatorType | ValidatorType)]``` attribute.
Example:
```csharp
public class CreateCustomerRequest 
{
    [Validate("CreateCustomerRequest.FirstName", ValidatorType.Regex | ValidatorType.Required)]
    public string FirstName { get; set; }

    [Validate("CreateCustomerRequest.Address", ValidatorType.Required)]
    public Address Address { get; set; }
    ...
}

public class Address
{
    [Validate("Address.Street", ValidatorType.Required | ValidatorType.MaxLength)]
    public string Street { get; set; }
}
```
Validate attribute can also be used on complex types, but in that case only required is supported on the nested type in the class used to trigger validation.
**OBS! Validation will only be performed on properties using the Validate attribute, and no exception will be thrown if no Validate attribute exists in a class used in the ValidateAsync method**

## Example registration
```csharp
builder.Services.AddValidatR<string>().AddParameterResolver<CreateCustomerRequest>(x => x.LastName);
```
In the above example, we also add a parameter resolver to be able to resolve a model without specifying the parameter value to send to the func when retrieving the validation rule value.

If there i a need to use a service in the func (as in the example application), use the IApplicationBuilder extension below (The func in the AddValidatR is optional).
```csharp
app.UseValidatR<string>()
    .AddMinLengthValidator(id, parameter) =>
{
    var storageService = app.Services.GetRequiredService<IStorageService>();
    return storageService.GetValidationRuleValue(id, ValidatorType.MinLength, parameter);
});
```

## Validation
There are 3 ways to validate
* Injecting ```IValidator<TParameter>```, where TParameter is the type of the key used in the func
* Injecting ```IValidator```, requires adding Parameter resolvers for the types being validated otherwise an exception will be thrown.
* Using the middleware.

## Using the middleware
The middleware validates the request model and returns badrequest with a HttpValidationProblemDetails response if errors are found. When using the AddValidatorMiddleware applicationBuilder extension the extension method scans all loaded assemblies for classes with properties using the ValidateAttribute to automatically register a middleware for each of the classes.
To explicitly tell ValidatR which classes to use, the extension methods supports params Type[] to specify the types to load middlewares for. 