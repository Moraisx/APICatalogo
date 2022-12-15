using APICatalogo.Context;
using Microsoft.EntityFrameworkCore;
/// <summary>
/// Convençoes do EF Core => Migrations
/// "Pomelo.EntityFrameworkCore.MySql" Version="6.0.2"
/// Microsoft.EntityFrameworkCore.Design" Version="6.0.3"
/// Microsoft.EntityFrameworkCore.Tools =>  dotnet tool install --global dotnet-ef
/// </summary>
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Equivalente ao ConfigureServices()

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//string de conexção com Banco De Dados
string mySqlConnection = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options =>
options.UseMySql(mySqlConnection, ServerVersion.AutoDetect(mySqlConnection)));

var app = builder.Build();

// Configure the HTTP request pipeline.
//Equivalente ao Configure()

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
