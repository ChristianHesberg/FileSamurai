using application.ports;
using application.services;
using Microsoft.EntityFrameworkCore;
using infrastructure;
using infrastructure.adapters;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<Context>(opt => opt.UseSqlite("Data Source=../database/database.db"));

builder.Services.AddScoped<IUserKeyPairPort, UserKeyPairKeyPairAdapter>();
builder.Services.AddScoped<IUserKeyPairService, UserKeyPairKeyPairService>();
builder.Services.AddScoped<IUserPort, UserAdapter>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IFilePort, FileAdapter>();
builder.Services.AddScoped<IFileService, FileService>();
builder.Services.AddScoped<IGroupPort, GroupAdapter>();
builder.Services.AddScoped<IGroupService, GroupService>();

builder.Services.AddControllers().AddJsonOptions(options =>  
{  
    options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;  
}); 
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();