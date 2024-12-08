using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Adiciona controllers ao container de DI
builder.Services.AddControllers();

// Configura o Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Processos API",
        Version = "v1",
        Description = "API para gerenciamento de processos",
        Contact = new OpenApiContact
        {
            Name = "Seu Nome",
            Email = "seu.email@exemplo.com"
        }
    });
});

var app = builder.Build();

// Configura o pipeline HTTP
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Processos API V1");
        c.RoutePrefix = string.Empty; // Faz o Swagger UI aparecer na raiz
    });
}

app.UseRouting();
app.MapControllers();

app.Run();