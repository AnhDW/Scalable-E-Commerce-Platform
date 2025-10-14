using AuthService.Data;
using AuthService.Profiles;
using AuthService.Services;
using AuthService.Services.IServices;
using Contracts.Entities;
using Microsoft.EntityFrameworkCore;
using System;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddDbContextFactory<AuthDbContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("LocalConnection")), ServiceLifetime.Scoped);
builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection("ApiSettings:JwtOptions"));

builder.Services.AddScoped<IAuthService, AuthService.Services.AuthService>();
builder.Services.AddScoped<IUserService, UserService>();

var assemblies = AppDomain.CurrentDomain.GetAssemblies()
    .Where(a => !a.FullName.StartsWith("Microsoft.Data.SqlClient"))
    .ToArray();

builder.Services.AddAutoMapper(cfg => cfg.AddMaps(assemblies));
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
