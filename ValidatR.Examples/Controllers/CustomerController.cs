using Microsoft.AspNetCore.Mvc;
using ValidatR.Examples.Viewmodels;

namespace ValidatR.Examples.Controllers;

[ApiController]
[Route("[controller]")]
public class CustomerController : ControllerBase
{
    private readonly IValidator<string> _validator;

    public CustomerController(IValidator<string> validator)
    {
        _validator = validator;
    }

    [HttpPost]
    public async Task<ActionResult> CreateCustomer(CreateCustomerRequest request, CancellationToken cancellationToken)
    {
        await _validator.ValidateAsync(request, "se", cancellationToken);

        return Created("", request);
    }
}
