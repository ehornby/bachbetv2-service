using BachBetV2.WebApi.Extensions;
using BachBetV2.WebApi.Middleware;
using BachBetV2.WebApi.Services;
using FluentValidation.AspNetCore;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;

builder.Services.AddConfigurationOptions(configuration);
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
});

builder.Services.AddServiceRegistrations();
builder.Services.AddDbContexts(configuration);
builder.Services.AddSwaggerGen();
builder.Services.AddFluentValidationAutoValidation().AddFluentValidationClientsideAdapters();
builder.Services.AddValidators();
builder.Services.AddMemoryCache(options =>
{
    options.SizeLimit = 20;
});
builder.Services.AddHostedService<InitializeCacheService>();

// Configure CORS
if (builder.Environment.IsDevelopment())
{
    builder.Services.AddCors(options =>
    {
        options.AddDefaultPolicy(policy =>
        {
            policy.AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
    });

}
else
{
    builder.Services.AddCors(options =>
    {
        options.AddDefaultPolicy(policy =>
        {
            policy.WithOrigins("http://localhost:3000", "http://bachbet.chriscardwell.com", "https://bachbet.chriscardwell.com")
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials();
        });
    });
}

var app = builder.Build();

app.UseHttpsRedirection();
app.UseRouting();
app.UseCors();

if (!builder.Environment.IsDevelopment())
{
    app.UseMiddleware<AuthMiddleware>();
}

if (builder.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.MapGet("/", () =>
{
    return "hello world";
});

app.Run();