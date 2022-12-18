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