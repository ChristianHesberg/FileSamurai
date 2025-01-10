using System.Text.Json.Serialization;
using api.Policies;
using api.Middleware;
using api.SchemaFilters;
using application.dtos;
using application.ports;
using application.services;
using application.validation;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using infrastructure;
using infrastructure.adapters;
using MicroElements.Swashbuckle.FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<Context>(opt => opt.UseSqlite("Data Source=../database/database.db"));

builder.Services.AddScoped<IUserKeyPairPort, UserKeyPairAdapter>();
builder.Services.AddScoped<IUserKeyPairService, UserKeyPairService>();
builder.Services.AddScoped<IUserPort, UserAdapter>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IFilePort, FileAdapter>();
builder.Services.AddScoped<IFileService, FileService>();
builder.Services.AddScoped<IGroupPort, GroupAdapter>();
builder.Services.AddScoped<IGroupService, GroupService>();

builder.Services.AddScoped<IValidator<AddFileDto>, AddFileDtoValidator>();
builder.Services.AddScoped<IValidator<AddUserToGroupDto>, AddUserToGroupDtoValidator>();
builder.Services.AddScoped<IValidator<GetFileOrAccessInputDto>, GetFileOrAccessInputDtoValidator>();
builder.Services.AddScoped<IValidator<FileDto>, FileDtoValidator>();
builder.Services.AddScoped<IValidator<AddOrGetUserFileAccessDto>, AddOrGetUserFileAccessDtoValidator>();
builder.Services.AddScoped<IValidator<GroupCreationDto>, GroupCreationDtoValidator>();
builder.Services.AddScoped<IValidator<UserRsaKeyPairDto>, UserRsaKeyPairValidator>();
builder.Services.AddScoped<IValidator<UserCreationDto>, UserCreationDtoValidator>();
builder.Services.AddScoped<IValidator<string>, GuidValidator>();
builder.Services.AddScoped<IValidator<string>, EmailValidator>();

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.OperationFilter<AddQueryParameterDescription>();
    options.OperationFilter<GlobalResponseTypeSchemaFilter>();
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
builder.Services.AddFluentValidationRulesToSwagger();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = "https://accounts.google.com";
        options.Audience =
            "503035586312-ujnij8557gd7nga1lbjsvi56vi98iubb.apps.googleusercontent.com";
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidIssuer = "https://accounts.google.com",
            ValidAudience =
                "503035586312-ujnij8557gd7nga1lbjsvi56vi98iubb.apps.googleusercontent.com",
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true
        };
    });

builder.Services.AddHttpContextAccessor();

builder.Services.AddScoped<IAuthorizationHandler, DocumentAccessHandler>();
builder.Services.AddScoped<IAuthorizationHandler, DocumentChangeHandler>();
builder.Services.AddScoped<IAuthorizationHandler, DocumentGetHandler>();
builder.Services.AddScoped<IAuthorizationHandler, DocumentAddHandler>();
builder.Services.AddScoped<IAuthorizationHandler, KeyPairPostHandler>();
builder.Services.AddScoped<IAuthorizationHandler, KeyPairGetPrivateKeyHandler>();
builder.Services.AddScoped<IAuthorizationHandler, GroupAddUserHandler>();
builder.Services.AddScoped<IAuthorizationHandler, DocumentGetUserFileAccessHandler>();
builder.Services.AddScoped<IAuthorizationHandler, DocumentDeleteAccessHandler>();
builder.Services.AddScoped<IAuthorizationHandler, GroupOwnerPolicyHandler>();
builder.Services.AddScoped<IAuthorizationHandler, GroupGetHandler>();
builder.Services.AddScoped<IAuthorizationHandler, UserOwnsResourcePolicyHandler>();
builder.Services.AddScoped<IAuthorizationHandler, GetAllFileAccessPolicyHandler>();
builder.Services.AddScoped<IAuthorizationHandler, ListAccessHandler>();

builder.Services.AddAuthorizationBuilder()
    .AddPolicy("FileAccess", policy =>
        policy.Requirements.Add(new Requirements.DocumentAccessRequirement()))
    .AddPolicy("DocumentChange", policy =>
        policy.Requirements.Add(new Requirements.DocumentChangeRequirement()))
    .AddPolicy("DocumentGet", policy =>
        policy.Requirements.Add(new Requirements.DocumentGetRequirement()))
    .AddPolicy("DocumentAdd", policy =>
        policy.Requirements.Add(new Requirements.DocumentAddRequirement()))
    .AddPolicy("DocumentGetUserFileAccess", policy =>
        policy.Requirements.Add(new Requirements.DocumentGetUserFileAccessRequirement()))
    .AddPolicy("DeleteAccess", policy =>
        policy.Requirements.Add(new Requirements.DocumentDeleteAccessRequirement()))
    .AddPolicy("PostRSAKeyPair", policy =>
        policy.Requirements.Add(new Requirements.KeyPairPostRequirement()))
    .AddPolicy("GetUserPK", policy =>
        policy.Requirements.Add(new Requirements.KeyPairGetPrivateKeyRequirement()))
    .AddPolicy("GroupAddUser", policy =>
        policy.Requirements.Add(new Requirements.GroupAddUserRequirement()))
    .AddPolicy("GroupOwnerPolicy", policy =>
        policy.Requirements.Add(new Requirements.GroupOwnerPolicyRequirement()))
    .AddPolicy("GroupGet", policy =>
        policy.Requirements.Add(new Requirements.GroupGetRequirement()))
    .AddPolicy("OwnsResourcePolicy", policy =>
        policy.Requirements.Add(new Requirements.UserOwnsResourcePolicyRequirement()))
    .AddPolicy("GetAllFileAccessPolicy", policy =>
        policy.Requirements.Add(new Requirements.GetAllFileAccessPolicyRequirement()))
    .AddPolicy("ListAccessPolicy", 
        policy => policy.Requirements.Add(new Requirements.ListAccessPolicyRequirement()));


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