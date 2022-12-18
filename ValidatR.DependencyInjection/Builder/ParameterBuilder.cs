namespace ValidatR.DependencyInjection.Builder;

using Resolvers;

public class ParameterBuilder<TParameter> : IParameterBuilder<TParameter>
{
	private readonly IValidator<TParameter> _validator;

	public ParameterBuilder(IValidator<TParameter> validator)
	{
		_validator = validator;
	}

	public IParameterBuilder<TParameter> AddParameterResolver<TModel>(Expression<Func<TModel, TParameter>> parameterSelector)
	{
		_validator.AddParameterResolver(new ParameterResolver<TModel, TParameter>(parameterSelector));

		return this;
	}
}
