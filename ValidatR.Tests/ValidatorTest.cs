using NSubstitute;
using NSubstitute.ExceptionExtensions;
using ValidatR.Exceptions;
using ValidatR.Providers;
using ValidatR.Resolvers;
using ValidatR.Tests.Fakes;
using ValidatR.Validators;

namespace ValidatR.Tests;
public class ValidatorTest
{
    private readonly IValidatorRule<string> _validator;
    private readonly IParameterResolver<string> _parameterResolver;
    private readonly IPropertyProvider _propertyProvider;
    private readonly Validator<string> _sut;

    public ValidatorTest()
    {
        _validator = Substitute.For<IValidatorRule<string>>();
        var validators = new List<IValidatorRule<string>>
        {
            _validator
        };

        _parameterResolver = Substitute.For<IParameterResolver<string>>();
        var parameterResolvers = new List<IParameterResolver<string>>
        {
            _parameterResolver
        };
        _propertyProvider = Substitute.For<IPropertyProvider>();

        _sut = new Validator<string>(validators, parameterResolvers, _propertyProvider);
    }

    [Fact]
    public async Task ValidateWithResolverThrowsException()
    {
        var fixture = new Fixture();
        var request = fixture.Create<RequestModel>();
        var cancellationToken = new CancellationToken();

        var parameter = fixture.Create<string>();

        _parameterResolver.GetParameterValue(request).Throws(new ParameterResolverNotFoundException<RequestModel, string>(request));

        Func<Task> act = () => _sut.ValidateAsync(request, cancellationToken);

        await act.Should().ThrowAsync<ParameterResolverNotFoundException<RequestModel, string>>();
    }

    [Fact]
    public async Task ValidateWithResolverSuccessfully()
    {
        var fixture = new Fixture();
        var request = fixture.Create<RequestModel>();
        var cancellationToken = new CancellationToken();

        var parameter = fixture.Create<string>();

        _parameterResolver.GetParameterValue(request).Throws(new ParameterResolverNotFoundException<RequestModel, string>(request));
        _parameterResolver.ShouldHandle(request).Returns(true);

        _propertyProvider.GetValidationContextForAllPropertiesAsync(request, cancellationToken).Returns(new List<IValidationContext>
        {
            new ValidationContext<RequestModel, string>(new Attributes.ValidateAttribute("test", Enums.ValidatorType.Required), "test", request)
        });

        await _sut.ValidateAsync(request, cancellationToken);
    }
}
