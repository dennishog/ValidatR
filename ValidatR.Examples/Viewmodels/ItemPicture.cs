using ValidatR.Attributes;
using ValidatR.Enums;

namespace ValidatR.Examples.Viewmodels;

public record ItemPicture(string UriSmall, [property: Validate("ItemPicture.UriLarge", ValidatorType.MinLength)] string UriLarge);