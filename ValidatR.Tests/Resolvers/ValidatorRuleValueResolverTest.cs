using ValidatR.Exceptions;
using ValidatR.Resolvers;

namespace ValidatR.Tests.Resolvers;

public class ValidatorRuleValueResolverTest
{
    [Fact]
    public void AddAndGetValidatorRuleValueSuccessfully()
    {
        var fixture = new Fixture();
        var id = fixture.Create<string>();
        var parameter = fixture.Create<string>();

        var sut = new ValidatorRuleValueResolver<string>();

        Func<string, string, string> func = (id, parameter) =>
        {
            return $"{id}{parameter}";
        };

        sut.AddRuleValueFunc(typeof(string), func);

        var result = sut.GetValidatorRuleValue<string>(typeof(string), id, parameter);

        result.Should().Be($"{id}{parameter}");
    }

    [Fact]
    public void GetValidatorRuleValueThrowsArgumentNullException()
    {
        var sut = new ValidatorRuleValueResolver<string?>();


        Action act = () => sut.GetValidatorRuleValue<string?>(typeof(string), "id", null);

        act.Should().Throw<ArgumentNullException>().WithParameterName("parameter");
    }

    [Fact]
    public void GetValidatorRuleValueThrowsValidatorRuleValueResolverNotFoundException()
    {
        var fixture = new Fixture();
        var id = fixture.Create<string>();
        var parameter = fixture.Create<string>();

        var sut = new ValidatorRuleValueResolver<string>();

        Action act = () => sut.GetValidatorRuleValue<string>(typeof(string), id, parameter);

        act.Should().Throw<ValidatorRuleValueResolverNotFoundException>().WithMessage($"Could not resolve validator rule value for type {typeof(string)}");
    }

    [Fact]
    public void AddAndGetValidatorRuleValueThrowsArgumentNullException()
    {
        var fixture = new Fixture();
        var id = fixture.Create<string>();
        var parameter = fixture.Create<string>();

        var sut = new ValidatorRuleValueResolver<string>();

        Func<string, string, string?> func = (id, parameter) =>
        {
            return null;
        };

        sut.AddRuleValueFunc(typeof(string), func);

        Action act = () => sut.GetValidatorRuleValue<string>(typeof(string), id, parameter);

        act.Should().Throw<ArgumentNullException>().WithParameterName("validatorRuleValue");
    }
}
