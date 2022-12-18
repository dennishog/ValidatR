namespace ValidatR.Exceptions
{
    public class ParameterResolverNotFoundException<TModel, TParameter> : Exception
    {
        public ParameterResolverNotFoundException(TModel model) : base($"No resolver found for type '{typeof(TParameter).Name}' using model '{typeof(TModel).Name}'")
        {

        }
    }
}
