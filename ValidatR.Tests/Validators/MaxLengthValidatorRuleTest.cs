using ValidatR.Attributes;
using ValidatR.Exceptions;
using ValidatR.Resolvers;
using ValidatR.Tests.Fakes;
using ValidatR.Validators;

namespace ValidatR.Tests.Validators;

public class MaxLengthValidatorRuleTest
{
    private readonly IValidatorRuleValueResolver<string> _ruleValueResolver;
    private readonly MaxLengthValidatorRule<string> _sut;

    public MaxLengthValidatorRuleTest()
    {
        _ruleValueResolver = Substitute.For<IValidatorRuleValueResolver<string>>();
        _sut = new MaxLengthValidatorRule<string>(_ruleValueResolver);
    }

    [Theory]
    [InlineData("test", 4)]
    [InlineData("testi", 5)]
    [InlineData("testin", 6)]
    [InlineData("testing", 7)]
    public async Task ValidateSuccessfully(string value, int maxLength)
    {
        var fixture = new Fixture();
        var model = fixture.Create<RequestModel>();
        var id = fixture.Create<string>();
        var parameter = fixture.Create<string>();

        var attribute = new ValidateAttribute(id, Enums.ValidatorType.MaxLength);
        var validationContext = new ValidationContext<RequestModel, string>(attribute, value, model);

        var cancellationToken = new CancellationToken();

        _ruleValueResolver.GetValidatorRuleValue<int>(typeof(MaxLengthValidatorRule<string>), id, parameter).Returns(maxLength);

        await _sut.HandleAsync(validationContext, parameter, cancellationToken);
    }

    [Theory]
    [InlineData("test", 3)]
    [InlineData("testi", 4)]
    [InlineData("testin", 5)]
    [InlineData("testing", 6)]
    public async Task ValidateThrowsValidationException(string value, int maxLength)
    {
        var fixture = new Fixture();
        var model = fixture.Create<RequestModel>();
        var id = fixture.Create<string>();
        var parameter = fixture.Create<string>();

        var attribute = new ValidateAttribute(id, Enums.ValidatorType.MaxLength);
        var validationContext = new ValidationContext<RequestModel, string>(attribute, value, model);

        var cancellationToken = new CancellationToken();

        _ruleValueResolver.GetValidatorRuleValue<int>(typeof(MaxLengthValidatorRule<string>), id, parameter).Returns(maxLength);

        Func<Task> act = () => _sut.HandleAsync(validationContext, parameter, cancellationToken);

        await act.Should().ThrowAsync<ValidationException>().WithMessage($"Value '{validationContext.Value}' should have a maximum length of '{maxLength}'");
    }
}
