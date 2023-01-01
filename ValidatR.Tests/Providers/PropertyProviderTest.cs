using ValidatR.Providers;
using ValidatR.Tests.Fakes;

namespace ValidatR.Tests.Providers;

public class PropertyProviderTest
{
    private readonly PropertyProvider _sut;

    public PropertyProviderTest()
    {
        _sut = new PropertyProvider();
    }

    [Fact]
    public async Task GetValidationContextForAllPropertiesSuccessfully()
    {
        var fixture = new Fixture();
        var model = fixture.Create<RequestModel>();

        var cancellationToken = new CancellationToken();

        var result = await _sut.GetValidationContextForAllPropertiesAsync(model, cancellationToken);

        result.Should().NotBeNull();
        result.Should().HaveCount(7);

        result.Should().SatisfyRespectively(first =>
        {
            var validationContext = (ValidationContext<RequestModel, string>)first;

            validationContext.Owner.Should().Be(model);
            validationContext.Value.Should().Be(model.StringValue);

            var attribute = validationContext.ValidateAttribute;
            attribute.Should().NotBeNull();
            attribute.Id.Should().Be("StringValue");
            attribute.ValidatorType.HasFlag(Enums.ValidatorType.Regex).Should().BeTrue();
            attribute.ValidatorType.HasFlag(Enums.ValidatorType.MaxLength).Should().BeTrue();
        },
        second =>
        {
            var validationContext = (ValidationContext<RequestModel, AnotherRequestModel>)second;

            validationContext.Owner.Should().Be(model);
            validationContext.Value.Should().Be(model.Another);

            var attribute = validationContext.ValidateAttribute;
            attribute.Should().NotBeNull();
            attribute.Id.Should().Be("Another");
            attribute.ValidatorType.HasFlag(Enums.ValidatorType.Required).Should().BeTrue();
        },
        third =>
        {
            var validationContext = (ValidationContext<AnotherRequestModel, string>)third;

            validationContext.Owner.Should().Be(model.Another);
            validationContext.Value.Should().Be(model.Another.StringValue);

            var attribute = validationContext.ValidateAttribute;
            attribute.Should().NotBeNull();
            attribute.Id.Should().Be("StringValue");
            attribute.ValidatorType.HasFlag(Enums.ValidatorType.Regex).Should().BeTrue();
            attribute.ValidatorType.HasFlag(Enums.ValidatorType.MaxLength).Should().BeTrue();
        },
        fourth =>
        {
            var validationContext = (ValidationContext<RequestModel, List<AnotherRequestModel>>)fourth;

            validationContext.Owner.Should().Be(model);
            validationContext.Value.Should().BeSameAs(model.Anothers);

            var attribute = validationContext.ValidateAttribute;
            attribute.Should().NotBeNull();
            attribute.Id.Should().Be("Anothers");
            attribute.ValidatorType.HasFlag(Enums.ValidatorType.Required).Should().BeTrue();
        },
        fifth =>
        {
            var validationContext = (ValidationContext<AnotherRequestModel, string>)fifth;

            validationContext.Owner.Should().Be(model.Anothers[0]);
            validationContext.Value.Should().Be(model.Anothers[0].StringValue);

            var attribute = validationContext.ValidateAttribute;
            attribute.Should().NotBeNull();
            attribute.Id.Should().Be("StringValue");
            attribute.ValidatorType.HasFlag(Enums.ValidatorType.Regex).Should().BeTrue();
            attribute.ValidatorType.HasFlag(Enums.ValidatorType.MaxLength).Should().BeTrue();
        },
        sixth =>
        {
            var validationContext = (ValidationContext<AnotherRequestModel, string>)sixth;

            validationContext.Owner.Should().Be(model.Anothers[1]);
            validationContext.Value.Should().Be(model.Anothers[1].StringValue);

            var attribute = validationContext.ValidateAttribute;
            attribute.Should().NotBeNull();
            attribute.Id.Should().Be("StringValue");
            attribute.ValidatorType.HasFlag(Enums.ValidatorType.Regex).Should().BeTrue();
            attribute.ValidatorType.HasFlag(Enums.ValidatorType.MaxLength).Should().BeTrue();
        },
        seventh =>
        {
            var validationContext = (ValidationContext<AnotherRequestModel, string>)seventh;

            validationContext.Owner.Should().Be(model.Anothers[2]);
            validationContext.Value.Should().Be(model.Anothers[2].StringValue);

            var attribute = validationContext.ValidateAttribute;
            attribute.Should().NotBeNull();
            attribute.Id.Should().Be("StringValue");
            attribute.ValidatorType.HasFlag(Enums.ValidatorType.Regex).Should().BeTrue();
            attribute.ValidatorType.HasFlag(Enums.ValidatorType.MaxLength).Should().BeTrue();
        });
    }
}
