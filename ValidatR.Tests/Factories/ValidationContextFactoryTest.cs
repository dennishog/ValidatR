using ValidatR.Factories;
using ValidatR.Tests.Fakes;

namespace ValidatR.Tests.Factories;

public class ValidationContextFactoryTest
{
    [Fact]
    public void CreateSuccessfully()
    {
        var fixture = new Fixture();
        var id = fixture.Create<string>();
        var propertyValue = fixture.Create<string>();
        var model = fixture.Create<RequestModel>();
        var propertyInfo = typeof(RequestModel).GetProperties().First(x => x.PropertyType.Equals(typeof(string)));
        var attribute = new Attributes.ValidateAttribute(id, Enums.ValidatorType.MaxLength);

        var validationContext = (ValidationContext<RequestModel, string>)ValidationContextFactory.Create(propertyInfo, attribute, propertyValue, model);

        validationContext.ValidateAttribute.Should().Be(attribute);
        validationContext.Owner.Should().Be(model);
        validationContext.Value.Should().Be(propertyValue);
    }
}
