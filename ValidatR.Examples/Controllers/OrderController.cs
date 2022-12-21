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
        // CreateOrderRequest does not have a parameter rsolver registered and will only be able to validate with a provided parameter
        await _validator.ValidateAsync(request, "se", cancellationToken);

        // Below would throw a ParameterResolverNotFoundException
        // await _validator.ValidateAsync(request, cancellationToken);

        return Created("", request);
    }
}
