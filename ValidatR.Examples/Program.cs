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
builder.Services.AddValidatR<string>()
    .AddParameterResolver<CreateCustomerRequest>(x => x.FirstName)
    .AddParameterResolver<CreateItemRequest>(x => x.Name);
builder.Services.AddTransient<IStorageService, StorageService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

var storageService = app.Services.GetRequiredService<IStorageService>();
//return storageService.GetValidationRuleValue(name, type, parameter);
app.UseValidatorMiddleware<CreateCustomerRequest>();
app.UseValidatR<string>()
    .AddMinLengthValidator((id, parameter) => storageService.GetValidationRuleValue<int>(id, ValidatorType.MinLength, parameter))
    .AddMaxLengthValidator((id, parameter) => storageService.GetValidationRuleValue<int>(id, ValidatorType.MaxLength, parameter))
    .AddRegexValidator((id, parameter) => storageService.GetValidationRuleValue<string>(id, ValidatorType.Regex, parameter))
    .AddRequiredValidator((id, parameter) => storageService.GetValidationRuleValue<bool>(id, ValidatorType.Required, parameter));

app.MapControllers();

app.Run();
