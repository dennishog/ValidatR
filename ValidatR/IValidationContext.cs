namespace ValidatR;

public interface IValidationContext
{
    Type GetValueType();
    Type GetModelType();
}
