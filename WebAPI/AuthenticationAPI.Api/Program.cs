using AuthenticationAPI.Api.Application.Identity;
using AuthenticationAPI.Api.Extensions;
using AuthenticationAPI.Api.Middlewares;
using AuthenticationAPI.Api.Models;

var builder = WebApplication.CreateBuilder(args);

// Goals:
// Add Keycloak via .NET Aspire ☑️
// Configure Confidential Client ☑️
// Create Endpoint for registering ☑️
// Create Endpoint for logging in
// Create Endpoint /me

// For .NET Aspire
builder.AddServiceDefaults();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

builder.Services.AddKeycloakAuthentication(builder.Configuration);

var app = builder.Build();

app.MapDefaultEndpoints();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapPost("/register", async (RegisterRequest request, IIdentityProvider identityProvider) =>
{
    var identityId = await identityProvider.RegisterUserAsync(
        request.FirstName,
        request.LastName,
        request.Email,
        request.Password);

    return !string.IsNullOrEmpty(identityId) ? Results.Ok(identityId) : Results.Problem();
})
.WithName("Register")
.WithTags("Users")
.WithOpenApi();

app.MapPost("/login", async (RegisterRequest request, IIdentityProvider identityProvider) =>
{
    var identityId = await identityProvider.RegisterUserAsync(
        request.FirstName,
        request.LastName,
        request.Email,
        request.Password);

    return !string.IsNullOrEmpty(identityId) ? Results.Ok(identityId) : Results.Problem();
})
.WithName("Register")
.WithTags("Users")
.WithOpenApi();

app.UseExceptionHandler();

app.Run();