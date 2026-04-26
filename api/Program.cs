using TestDevBackJR.Application.Validators;
using TestDevBackJR.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddDatabaseConfiguration(builder.Configuration);

// Register application services
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