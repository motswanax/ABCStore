using Application;
using Application.Features.Categories.Validations;

using FluentValidation;

using Infrastructure;

using NSwag.AspNetCore;

using WebApi;
using WebApi.Filters;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

// Add services to the container.

builder.Services.AddControllers(options =>
{
    options.Filters.Add<ValidateFluentValidationFilter>();
});
builder.Services.AddInfrastructureServices(builder.Configuration);

builder.Services.AddOpenApiDocument(options =>
{
    options.Title = "ABCStore API";
});

builder.Services.AddValidatorsFromAssemblyContaining<CreateCategoryRequestValidator>();
builder.Services.AddScoped<ValidateFluentValidationFilter>();

builder.Services.AddApplicationServices();

var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseOpenApi(settings =>
    {
        settings.Path = "/openapi/v1.json";
    });

    app.UseSwaggerUi(settings =>
    {
        settings.Path = "/swagger";
        settings.DocumentPath = "/openapi/v1.json";
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
