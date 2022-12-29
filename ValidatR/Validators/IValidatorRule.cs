namespace ValidatR.Validators;
internal interface IValidatorRule
{
    Task ExecuteHandleAsync<T>(IValidationContext validationContext, T parameter, CancellationToken cancellationToken);
}


internal interface IValidatorRule<TParameter> : IValidatorRule
{
    Task HandleAsync<TModel, TValue>(ValidationContext<TModel, TValue> validationContext, TParameter parameter, CancellationToken cancellationToken);
}