using ValidatR.Resolvers;
using ValidatR.Tests.Fakes;

namespace ValidatR.Tests.Resolvers;

public class ParameterResolverTest
{
    [Fact]
    public void ShouldHandleSuccessfully()
    {
        var fixture = new Fixture();
        var request = fixture.Create<RequestModel>();
        var anotherRequest = fixture.Create<AnotherRequestModel>();

        var sut = new ParameterResolver<RequestModel, string>(x => x.StringValue);

        var resultTrue = sut.ShouldHandle(request);
        resultTrue.Should().BeTrue();

        var resultFalse = sut.ShouldHandle(anotherRequest);
        resultFalse.Should().BeFalse();
    }

    [Fact]
    public void GetParameterValueSuccessfully()
    {
        var fixture = new Fixture();
        var request = fixture.Create<RequestModel>();

        var sut = new ParameterResolver<RequestModel, string>(x => x.StringValue);

        var result = sut.GetParameterValue(request);

        result.Should().Be(request.StringValue);
    }
}
