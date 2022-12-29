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
        // This code will not run, since we have already validated the model in the middleware

        // This request has already been validated in the middleware
        await _validator.ValidateAsync(request, "se", cancellationToken); // Validate with provided key

        // Since validation in the middleware requires registering a parameter resolver we can also use the parameter-less validate method
        await _validator.ValidateAsync(request, cancellationToken); // Validate using parameter resolver (if using a parameter resolver we can instead inject IValidator without typed parameter)

        // Address can also be validated on it's own
        await _validator.ValidateAsync(request.Address, "se", cancellationToken);

        return Created("", request);
    }

    [HttpPost("{customerId}/address")]
    public async Task<ActionResult> CreateCustomerAddress(int customerId, Address address, CancellationToken cancellationToken)
    {
        // This code will not run, since we have already validated the model in the middleware
        await _validator.ValidateAsync(address, "se", cancellationToken);

        return Created("", address);
    }
}
