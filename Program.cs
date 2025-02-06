using Fiap.Api.Data;
using Fiap.Api.Handlers;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Configura��o do DbContext com a conex�o ao banco
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));


// Registrar outros servi�os necess�rios (como reposit�rios, etc.)
builder.Services.AddScoped<IMedicoHandler, MedicoHandler>();

// Add services to the container.
builder.Services.AddControllers();

// Configura��o do Swagger para documenta��o de APIs
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configurar o pipeline de requisi��o HTTP
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

// Mapear os controllers da aplica��o
app.MapControllers();

app.Run();
