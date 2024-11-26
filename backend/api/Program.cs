using application;
using core.application_interfaces;
using core.ports;
using core.services;
using Microsoft.EntityFrameworkCore;
using shared;
using shared.adapters;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<Context>(opt => opt.UseSqlite("Data Source=../database/database.db"));

builder.Services.AddScoped<IUserPort, UserAdapter>();
builder.Services.AddScoped<ICryptography, Cryptography>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddTransient<IDbInitializer, DbInitializer>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    
    using (var scope = app.Services.CreateScope())
    {
        var services = scope.ServiceProvider;
        var dbContext = services.GetService<Context>();
        var dbInitializer = services.GetService<IDbInitializer>();
        dbInitializer.Initialize(dbContext);
    }
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();