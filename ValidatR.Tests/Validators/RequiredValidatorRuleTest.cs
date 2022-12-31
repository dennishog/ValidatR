using ValidatR.Attributes;
using ValidatR.Exceptions;
using ValidatR.Resolvers;
using ValidatR.Tests.Fakes;
using ValidatR.Validators;

namespace ValidatR.Tests.Validators;

public class RequiredValidatorRuleTest
{
    private readonly IValidatorRuleValueResolver<string> _ruleValueResolver;
    private readonly RequiredValidatorRule<string> _sut;

    public RequiredValidatorRuleTest()
    {
        _ruleValueResolver = Substitute.For<IValidatorRuleValueResolver<string>>();
        _sut = new RequiredValidatorRule<string>(_ruleValueResolver);
    }

    [Theory]
    [InlineData("test", true)]
    public async Task ValidateSuccessfully(string value, bool required)
    {
        var fixture = new Fixture();
        var model = fixture.Create<RequestModel>();
        var id = fixture.Create<string>();
        var parameter = fixture.Create<string>();

        var attribute = new ValidateAttribute(id, Enums.ValidatorType.Required);
        var validationContext = new ValidationContext<RequestModel, string>(attribute, value, model);

        var cancellationToken = new CancellationToken();

        _ruleValueResolver.GetValidatorRuleValue<bool>(typeof(RequiredValidatorRule<string>), id, parameter).Returns(required);

        await _sut.HandleAsync(validationContext, parameter, cancellationToken);
    }

    [Theory]
    [InlineData(null, true)]
    [InlineData("", true)]
    public async Task ValidateThrowsValidationException(string? value, bool required)
    {
        var fixture = new Fixture();
        var model = fixture.Create<RequestModel>();
        var id = fixture.Create<string>();
        var parameter = fixture.Create<string>();

        var attribute = new ValidateAttribute(id, Enums.ValidatorType.Required);
        var validationContext = new ValidationContext<RequestModel, string?>(attribute, value, model);

        var cancellationToken = new CancellationToken();

        _ruleValueResolver.GetValidatorRuleValue<bool>(typeof(RequiredValidatorRule<string>), id, parameter).Returns(required);

        Func<Task> act = () => _sut.HandleAsync(validationContext, parameter, cancellationToken);

        await act.Should().ThrowAsync<ValidationException>().WithMessage($"Value is required");
    }

    [Theory]
    [InlineData("test", true)]
    public async Task ExecuteSuccessfully(string value, bool required)
    {
        var fixture = new Fixture();
        var model = fixture.Create<RequestModel>();
        var id = fixture.Create<string>();
        var parameter = fixture.Create<string>();

        var attribute = new ValidateAttribute(id, Enums.ValidatorType.Required);
        var validationContext = new ValidationContext<RequestModel, string>(attribute, value, model);

        var cancellationToken = new CancellationToken();

        _ruleValueResolver.GetValidatorRuleValue<bool>(typeof(RequiredValidatorRule<string>), id, parameter).Returns(required);

        await _sut.ExecuteHandleAsync(validationContext, parameter, cancellationToken);
    }

    [Theory]
    [InlineData(null, true)]
    [InlineData("", true)]
    public async Task ExecuteThrowsValidationException(string? value, bool required)
    {
        var fixture = new Fixture();
        var model = fixture.Create<RequestModel>();
        var id = fixture.Create<string>();
        var parameter = fixture.Create<string>();

        var attribute = new ValidateAttribute(id, Enums.ValidatorType.Required);
        var validationContext = new ValidationContext<RequestModel, string?>(attribute, value, model);

        var cancellationToken = new CancellationToken();

        _ruleValueResolver.GetValidatorRuleValue<bool>(typeof(RequiredValidatorRule<string>), id, parameter).Returns(required);

        Func<Task> act = () => _sut.ExecuteHandleAsync(validationContext, parameter, cancellationToken);

        await act.Should().ThrowAsync<ValidationException>().WithMessage($"Value is required");
    }
}
