using Infrastructure.Data;
using Infrastructure.Repositories;
using Domain.Repositories;
using Application.UseCases.Usuarios;
using Application.UseCases.Livros;
using Application.UseCases.Emprestimos;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Configure MongoDB
builder.Services.Configure<MongoDbSettings>(
    builder.Configuration.GetSection("MongoDbSettings"));

builder.Services.AddSingleton<MongoDbContext>();

// Register repositories
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<ILivroRepository, LivroRepository>();
builder.Services.AddScoped<IEmprestimoRepository, EmprestimoRepository>();

// Register use cases
builder.Services.AddScoped<CriarUsuarioUseCase>();
builder.Services.AddScoped<ObterUsuarioUseCase>();
builder.Services.AddScoped<CriarLivroUseCase>();
builder.Services.AddScoped<CriarEmprestimoUseCase>();

// Configure Health Checks
builder.Services.AddHealthChecks()
    .AddCheck("mongodb", () =>
    {
        try
        {
            var context = builder.Services.BuildServiceProvider().GetRequiredService<MongoDbContext>();
            context.Database.RunCommand<MongoDB.Bson.BsonDocument>(new MongoDB.Bson.BsonDocument("ping", 1));
            return HealthCheckResult.Healthy("MongoDB está conectado e respondendo");
        }
        catch (Exception ex)
        {
            return HealthCheckResult.Unhealthy("MongoDB não está disponível", ex);
        }
    })
    .AddCheck("api", () => HealthCheckResult.Healthy("API está funcionando"));

// Configure Swagger/OpenAPI with versioning
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Biblioteca API - Clean Architecture + DDD + MongoDB",
        Version = "v1",
        Description = @"API REST para gerenciamento de biblioteca desenvolvida com:
        
- **Clean Architecture**: Separação em camadas (Domain, Application, Infrastructure, API)
- **Domain-Driven Design (DDD)**: Entidades ricas, Value Objects, Agregados
- **MongoDB**: Banco de dados NoSQL para persistência
- **Health Check**: Monitoramento da aplicação e banco de dados
- **Clean Code**: Princípios SRP, DRY, KISS, YAGNI

## Funcionalidades

- Gestão de Usuários (CRUD completo)
- Gestão de Livros (CRUD completo)
- Sistema de Empréstimos com regras de negócio
- Renovações e devoluções
- Cálculo de multas por atraso
- Consultas e relatórios

## Endpoints Principais

- `/api/Usuarios` - Gerenciamento de usuários
- `/api/Livros` - Gerenciamento de livros
- `/api/Emprestimos` - Gerenciamento de empréstimos
- `/health` - Status da aplicação e MongoDB",
        Contact = new Microsoft.OpenApi.Models.OpenApiContact
        {
            Name = "Biblioteca API - CP5",
            Email = "biblioteca@fiap.com.br"
        }
    });

    // Adicionar exemplos e descrições detalhadas
    c.EnableAnnotations();
});

// Configure CORS
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Biblioteca API v1");
    c.RoutePrefix = string.Empty; // Swagger na raiz
    c.DocumentTitle = "Biblioteca API - Documentação";
});

// Configure Health Check endpoint
app.MapHealthChecks("/health", new HealthCheckOptions
{
    ResponseWriter = async (context, report) =>
    {
        context.Response.ContentType = "application/json";
        var result = JsonSerializer.Serialize(new
        {
            status = report.Status.ToString(),
            checks = report.Entries.Select(e => new
            {
                name = e.Key,
                status = e.Value.Status.ToString(),
                description = e.Value.Description,
                duration = e.Value.Duration.TotalMilliseconds
            }),
            totalDuration = report.TotalDuration.TotalMilliseconds
        }, new JsonSerializerOptions
        {
            WriteIndented = true
        });
        await context.Response.WriteAsync(result);
    }
});

app.UseHttpsRedirection();
app.UseCors();
app.UseAuthorization();
app.MapControllers();

// Log startup information
var logger = app.Services.GetRequiredService<ILogger<Program>>();
logger.LogInformation("🚀 Biblioteca API iniciada com sucesso!");
logger.LogInformation("📚 MongoDB configurado");
logger.LogInformation("✅ Health Check disponível em /health");
logger.LogInformation("📖 Swagger disponível em /");

app.Run("http://0.0.0.0:5001");
