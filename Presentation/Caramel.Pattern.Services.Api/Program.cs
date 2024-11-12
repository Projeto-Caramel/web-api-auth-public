using Caramel.Pattern.Integrations.UsersControl;
using Caramel.Pattern.Services.Api.Configurators;
using Caramel.Pattern.Services.Api.Middlewares;
using Caramel.Pattern.Services.Application;
using Caramel.Pattern.Services.Domain.AutoMapper;
using Caramel.Pattern.Services.Infra;
using System.Text.Encodings.Web;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("CORS-RULES", policy =>
    {
        policy
        .WithOrigins(
            "https://*",
            "http://*")
        .WithExposedHeaders("*")
        .WithHeaders(
            "content-type",
            "authorization",
            "bearer")
        .AllowAnyMethod();
    });
});

// HttpClient
builder.Services.AddHttpClient("AuthClient", c =>
{
    c.BaseAddress = new Uri(builder.Configuration["APIs:AuthUrl"]);
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

// Swashbuckle - API Documentation
builder.Services.ConfigureSwagger(builder.Environment.EnvironmentName);

// Add Authentication and Authorization
builder.Services.ConfigureAuth(builder.Configuration);

// Exceptions Handlers
builder.Services.AddExceptionHandler<BusinessExceptionHandler>();
builder.Services.AddExceptionHandler<DatabaseExceptionHandler>();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

// Serialization Pattern
builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
    options.SerializerOptions.WriteIndented = true;
    options.SerializerOptions.Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping;
});

// AutoMapper
builder.Services.AddAutoMapper(typeof(MappingProfile));

// Layer's Dependency
builder.Services.AddDatabaseModule(builder.Configuration);
builder.Services.AddApplicationModule();
builder.Services.AddUsersControlModule();

builder.Services.AddAWSLambdaHosting(LambdaEventSource.HttpApi);

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

// Apply CORS policy
app.UseCors("CORS-RULES");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// Exceptions Handlers
app.UseExceptionHandler();

//Swashbuckle - API Documentation
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.RoutePrefix = "swagger";
});

app.Run();
