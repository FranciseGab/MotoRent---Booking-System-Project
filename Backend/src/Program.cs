// using Microsoft.EntityFrameworkCore;
// using src.db;
// using Microsoft.OpenApi.Models;
// using src.services;

// var builder = WebApplication.CreateBuilder(args);

// // Add Controllers
// builder.Services.AddControllers();
// builder.Services.AddScoped<CustomerService>();
// builder.Services.AddScoped<MotorService>();
// builder.Services.AddScoped<TransactionService>();

// // ✅ Add Swagger/OpenAPI
// builder.Services.AddEndpointsApiExplorer();
// builder.Services.AddSwaggerGen(c =>
// {
//     c.SwaggerDoc("v1", new OpenApiInfo
//     {
//         Title = "Motor Backend API",
//         Version = "v1"
//     });
// });

// // CORS
// builder.Services.AddCors(options =>
// {
//     options.AddPolicy("AllowAll",
//         policy =>
//         {
//             policy.AllowAnyOrigin()
//                   .AllowAnyHeader()
//                   .AllowAnyMethod();
//         });
// });

// // SQLite DbContext
// builder.Services.AddDbContext<AppDbContext>(options =>
//     options.UseSqlite("Data Source=db/db.sqlite"));

// var app = builder.Build();

// // 🔥 CREATE DATABASE + SEED DATA
// using (var scope = app.Services.CreateScope())
// {
//     var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
//     DbInit.Initialize(context);
// }

// // 🔹 Redirect root "/" to Swagger
// app.MapGet("/", () => Results.Redirect("/swagger/index.html"));

// // ✅ Swagger middleware
// if (app.Environment.IsDevelopment())
// {
//     app.UseSwagger();
//     app.UseSwaggerUI(c =>
//     {
//         c.SwaggerEndpoint("/swagger/v1/swagger.json", "Motor Backend API V1");
//     });
// }
// app.UseCors("AllowAll");   
// app.UseHttpsRedirection();

// app.UseCors("AllowAll");

// app.UseAuthorization();

// app.MapControllers();

// app.Run();

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

// SQLite DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data Source=db/db.sqlite"));

var app = builder.Build();

// CREATE DATABASE + SEED DATA
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    DbInit.Initialize(context);
}

// Redirect root "/" to Swagger
app.MapGet("/", () => Results.Redirect("/swagger/index.html"));

// Swagger middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Motor Backend API V1");
    });
}

app.UseCors("AllowAll");        // ✅ CORS una, isang beses lang
app.UseAuthorization();         // ✅ pagkatapos
app.MapControllers();           // ✅ controllers

app.Run();