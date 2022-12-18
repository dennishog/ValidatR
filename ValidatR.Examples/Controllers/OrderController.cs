using Microsoft.AspNetCore.Mvc;
using ValidatR.Examples.Viewmodels;

namespace ValidatR.Examples.Controllers;

[ApiController]
[Route("[controller]")]
public class OrderController : ControllerBase
{
    private readonly IValidator<string> _validator;

    public OrderController(IValidator<string> validator)
    {
        _validator = validator;
    }

    [HttpPost]
    public async Task<ActionResult> CreateOrder(CreateOrderRequest request, CancellationToken cancellationToken)
    {
        await _validator.ValidateAsync(request, "se", cancellationToken);

        return Created("", request);
    }
}
