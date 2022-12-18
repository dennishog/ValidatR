namespace ValidatR.Tests;

using Enums;
using Fakes;
using ValidatR.Resolvers;

public class ValidatorTest
{
    [Fact]
    public async Task ValidateWithResolverSuccessfully()
    {
        var fixture = new Fixture();
        var request = fixture.Create<RequestModel>();
        var cancellationToken = new CancellationToken();

        var sut = new Validator<string>((name, type, parameter) =>
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
}
