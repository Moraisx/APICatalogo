using APICatalogo.Context;
using APICatalogo.Extensions;
using APICatalogo.Services;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
/// <summary>
/// Convençoes do EF Core => Migrations
/// "Pomelo.EntityFrameworkCore.MySql" Version="6.0.2"
/// Microsoft.EntityFrameworkCore.Design" Version="6.0.3"
/// Microsoft.EntityFrameworkCore.Tools =>  dotnet tool install --global dotnet-ef
/// </summary>
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Equivalente ao ConfigureServices()

//AddJsonOptions(options =>... Ignora o objeto quando um ciclo de referência é detectado durante a serialização.
//Quando duas entidades estão se referenciando uma com a outra
//Ex:return _context.Categorias.Include(produto => produto.Produtos).ToList();
builder.Services.AddControllers().AddJsonOptions(options => 
options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//string de conexção com Banco De Dados
string mySqlConnection = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options =>
options.UseMySql(mySqlConnection, ServerVersion.AutoDetect(mySqlConnection)));

//Necessario para a utilização do [FromServices]
builder.Services.AddTransient<IMeuServico, MeuServico>();

var app = builder.Build();

// Configure the HTTP request pipeline.
//Equivalente ao Configure()

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.ConfigureExceptionHandler();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
