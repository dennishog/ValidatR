using ValidatR.DependencyInjection.Tests.Fakes;
using ValidatR.Providers;
using ValidatR.Resolvers;
using ValidatR.Validators;

namespace ValidatR.DependencyInjection.Tests;

public class ServiceCollectionExtensionsTest
{
    [Fact]
    public void AddValidatRSuccessfully()
    {
        var serviceCollection = new ServiceCollection();

        serviceCollection.AddValidatR<string>().AddParameterResolver<RequestModel>(x => x.StringValue!);

        var serviceProvider = serviceCollection.BuildServiceProvider();

        var validatorRuleValueResolver = serviceProvider.GetRequiredService<IValidatorRuleValueResolver<string>>();
        validatorRuleValueResolver.Should().NotBeNull();

        var propertyProvider = serviceProvider.GetRequiredService<IPropertyProvider>();
        propertyProvider.Should().NotBeNull();

        var validatorWithParameter = serviceProvider.GetRequiredService<IValidator<string>>();
        validatorWithParameter.Should().NotBeNull();

        var validator = serviceProvider.GetRequiredService<IValidator>();
        validator.Should().NotBeNull();

        var validatorRules = serviceProvider.GetRequiredService<IEnumerable<IValidatorRule<string>>>();
        validatorRules.Should().NotBeNull();
        validatorRules.Should().HaveCount(4);

        var minLength = validatorRules.SingleOrDefault(validatorRule => validatorRule.GetType().Equals(typeof(MinLengthValidatorRule<string>)));
        minLength.Should().NotBeNull();

        var maxLength = validatorRules.SingleOrDefault(validatorRule => validatorRule.GetType().Equals(typeof(MaxLengthValidatorRule<string>)));
        maxLength.Should().NotBeNull();

        var regex = validatorRules.SingleOrDefault(validatorRule => validatorRule.GetType().Equals(typeof(RegexValidatorRule<string>)));
        regex.Should().NotBeNull();

        var required = validatorRules.SingleOrDefault(validatorRule => validatorRule.GetType().Equals(typeof(RequiredValidatorRule<string>)));
        required.Should().NotBeNull();

        var parameterResolver = serviceProvider.GetRequiredService<IParameterResolver<string>>();
        parameterResolver.Should().NotBeNull();
    }
}
