# Keycloak Integration

This repository documents my learning process and implementation of Keycloak as an identity provider within a .NET Web API project using .NET Aspire Orchestration. The project demonstrates how to configure and use Keycloak for user authentication and registration using both a confidential and a public client.

## Features
- **Containerized Keycloak Instance** using .NET Aspire
- **Realm Configuration** for user authentication
- **Confidential Client** for user registration (includes saving identity provider ID)
- **Public Client** to allow users to obtain JWT access tokens
- **Endpoints for User Registration and Login**

## API Endpoints
### Register User
#### Request:
```http
POST /register
Content-Type: application/json

{
  "firstName": "Ethem",
  "lastName": "Sahbaz",
  "email": "ethemsahbaz@example.com",
  "password": "Test123!"
}
```
#### Response:
```json
{
  "id": "user-unique-identifier"
}
```

### Login User
#### Request:
```http
POST /login
Content-Type: application/json

{
  "email": "ethemsahbaz@example.com",
  "password": "Test123!"
}
```
#### Response:
```json
{
  "access_token": "jwt-token"
}
```

## Keycloak Authentication Flow
1. **User Registration** → Stored in Keycloak with a unique identity ID.
2. **User Login** → JWT Access Token is returned.
3. **Secure API Requests** → Clients use the JWT token for authorization.

## Configuration (.NET Aspire)
### Adding Keycloak as a Service in .NET Aspire
```csharp
var builder = DistributedApplication.CreateBuilder(args);

var keycloak = builder.AddKeycloak("keycloak", 8080)
    .WithContainerName("AuthenicationAPI.Keycloak")
    .WithDataVolume()
    .WithLifetime(ContainerLifetime.Persistent)
    .WithExternalHttpEndpoints();

builder.AddProject<Projects.AuthenticationAPI_Api>("authenticationapi-api")
    .WithExternalHttpEndpoints()
    .WithReference(keycloak)
    .WaitFor(keycloak);

builder.Build().Run();
```

### Identity Provider Implementation
```csharp
public async Task<string> RegisterUserAsync(string firstName, string lastName, string email, string password)
{
    UserRepresentation userRepresentation = new(
        email,
        email,
        firstName,
        lastName,
        true,
        true,
        [new CredentialRepresentation("password", password, false)]);

    var identityId = await _keycloakService.RegisterUserAsync(userRepresentation);

    return identityId;
}
```

## Testing with Postman
1. **Register a user** using the `/register` endpoint.
2. **Login the user** using the `/login` endpoint.
3. **Use the returned JWT Token** to make authenticated API calls.

## Conclusion
This project is a reference for me to how integrate keycloak as an identity provider. I learned how to setup it with .NET Aspire, configure a realm, configure clients for different use cases and how to setup and use the keycloak api in .NET.
