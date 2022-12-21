using Microsoft.AspNetCore.Mvc;
using ValidatR.Examples.Viewmodels;

namespace ValidatR.Examples.Controllers;

[ApiController]
[Route("[controller]")]
public class CustomerController : ControllerBase
{
    private readonly IValidator<string> _validator;

    public CustomerController(IValidator<string> validator, IValidator validatorWithResolver)
    {
        _validator = validator;
    }

    [HttpPost]
    public async Task<ActionResult> CreateCustomer(CreateCustomerRequest request, CancellationToken cancellationToken)
    {
        // This request has already been validated in the middleware
        await _validator.ValidateAsync(request, "se", cancellationToken); // Validate with provided key

        // Since validation in the middleware requires registering a parameter resolver we can also use the parameter-less validate method
        await _validator.ValidateAsync(request, cancellationToken); // Validate using parameter resolver (if using a parameter resolver we can instead inject IValidator without typed parameter)

        return Created("", request);
    }
}
