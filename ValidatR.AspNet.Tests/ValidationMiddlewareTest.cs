using ValidatR.AspNet.Tests.Fakes;

namespace ValidatR.AspNet.Tests;

public class ValidationMiddlewareTest
{
    [Fact]
    public async Task InvokeSuccessfully()
    {
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddMvc();
        var serviceProvider = serviceCollection.BuildServiceProvider();

        var fixture = new Fixture();
        var model = fixture.Create<RequestModel>();

        var cancellationToken = new CancellationToken();

        var httpContext = new DefaultHttpContext
        {
            RequestAborted = cancellationToken
        };

        var json = JsonConvert.SerializeObject(model);
        var stream = new MemoryStream(Encoding.UTF8.GetBytes(json));
        httpContext.Request.Body = stream;

        httpContext.Request.ContentLength = stream.Length;
        httpContext.Request.ContentType = "application/json";

        var controllerActionDescriptor = new ControllerActionDescriptor
        {
            ActionName = "Test",
            Parameters = new List<ParameterDescriptor> {
                new ParameterDescriptor
                {
                    Name = "Test",
                    ParameterType = typeof(RequestModel)
                }
            }
        };

        var endpointMetadataCollection = new EndpointMetadataCollection(controllerActionDescriptor);

        var endpoint = new Endpoint((httpContext) => Task.CompletedTask, endpointMetadataCollection, "test");
        httpContext.SetEndpoint(endpoint);
        var requestDelegate = new RequestDelegate((context) => Task.FromResult(httpContext));
        var sut = new ValidationMiddleware<RequestModel>(requestDelegate);

        var validator = Substitute.For<IValidator>();

        await sut.InvokeAsync(httpContext, validator);

        await validator.Received(1).ValidateAsync(Arg.Is<RequestModel>(model), httpContext.RequestAborted);
    }
}
