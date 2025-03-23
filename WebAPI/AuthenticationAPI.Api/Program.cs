using AuthenticationAPI.Api.Middlewares;
using AuthenticationAPI.Api.Models;

var builder = WebApplication.CreateBuilder(args);

// Goals:
// Add Keycloak via .NET Aspire ☑️
// Configure Confidential Client -
// Create Endpoint for registering
// Create Endpoint for logging in
// Create Endpoint /me

// For .NET Aspire
builder.AddServiceDefaults();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
var app = builder.Build();

app.MapDefaultEndpoints();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/register", async (RegisterRequest request) =>
{
    return "Hi";
})
.WithName("Register")
.WithTags("Users")
.WithOpenApi();

app.UseExceptionHandler();

app.Run();