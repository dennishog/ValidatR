using ValidatR.Validators;

namespace ValidatR.DependencyInjection.Builder;

public class ValidatorBuilder<TParameter> : IValidatorBuilder<TParameter>
{
    private readonly IValidator<TParameter> _validator;

    public ValidatorBuilder(IValidator<TParameter> validator)
    {
        _validator = validator;
    }

    public IValidatorBuilder<TParameter> AddMaxLengthValidator(Func<string, TParameter, int> getRuleValidationValue)
    {
        _validator.AddValidator(new MaxLengthValidatorRule<TParameter>(getRuleValidationValue));

        return this;
    }

    public IValidatorBuilder<TParameter> AddMinLengthValidator(Func<string, TParameter, int> getRuleValidationValue)
    {
        _validator.AddValidator(new MinLengthValidatorRule<TParameter>(getRuleValidationValue));

        return this;
    }

    public IValidatorBuilder<TParameter> AddRegexValidator(Func<string, TParameter, string> getRuleValidationValue)
    {
        _validator.AddValidator(new RegexValidatorRule<TParameter>(getRuleValidationValue));

        return this;
    }

    public IValidatorBuilder<TParameter> AddRequiredValidator(Func<string, TParameter, bool> getRuleValidationValue)
    {
        _validator.AddValidator(new RequiredValidatorRule<TParameter>(getRuleValidationValue));

        return this;
    }
}
