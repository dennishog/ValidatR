using NSubstitute;
using ValidatR.Providers;
using ValidatR.Resolvers;
using ValidatR.Tests.Fakes;
using ValidatR.Validators;

namespace ValidatR.Tests;
public class ValidatorTest
{
    private readonly IValidatorRule<string> _validator;
    private readonly IEnumerable<IValidatorRule<string>> _validators;
    private readonly IParameterResolver<string> _parameterResolver;
    private readonly IEnumerable<IParameterResolver<string>> _parameterResolvers;
    private readonly IPropertyProvider _propertyProvider;
    private readonly Validator<string> _sut;

    public ValidatorTest()
    {
        _validator = Substitute.For<IValidatorRule<string>>();
        _validators = new List<IValidatorRule<string>>
        {
            _validator
        };

        _parameterResolver = Substitute.For<IParameterResolver<string>>();
        _parameterResolvers = new List<IParameterResolver<string>>
        {
            _parameterResolver
        };
        _propertyProvider = Substitute.For<IPropertyProvider>();

        _sut = new Validator<string>(_validators, _parameterResolvers, _propertyProvider);
    }

    [Fact]
    public async Task ValidateWithResolverSuccessfully()
    {
        var fixture = new Fixture();
        var request = fixture.Create<RequestModel>();
        var cancellationToken = new CancellationToken();

        var parameter = fixture.Create<string>();

        _parameterResolver.GetParameterValue(request).Returns(parameter);
        _parameterResolver.ShouldHandle(request).Returns(true);

        _propertyProvider.GetValidationContextForAllPropertiesAsync(request, cancellationToken).Returns(new List<IValidationContext>
        {
            new ValidationContext<RequestModel, string>(new Attributes.ValidateAttribute("test", Enums.ValidatorType.Required), "test", request)
        });

        await _sut.ValidateAsync(request, cancellationToken);
    }
}
