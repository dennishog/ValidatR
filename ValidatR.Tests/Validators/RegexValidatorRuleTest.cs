using ValidatR.Attributes;
using ValidatR.Exceptions;
using ValidatR.Resolvers;
using ValidatR.Tests.Fakes;
using ValidatR.Validators;

namespace ValidatR.Tests.Validators;

public class RegexValidatorRuleTest
{
    private readonly IValidatorRuleValueResolver<string> _ruleValueResolver;
    private readonly RegexValidatorRule<string> _sut;

    public RegexValidatorRuleTest()
    {
        _ruleValueResolver = Substitute.For<IValidatorRuleValueResolver<string>>();
        _sut = new RegexValidatorRule<string>(_ruleValueResolver);
    }

    [Theory]
    [InlineData("test", "^[a-z][a-z][a-z][a-z]$")]
    public async Task ValidateSuccessfully(string value, string pattern)
    {
        var fixture = new Fixture();
        var model = fixture.Create<RequestModel>();
        var id = fixture.Create<string>();
        var parameter = fixture.Create<string>();

        var attribute = new ValidateAttribute(id, Enums.ValidatorType.Regex);
        var validationContext = new ValidationContext<RequestModel, string>(attribute, value, model);

        var cancellationToken = new CancellationToken();

        _ruleValueResolver.GetValidatorRuleValue<string>(typeof(RegexValidatorRule<string>), id, parameter).Returns(pattern);

        await _sut.HandleAsync(validationContext, parameter, cancellationToken);
    }

    [Theory]
    [InlineData("test", @"\d\d\d\d")]
    public async Task ValidateThrowsValidationException(string value, string pattern)
    {
        var fixture = new Fixture();
        var model = fixture.Create<RequestModel>();
        var id = fixture.Create<string>();
        var parameter = fixture.Create<string>();

        var attribute = new ValidateAttribute(id, Enums.ValidatorType.Regex);
        var validationContext = new ValidationContext<RequestModel, string>(attribute, value, model);

        var cancellationToken = new CancellationToken();

        _ruleValueResolver.GetValidatorRuleValue<string>(typeof(RegexValidatorRule<string>), id, parameter).Returns(pattern);

        Func<Task> act = () => _sut.HandleAsync(validationContext, parameter, cancellationToken);

        await act.Should().ThrowAsync<ValidationException>().WithMessage($"Value '{validationContext.Value}' is not allowed. Expected to follow pattern '{pattern}'");
    }
}
