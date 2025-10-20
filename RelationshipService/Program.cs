using Common.Extensions;
using Microsoft.EntityFrameworkCore;
using RelationshipService.Data;
using RelationshipService.Repositories;
using RelationshipService.Repositories.IRepositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddDbContextFactory<RelationshipDbContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("LocalConnection")), ServiceLifetime.Scoped);
builder.Services.AddJwtAuthentication(builder.Configuration);

builder.Services.AddScoped<IUserStoreRelationRepository, UserStoreRelationRepository>();

var assemblies = AppDomain.CurrentDomain.GetAssemblies()
    .Where(a => !a.FullName.StartsWith("Microsoft.Data.SqlClient"))
    .ToArray();

builder.Services.AddAutoMapper(cfg => cfg.AddMaps(assemblies));
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
