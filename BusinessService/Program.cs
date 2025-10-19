using BusinessService.Data;
using BusinessService.Repositories;
using BusinessService.Repositories.IRepositories;
using Common.Extensions;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddDbContextFactory<BusinessDbContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("LocalConnection")), ServiceLifetime.Scoped);
builder.Services.AddJwtAuthentication(builder.Configuration);

builder.Services.AddScoped<ISharedRepository, SharedRepository>();
builder.Services.AddScoped<IStoreRepository, StoreRepository>();
builder.Services.AddScoped<IStoreTagRepository, StoreTagRepository>();
builder.Services.AddScoped<IStoreCategoryRepository, StoreCategoryRepository>();
builder.Services.AddScoped<IStoreTagRelationRepository, StoreTagRelationRepository>();
builder.Services.AddScoped<IStoreCategoryRelationRepository, StoreCategoryRelationRepository>();

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
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
AddlyMigration();
app.Run();
void AddlyMigration()
{
    using (var scope = app.Services.CreateScope())
    {
        var _db = scope.ServiceProvider.GetRequiredService<BusinessDbContext>();
        if (_db.Database.GetPendingMigrations().Count() > 0)
        {
            _db.Database.Migrate();
        }
    }
}