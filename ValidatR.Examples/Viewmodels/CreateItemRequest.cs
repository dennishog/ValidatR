using ValidatR.Attributes;

namespace ValidatR.Examples.Viewmodels;

public record CreateItemRequest(int ItemId, [Validate("CreateItemRequest.Name", Enums.ValidatorType.Regex | Enums.ValidatorType.MaxLength)] string Name, string Description, [Validate("CreateItemRequest.Pictures", Enums.ValidatorType.Required)] List<ItemPicture> Pictures);