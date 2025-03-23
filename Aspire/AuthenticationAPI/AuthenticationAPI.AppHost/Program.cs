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
