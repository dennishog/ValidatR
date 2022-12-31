using ValidatR.Attributes;
using ValidatR.Exceptions;
using ValidatR.Resolvers;
using ValidatR.Tests.Fakes;
using ValidatR.Validators;

namespace ValidatR.Tests.Validators;

public class MinLengthValidatorRuleTest
{
    private readonly IValidatorRuleValueResolver<string> _ruleValueResolver;
    private readonly MinLengthValidatorRule<string> _sut;

    public MinLengthValidatorRuleTest()
    {
        _ruleValueResolver = Substitute.For<IValidatorRuleValueResolver<string>>();
        _sut = new MinLengthValidatorRule<string>(_ruleValueResolver);
    }

    [Theory]
    [InlineData("test", 3)]
    [InlineData("testi", 4)]
    [InlineData("testin", 5)]
    [InlineData("testing", 6)]
    public async Task ValidateSuccessfully(string value, int minLength)
    {
        var fixture = new Fixture();
        var model = fixture.Create<RequestModel>();
        var id = fixture.Create<string>();
        var parameter = fixture.Create<string>();

        var attribute = new ValidateAttribute(id, Enums.ValidatorType.MinLength);
        var validationContext = new ValidationContext<RequestModel, string>(attribute, value, model);

        var cancellationToken = new CancellationToken();

        _ruleValueResolver.GetValidatorRuleValue<int>(typeof(MinLengthValidatorRule<string>), id, parameter).Returns(minLength);

        await _sut.HandleAsync(validationContext, parameter, cancellationToken);
    }

    [Theory]
    [InlineData("test", 5)]
    [InlineData("testi", 6)]
    [InlineData("testin", 7)]
    [InlineData("testing", 8)]
    public async Task ValidateThrowsValidationException(string value, int minLength)
    {
        var fixture = new Fixture();
        var model = fixture.Create<RequestModel>();
        var id = fixture.Create<string>();
        var parameter = fixture.Create<string>();

        var attribute = new ValidateAttribute(id, Enums.ValidatorType.MinLength);
        var validationContext = new ValidationContext<RequestModel, string>(attribute, value, model);

        var cancellationToken = new CancellationToken();

        _ruleValueResolver.GetValidatorRuleValue<int>(typeof(MinLengthValidatorRule<string>), id, parameter).Returns(minLength);

        Func<Task> act = () => _sut.HandleAsync(validationContext, parameter, cancellationToken);

        await act.Should().ThrowAsync<ValidationException>().WithMessage($"Value '{validationContext.Value}' should have a minimum length of '{minLength}'");
    }
}
