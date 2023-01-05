using ValidatR.AspNet;
using ValidatR.DependencyInjection;
using ValidatR.Enums;
using ValidatR.Examples.Services;
using ValidatR.Examples.Viewmodels;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// The T in this case refers to the type for the parameter for retrieving the validator rule value
builder.Services.AddValidatR<string>()
    .AddParameterResolver<CreateCustomerRequest>(x => x.FirstName) // We need to register parameter resolvers if we want to use Validate without providing a parameter (required by middleware)
    .AddParameterResolver<CreateItemRequest>(x => x.Name)
    .AddParameterResolver<Address>(x => x.Street);
builder.Services.AddTransient<IStorageService, StorageService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

// This automatically registers an instance of the middleware per type
// To be able to use a middleware to perform validation this is nesscecary since modelbinding has not yet occured.
// The other option would be action filters
// The types is optional and should only be used when manually wanting to control which types get a middleware, otherwise an assembly scan will be performed registering all types using the ValidateAttribute
app.UseValidatorMiddleware(typeof(CreateCustomerRequest), typeof(Address));

// Example registration where a storageService is implemented handing retriving the rule values to use based on id, validator and parameter
var storageService = app.Services.GetRequiredService<IStorageService>();
app.UseValidatR<string>()
    .AddMinLengthValidator((id, parameter) => storageService.GetValidationRuleValue<int>(id, ValidatorType.MinLength, parameter))
    .AddMaxLengthValidator((id, parameter) => storageService.GetValidationRuleValue<int>(id, ValidatorType.MaxLength, parameter))
    .AddRegexValidator((id, parameter) => storageService.GetValidationRuleValue<string>(id, ValidatorType.Regex, parameter))
    .AddRequiredValidator((id, parameter) => storageService.GetValidationRuleValue<bool>(id, ValidatorType.Required, parameter));

app.MapControllers();

app.Run();
