using TestDevBackJR.Application.Validators;
using TestDevBackJR.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllerConfiguration();
builder.Services.AddOpenApi();
builder.Services.AddDatabaseConfiguration(builder.Configuration);

builder.Services.AddApplicationServices();
builder.Services.AddScoped<LoginValidator>();

var app = builder.Build();

app.MigrateDatabase();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();