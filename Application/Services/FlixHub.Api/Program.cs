var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddEnvironmentVariables("FlixHubKeys:");

// load required keys in order to use it throwgh all application life cycle
string publicId = builder.Configuration["VaultKey1"]!;
string secretId = builder.Configuration["VaultKey2"]!;
DataProtectionProviderExtention.Initialize(publicId, secretId);

// Add services to the container.
builder
    .ConfigureCoreServices(builder.Configuration)

    .Services
    .AddSwagger()
    .AddCors()
    .AddNotificationModule(builder.Configuration)
    .AddCoreServices(builder.Configuration)
    .AddFlixHubModule(builder.Configuration, builder.Environment.IsDevelopment())
    .AddApiHealthChecks(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app
        .UseSwagger()
        .UseSwaggerUI();
}

app.UseCors(policy =>
{
    policy
        .AllowAnyOrigin()
        .AllowAnyHeader()
        .AllowAnyMethod();
});

app.RegisterEndpoints();
app.MapPing();

app
    .RegisterTypedTasks()
    .UseCoreServices(builder.Configuration)
    .UseApiHealthChecks();

app.Run();