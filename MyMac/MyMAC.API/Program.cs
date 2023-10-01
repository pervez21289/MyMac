using MyMac.DAL.Interfaces;
using MyMac.DAL.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddScoped<IMacService, MacService>();

var app = builder.Build();

BaseRepo.ConnectionString = app.Configuration.GetConnectionString("Value");

// Configure the HTTP request pipeline.

app.UseAuthorization();

app.MapControllers();


app.Run();
