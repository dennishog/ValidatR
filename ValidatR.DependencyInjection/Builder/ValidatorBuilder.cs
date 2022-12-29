using ValidatR.Resolvers;
using ValidatR.Validators;

namespace ValidatR.DependencyInjection.Builder;

public class ValidatorBuilder<TParameter> : IValidatorBuilder<TParameter>
{
    private readonly IEnumerable<IValidatorRule<TParameter>> _validators;
    private readonly IValidatorRuleValueResolver<TParameter> _validatorRuleValueResolver;

    public ValidatorBuilder(IApplicationBuilder applicationBuilder, IValidatorRuleValueResolver<TParameter> validatorRuleValueResolver)
    {
        _validators = applicationBuilder.ApplicationServices.GetRequiredService<IEnumerable<IValidatorRule<TParameter>>>();
        _validatorRuleValueResolver = validatorRuleValueResolver;
    }

    public IValidatorBuilder<TParameter> AddMaxLengthValidator(Func<string, TParameter, int> getRuleValidationValue)
    {
        var validator = _validators.Single(x => x is MaxLengthValidatorRule<TParameter>) as MaxLengthValidatorRule<TParameter>;

        _validatorRuleValueResolver.AddRuleValueFunc(validator?.GetType() ?? throw new ArgumentNullException(nameof(validator)), getRuleValidationValue);

        return this;
    }

    public IValidatorBuilder<TParameter> AddMinLengthValidator(Func<string, TParameter, int> getRuleValidationValue)
    {
        var validator = _validators.Single(x => x is MinLengthValidatorRule<TParameter>) as MinLengthValidatorRule<TParameter>;

        _validatorRuleValueResolver.AddRuleValueFunc(validator?.GetType() ?? throw new ArgumentNullException(nameof(validator)), getRuleValidationValue);

        return this;
    }

    public IValidatorBuilder<TParameter> AddRegexValidator(Func<string, TParameter, string> getRuleValidationValue)
    {
        var validator = _validators.Single(x => x is RegexValidatorRule<TParameter>) as RegexValidatorRule<TParameter>;

        _validatorRuleValueResolver.AddRuleValueFunc(validator?.GetType() ?? throw new ArgumentNullException(nameof(validator)), getRuleValidationValue);

        return this;
    }

    public IValidatorBuilder<TParameter> AddRequiredValidator(Func<string, TParameter, bool> getRuleValidationValue)
    {
        var validator = _validators.Single(x => x is RequiredValidatorRule<TParameter>) as RequiredValidatorRule<TParameter>;

        _validatorRuleValueResolver.AddRuleValueFunc(validator?.GetType() ?? throw new ArgumentNullException(nameof(validator)), getRuleValidationValue);

        return this;
    }
}
