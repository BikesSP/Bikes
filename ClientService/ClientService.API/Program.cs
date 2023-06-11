using ClientService.API;
using ClientService.API.Filters;
using ClientService.Applcation;
using ClientService.Application;
using ClientService.Application.Common.Exceptions;
using ClientService.Infrastructure;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using ServiceStack;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Configuration;
using System.Net;

var builder = WebApplication.CreateBuilder(args);
builder.Logging.AddConsole();

var services = builder.Services;
var configuration = builder.Configuration;

// Add services to the container.

services.AddControllers();
services.AddHttpContextAccessor();
services.AddHttpClient();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(otp =>  
{
    otp.SwaggerDoc("v1", new OpenApiInfo()
    {
        Title = "Biké API",
        Version = "v1"
    });

    otp.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "bearer"
    });

    //Filter security
    otp.OperationFilter<AuthorizationOperationFilter>();
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowOrigin",
        builder =>
        {
            builder.AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();
        });
});

builder.Services.ConfigureApiServices(builder.Configuration);

builder.Services.ConfigureApplicationServices(builder.Configuration);

builder.Services.ConfigureInfrastructureServices(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseCors("AllowOrigin");
}

app.UseHttpsRedirection();

app.UseMiddleware<ExceptionMiddleware>();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
