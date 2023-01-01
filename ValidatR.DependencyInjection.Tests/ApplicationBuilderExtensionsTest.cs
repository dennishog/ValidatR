using Microsoft.AspNetCore.Builder;
using NSubstitute;

namespace ValidatR.DependencyInjection.Tests;

public class ApplicationBuilderExtensionsTest
{
    [Fact]
    public void UseValidatRSuccessfully()
    {
        var serviceCollection = new ServiceCollection();

        serviceCollection.AddValidatR<string>();

        var serviceProvider = serviceCollection.BuildServiceProvider();

        var applicationBuilder = Substitute.For<IApplicationBuilder>();
        applicationBuilder.ApplicationServices.Returns(serviceProvider);

        applicationBuilder.UseValidatR<string>()
            .AddMinLengthValidator((id, parameter) => 1)
            .AddMaxLengthValidator((id, parameter) => 10)
            .AddRegexValidator((id, parameter) => @"\d\d")
            .AddRequiredValidator((id, parameter) => true);
    }
}
