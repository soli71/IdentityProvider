using IdentityProvider;
using IdentityProvider.Models;
using Microsoft.EntityFrameworkCore;
using System.Net;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<ISecurityService, SecurityService>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();
builder.Services.AddControllers(config =>
{
    config.Filters.Add(typeof(Permission));
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwagger();
builder.Services.AddDbContext<AppDbContext>(opt =>
{
    string connectionString = builder.Configuration.GetConnectionString("ConnectionString");
    opt.UseSqlServer(connectionString);
});

builder.Services.AddPermissions();
var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();
app.Use(async (context, next) =>
{
    try
    {
        await next(context);
    }
    catch (UnAuthorizeException ex)
    {
        context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
    }
    catch (UnauthorizedAccessException ex)
    {
        context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
    }

});
var dbcontext = builder.Services.BuildServiceProvider().GetRequiredService<AppDbContext>();
ApplicationDbInitializer.SeedAdminUser(dbcontext);
app.MapControllers();
if (app.Environment.IsDevelopment())
{
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
    });
    app.UseSwagger(x => x.SerializeAsV2 = true);
}
AuthenticationExtensions.UseAuthentication(app);
app.Run();

