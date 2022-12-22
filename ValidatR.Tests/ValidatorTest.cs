
using ValidatR.Enums;
using ValidatR.Exceptions;
using ValidatR.Resolvers;
using ValidatR.Tests.Fakes;

namespace ValidatR.Tests;
public class ValidatorTest
{
    [Fact]
    public async Task ValidateWithResolverSuccessfully()
    {
        var fixture = new Fixture();
        var request = fixture.Create<RequestModel>();
        var cancellationToken = new CancellationToken();

        var sut = new Validator<string>();

        sut.SetValidationRuleValueResolver((name, type, parameter) =>
        {
            return type switch
            {
                ValidatorType.Regex => @".*",
                ValidatorType.MaxLength => "400",
                _ => throw new InvalidOperationException()
            };
        });
        sut.AddParameterResolver(new ParameterResolver<RequestModel, string>(x => x.StringValue));

        await sut.ValidateAsync(request, cancellationToken);
    }

    [Fact]
    public async Task ValidateWithResolverThrowsMissingResolverException()
    {
        var fixture = new Fixture();
        var request = fixture.Create<RequestModel>();
        var cancellationToken = new CancellationToken();

        var sut = new Validator<string>();
        sut.SetValidationRuleValueResolver((name, type, parameter) =>
        {
            return type switch
            {
                ValidatorType.Regex => @".*",
                ValidatorType.MaxLength => "400",
                _ => throw new InvalidOperationException()
            };
        });
        Func<Task> act = () => sut.ValidateAsync(request, cancellationToken);

        await act.Should().ThrowAsync<ParameterResolverNotFoundException<RequestModel, string>>();
    }

    [Fact]
    public async Task ValidateWithParameter()
    {
        var fixture = new Fixture();
        var request = fixture.Create<RequestModel>();
        var cancellationToken = new CancellationToken();
        var keyParameter = fixture.Create<string>();

        var sut = new Validator<string>();
        sut.SetValidationRuleValueResolver((name, type, parameter) =>
        {
            if (parameter.Equals(keyParameter))
            {
                return type switch
                {
                    ValidatorType.Regex => @".*",
                    ValidatorType.MaxLength => "400",
                    _ => throw new InvalidOperationException()
                };
            }

            throw new InvalidOperationException();
        });

        await sut.ValidateAsync(request, keyParameter, cancellationToken);
    }
}
