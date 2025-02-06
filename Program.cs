using Fiap.Api.Data;
using Fiap.Api.Handlers;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Configuração do DbContext com a conexão ao banco
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));


// Registrar outros serviços necessários (como repositórios, etc.)
builder.Services.AddScoped<IMedicoHandler, MedicoHandler>();

// Add services to the container.
builder.Services.AddControllers();

// Configuração do Swagger para documentação de APIs
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configurar o pipeline de requisição HTTP
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

// Mapear os controllers da aplicação
app.MapControllers();

app.Run();
