using OsirisOnlineBetting.Controllers;
using OsirisOnlineBetting.Interfaces;
using OsirisOnlineBetting.Middleware;
using OsirisOnlineBetting.Repositories;
using OsirisOnlineBetting.Services;
using RabbitMQ.Client;
using System.Data;
using System.Data.SqlClient;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IDbConnection>(sp => 
    new SqlConnection(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddSingleton<IConnectionFactory>(sp =>
{
    var config = builder.Configuration;
    return new ConnectionFactory
    {
        HostName = config.GetConnectionString("RabbitMqConnectionString"),
        UserName = config.GetSection("RabbitMqCredentials")["Username"],
        Password = config.GetSection("RabbitMqCredentials")["Password"]
    };
});

builder.Services.AddScoped<ICasinoRepository, CasinoRepository>();
builder.Services.AddScoped<ICasinoService, CasinoService>();
builder.Services.AddScoped<IRabbitMqService, RabbitMqService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ExceptionMiddleware>();
app.ConfigureCasinoWagerEndpoints();

app.Run();

