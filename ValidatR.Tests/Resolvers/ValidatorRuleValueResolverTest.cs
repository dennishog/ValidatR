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
}
