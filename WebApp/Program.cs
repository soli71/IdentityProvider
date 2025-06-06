using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication(opt =>
{
    opt.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    opt.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
})
 .AddCookie("Cookies")
.AddOpenIdConnect(OpenIdConnectDefaults.AuthenticationScheme, opt =>
{
    opt.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    opt.Authority = "https://localhost:7050"; // IdentityServer URL
    opt.ClientId = "web"; // Client ID
    opt.ClientSecret = "secret";
    opt.ResponseType = OpenIdConnectResponseType.Code; // Use authorization code flow
    opt.SaveTokens = true; // Save tokens in the authentication cookie
    opt.UsePkce = true; // Use Proof Key for Code Exchange (PKCE)
    opt.ClaimActions.DeleteClaim("sid");
    opt.Scope.Add("address");
    opt.ClaimActions.MapUniqueJsonKey("address", "address");

    opt.GetClaimsFromUserInfoEndpoint = true;
});
builder.Services.AddHttpContextAccessor();
// Add services to the container.
builder.Services.AddRazorPages();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication(); // Enable authentication middleware
app.UseAuthorization();

app.MapRazorPages();

app.Run();