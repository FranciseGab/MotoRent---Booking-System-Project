using Microsoft.EntityFrameworkCore;
using src.db;
using Microsoft.OpenApi.Models;
using src.services;

var builder = WebApplication.CreateBuilder(args);

// Add Controllers
builder.Services.AddControllers();
builder.Services.AddScoped<CustomerService>();
builder.Services.AddScoped<MotorService>();
builder.Services.AddScoped<TransactionService>();

// Add Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Motor Backend API",
        Version = "v1"
    });
});

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy =>
        {
            policy.AllowAnyOrigin()
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});

// PostgreSQL DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(Environment.GetEnvironmentVariable("DATABASE_URL") 
        ?? builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// CREATE DATABASE + SEED DATA
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    DbInit.Initialize(context);
}

// Redirect root "/" to Swagger
app.MapGet("/", () => Results.Redirect("/swagger/index.html"));

// ✅ Swagger accessible sa Railway (hindi lang Development)
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Motor Backend API V1");
});

app.UseCors("AllowAll");
app.UseAuthorization();
app.MapControllers();

app.Run();