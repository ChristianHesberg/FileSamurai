
using System.Text;
using api.Policies;
using api.Middleware;
using application.dtos;
using application.ports;
using application.services;
using application.validation;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using infrastructure;
using infrastructure.adapters;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

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

builder.Services.AddScoped<IValidator<AddFileDto>, AddFileDtoValidator>();
builder.Services.AddScoped<IValidator<GetFileOrAccessInputDto>, GetFileOrAccessInputDtoValidator>();
builder.Services.AddScoped<IValidator<FileDto>, FileDtoValidator>();
builder.Services.AddScoped<IValidator<AddOrGetUserFileAccessDto>, AddOrGetUserFileAccessDtoValidator>();

builder.Services.AddControllers().AddJsonOptions(options =>  
{  
    options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;  
}); 

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = "https://accounts.google.com";
        options.Audience = "503035586312-ujnij8557gd7nga1lbjsvi56vi98iubb.apps.googleusercontent.com"; // Replace with your client ID
        options.TokenValidationParameters = new TokenValidationParameters
        {
            
            ValidIssuer = "https://accounts.google.com",
            ValidAudience = "503035586312-ujnij8557gd7nga1lbjsvi56vi98iubb.apps.googleusercontent.com", // Replace with your client ID
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = false, //TODO SET TRUE ONLY FALSE FOR TESTING
            ValidateIssuerSigningKey = true
        };
    });

builder.Services.AddHttpContextAccessor();

builder.Services.AddScoped<IAuthorizationHandler, DocumentAccessHandler>();
builder.Services.AddScoped<IAuthorizationHandler, DocumentChangeHandler>();
builder.Services.AddScoped<IAuthorizationHandler, DocumentGetHandler>();
builder.Services.AddScoped<IAuthorizationHandler, DocumentAddHandler>();
builder.Services.AddScoped<IAuthorizationHandler, KeyPairPostHandler>();
builder.Services.AddScoped<IAuthorizationHandler, KeyPairGetPKHandler>();
builder.Services.AddScoped<IAuthorizationHandler, GroupAddUserHandler>();

builder.Services.AddAuthorizationBuilder()
    .AddPolicy("FileAccess", policy =>
        policy.Requirements.Add(new DocumentAccessRequirement()))
    .AddPolicy("DocumentChange", policy =>
    policy.Requirements.Add(new DocumentChangeRequirement()))
    .AddPolicy("DocumentGet", policy =>
        policy.Requirements.Add(new DocumentGetRequirement()))
    .AddPolicy("DocumentAdd", policy =>
        policy.Requirements.Add(new DocumentAddRequirement()))
    .AddPolicy("DocumentGetUserFileAccess", policy =>
        policy.Requirements.Add(new DocumentGetUserFileAccessRequirement()))
    .AddPolicy("PostRSAKeyPair", policy =>
        policy.Requirements.Add(new KeyPairPostRequirement()))
    .AddPolicy("GetUserPK", policy =>
    policy.Requirements.Add(new KeyPairGetPKRequirement()))
    .AddPolicy("GroupAddUser", policy =>
    policy.Requirements.Add(new GroupAddUserRequirement()));


builder.Services.AddCors(options =>  
{  
    options.AddPolicy("cors", builder =>  
    {  
        builder.WithOrigins("http://localhost:3000")  
            .AllowAnyMethod()  
            .AllowAnyHeader();
    });  
});  

var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>(); 

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


app.UseCors("cors"); 

app.UseAuthorization();

app.MapControllers();

app.Run();
