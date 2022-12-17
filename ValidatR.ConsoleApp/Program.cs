// See https://aka.ms/new-console-template for more information
using ValidatR.ConsoleApp.Models;

Console.WriteLine("Hello, World!");

var validator = new ValidatR.Validator<string>((name, type, parameter) =>
{
    Console.WriteLine($"Func called for '{name}' from type '{type}', with parameter '{parameter}'");

    if (type == ValidatR.Enums.ValidatorType.Regex)
    {
        return @"\d\d";
    }
    else if (type == ValidatR.Enums.ValidatorType.MaxLength)
    {
        return "3";
    }

    throw new Exception("dökdök");
});
validator.ValidateAsync(new CreateCustomerRequest("Dennis", "Hogström", 10, false), "testKey", CancellationToken.None).GetAwaiter().GetResult();