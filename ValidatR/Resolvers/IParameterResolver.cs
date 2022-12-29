namespace ValidatR.Resolvers;

internal interface IParameterResolver<TParameter>
{
    bool ShouldHandle<T>(T model);
    TParameter GetParameterValue<T>(T model);
}
