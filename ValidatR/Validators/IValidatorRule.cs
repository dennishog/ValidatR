namespace ValidatR.Validators;

public interface IValidatorRule<TParameter>
{
    Task HandleAsync<TModel, TValue>(ValidationContext<TModel, TValue> validationContext, TParameter parameter, CancellationToken cancellationToken);
}