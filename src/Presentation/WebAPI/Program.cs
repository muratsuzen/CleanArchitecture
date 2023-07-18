using Application;
using Application.Middlewares;
using Persistence;
using WebAPI.Extensions;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddStackExchangeRedisCache(opt=>opt.Configuration= "localhost:6379,password=msuzen123!");
builder.Services.AddPersistenceServices(builder.Configuration);
builder.Services.AddApplicationServices();

builder.Services.ConfigureApiBehavior();
builder.Services.ConfigureCorsPolicy();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//Custom Exception Middleware
app.UseMiddleware<ExceptionMiddleware>();
app.UseAuthorization();
app.UseErrorHandler();
//app.UseCors();
app.MapControllers();

app.Run();
