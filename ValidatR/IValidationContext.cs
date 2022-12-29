namespace ValidatR;

internal interface IValidationContext
{
    Type GetValueType();
    Type GetModelType();
}
