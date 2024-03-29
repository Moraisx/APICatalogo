using APICatalogo.Context;
using APICatalogo.DTOs.Mappings;
using APICatalogo.Extensions;
using APICatalogo.Logging;
using APICatalogo.Repository;
using APICatalogo.Services;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
/// <summary>
/// Conven�oes do EF Core => Migrations
/// "Pomelo.EntityFrameworkCore.MySql" Version="6.0.2"
/// Microsoft.EntityFrameworkCore.Design" Version="6.0.3"
/// Microsoft.EntityFrameworkCore.Tools =>  dotnet tool install --global dotnet-ef
/// </summary>

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Equivalente ao ConfigureServices()

//AddJsonOptions(options =>... Ignora o objeto quando um ciclo de refer�ncia � detectado durante a serializa��o.
//Quando duas entidades est�o se referenciando uma com a outra
//Ex:return _context.Categorias.Include(produto => produto.Produtos).ToList();
builder.Services.AddControllers().AddJsonOptions(options => 
options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//string de conex��o com Banco De Dados
string mySqlConnection = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options =>
options.UseMySql(mySqlConnection, ServerVersion.AutoDetect(mySqlConnection)));

//registro UnitOfWork
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

//Necessario para a utiliza��o do [FromServices]
builder.Services.AddTransient<IMeuServico, MeuServico>();

//Addicionando o auto-mapper (DTO => Mapeamento feito atraves da classe MappingProfile)
var mappingConfig = new MapperConfiguration(mc => 
{
    mc.AddProfile(new MappingProfile());
 });
IMapper mapper = mappingConfig.CreateMapper();
builder.Services.AddSingleton(mapper);

//Exemplo de log salvo em txt
var alimentarLog = false;
if (alimentarLog)
{
    builder.Logging.AddProvider(new CustomLoggerProvider(new CustomLoggerProviderConfiguration
    {
        LogLevel = LogLevel.Information
    }));
}

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
