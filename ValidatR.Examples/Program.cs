using ValidatR.AspNet;
using ValidatR.DependencyInjection;
using ValidatR.Examples.Services;
using ValidatR.Examples.Viewmodels;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddValidatR<string>().AddParameterResolver<CreateCustomerRequest>(x => x.FirstName);
builder.Services.AddTransient<IStorageService, StorageService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();
app.UseValidatorMiddleware<CreateCustomerRequest>();
app.UseValidatR<string>((name, type, parameter) =>
{
    var storageService = app.Services.GetRequiredService<IStorageService>();
    return storageService.GetValidationRuleValue(name, type, parameter);
});

app.MapControllers();

app.Run();
