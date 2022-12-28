namespace ValidatR.DependencyInjection.Builder;

public interface IValidatorBuilder<TParameter>
{
    IValidatorBuilder<TParameter> AddMinLengthValidator(Func<string, TParameter, int> getRuleValidationValue);
    IValidatorBuilder<TParameter> AddMaxLengthValidator(Func<string, TParameter, int> getRuleValidationValue);
    IValidatorBuilder<TParameter> AddRegexValidator(Func<string, TParameter, string> getRuleValidationValue);
    IValidatorBuilder<TParameter> AddRequiredValidator(Func<string, TParameter, bool> getRuleValidationValue);
}
