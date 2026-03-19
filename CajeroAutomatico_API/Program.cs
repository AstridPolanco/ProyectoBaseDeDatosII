
using CajeroAutomaticoAPI.Data;
using CajeroAutomaticoAPI.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddSingleton<DatabaseConnection>();
builder.Services.AddScoped<TarjetaService>();
builder.Services.AddScoped<TransaccionService>();
builder.Services.AddScoped<CuentaHabienteService>();
builder.Services.AddScoped<CuentaService>();
builder.Services.AddScoped<BitacoraService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new()
    {
        Title = "Cajero Automático API",
        Version = "v1",
        Description = "API REST para el simulador de cajero automático - Base de Datos II"
    });
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader());
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Cajero API v1");
    c.RoutePrefix = "swagger";
});

app.UseCors("AllowAll");
app.UseAuthorization();
app.UseDefaultFiles();
app.UseStaticFiles();
app.MapControllers();
app.Run();