# ValidatR
Simple attribute based validations where validation rule values are fetched through Func to let the consumer decide the values based on field, validationtype and a parameter which can be sent in the ValidateAsync or fetched through a parameter resolver.

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
There are two ways to validate
* Injecting ```IValidator<TParameter>```, where TParameter is the type of the key used in the func
* Injecting ```IValidator```, requires adding Parameter resolvers for the types being validated otherwise an exception will be thrown.
* Using the middleware (experimental and are low performance since it requires one middleware/type at the moment).

## Registering parameter resolvers
The AddValidatR extension method returns an interface so you can add resolvers.
```csharp
builder.Services.AddValidatR<string>().AddParameterResolver<CreateCustomerRequest>(x => x.LastName);
```