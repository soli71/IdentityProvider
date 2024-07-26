using IdentityServer;
using IdentityServer.Models;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

Debugger.Launch();
builder.Configuration.AddJsonFile("appsettings.Development.json", optional: false, reloadOnChange: true);
var migrationsAssembly = typeof(Program).Assembly.GetName().Name;

string connectionString = builder.Configuration.GetConnectionString("cnn");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(connectionString));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddIdentity<User, Role>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddRazorPages();
var idsBuilder = builder.Services.AddIdentityServer()
          .AddConfigurationStore(options =>
          {
              options.ConfigureDbContext = b => b.UseSqlServer(connectionString,
                  sql => sql.MigrationsAssembly(migrationsAssembly));
          })
          .AddOperationalStore(options =>
          {
              options.ConfigureDbContext = b => b.UseSqlServer(connectionString,
                  sql => sql.MigrationsAssembly(migrationsAssembly));
          });
idsBuilder.AddAspNetIdentity<User>();

var app = builder.Build();

Initialize.Start(app);
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseIdentityServer();
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();