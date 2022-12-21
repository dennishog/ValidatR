using ValidatR.Attributes;

namespace ValidatR.Examples.Viewmodels;

public record CreateItemRequest(int ItemId, [Validate(Enums.ValidatorType.Regex | Enums.ValidatorType.MaxLength, "CreateItemRequest.Name")] string Name, string Description, [Validate(Enums.ValidatorType.Required, "CreateItemRequest.Pictures")] List<ItemPicture> Pictures);