using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ProductAPIVS.Container;
using SalesOrderAPI.Models;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// register db Context
builder.Services.AddDbContext<SalesDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("connection"));
});

builder.Services.AddTransient<ICustomerContainer, CustomerContainer>();
builder.Services.AddTransient<IInvoiceContainer, InvoiceContainer>();
builder.Services.AddTransient<IProductContainer, ProductContainer>();

var automapper = new MapperConfiguration(item => item.AddProfile(new MappingProfile()));
IMapper mapper = automapper.CreateMapper();
builder.Services.AddSingleton(mapper);

// individual logging
// Log.Logger = new LoggerConfiguration().WriteTo.File("/Users/ahumibee/Desktop/Logs/ApiLog-.log", rollingInterval: RollingInterval.Day).CreateLogger();

// global logging
// var _logger = new LoggerConfiguration()
// .MinimumLevel.Error()
// .WriteTo.File("/Users/ahumibee/Desktop/Logs/ApiLog-.log", rollingInterval: RollingInterval.Day).CreateLogger();
// builder.Logging.AddSerilog(_logger);

// logging from appsettings file
var _logger = new LoggerConfiguration()
.ReadFrom.Configuration(builder.Configuration)
.Enrich.FromLogContext()
.CreateLogger();
builder.Logging.AddSerilog(_logger);


/*
// to test if the appsettings.json file is being read
Log.Logger = new LoggerConfiguration()
    .WriteTo.File("log.txt")
    .CreateLogger();
var connectionString23 = builder.Configuration.GetConnectionString("connection");
Log.Information($"Connection string is..................: {connectionString23}");
*/

var app = builder.Build();

// Configure the HTTP request pipeline.
// if (app.Environment.IsDevelopment())
// {
app.UseSwagger();
app.UseSwaggerUI();
// }

app.UseStaticFiles();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
