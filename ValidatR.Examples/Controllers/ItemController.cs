using Microsoft.AspNetCore.Mvc;
using ValidatR.Examples.Viewmodels;

namespace ValidatR.Examples.Controllers;

[ApiController]
[Route("[controller]")]
public class ItemController : ControllerBase
{
    private readonly IValidator _validator;

    public ItemController(IValidator validator)
    {
        _validator = validator;
    }

    [HttpPost]
    public async Task<ActionResult> CreateItem(CreateItemRequest request, CancellationToken cancellationToken)
    {
        await _validator.ValidateAsync(request, cancellationToken);

        return Created("", request);
    }
}
