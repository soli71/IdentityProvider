using Duende.IdentityModel;
using Duende.IdentityServer;
using Duende.IdentityServer.EntityFramework.DbContexts;
using Duende.IdentityServer.EntityFramework.Mappers;
using Duende.IdentityServer.Models;
using IdentityServer.Models;
using Microsoft.AspNetCore.Identity;

namespace IdentityServer
{
    public static class Config
    {
        public static IEnumerable<IdentityResource> IdentityResources =>
            new IdentityResource[]
            {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile(),
            new IdentityResource()
            {
                Name = "verification",
                UserClaims = new List<string>
                {
                    JwtClaimTypes.Email,
                    JwtClaimTypes.EmailVerified
                }
            }
            };

        public static IEnumerable<ApiScope> ApiScopes =>
            new ApiScope[]
            {
            new ApiScope(name: "api1", displayName: "My API")
            };

        public static IEnumerable<ApiResource> ApiResources =>
            [new ApiResource("api", "test api") { Scopes = { "api1" } }];

        public static IEnumerable<Client> Clients =>
            new Client[]
            {
            new Client
            {
                ClientId = "client",

                // no interactive user, use the clientid/secret for authentication
                AllowedGrantTypes = GrantTypes.ClientCredentials,

                // secret for authentication
                ClientSecrets =
                {
                    new Secret("secret".Sha512())
                },

                // scopes that client has access to
                AllowedScopes = { "api1" }
            },
            // interactive ASP.NET Core Web App
            new Client
            {
                ClientId = "web",
                ClientSecrets = { new Secret("secret".Sha512()) },

                AllowedGrantTypes = GrantTypes.Code,

                // where to redirect to after login
                RedirectUris = { "https://localhost:5002/signin-oidc" },

                // where to redirect to after logout
                PostLogoutRedirectUris = { "https://localhost:5002/signout-callback-oidc" },

                AllowOfflineAccess = false,

                RequireConsent = true,

                RequirePkce=true,

                AllowedScopes =
                {
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Profile,
                    "api1"
                }
            }
            };
    }

    public static class Initialize
    {
        public static void Start(WebApplication app)
        {
            using var scope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ConfigurationDbContext>();

            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();

            if (!userManager.Users.Any())
                userManager.CreateAsync(new User
                {
                    UserName = "admin",
                    Email = "test@admin.com"
                }, "Admin@1234").GetAwaiter().GetResult();

            InitializeClientData(app, context);
            InitializeApiScopes(context);
            InitializeIdentityResources(context);
            InitializeApiResource(context);
        }

        private static void InitializeIdentityResources(ConfigurationDbContext context)
        {
            if (!context.IdentityResources.Any())
            {
                foreach (var resource in Config.IdentityResources)
                {
                    context.IdentityResources.Add(resource.ToEntity());
                }
                context.SaveChanges();
            }
        }

        private static void InitializeApiResource(ConfigurationDbContext context)
        {
            if (!context.ApiResources.Any())
            {
                foreach (var resource in Config.ApiResources)
                    context.ApiResources.Add(resource.ToEntity());
            }
            context.SaveChanges();
        }

        private static void InitializeClientData(WebApplication app, ConfigurationDbContext context)
        {
            var clients = Config.Clients.ToList();

            if (!context.Clients.Any())
            {
                ProcessAndAddClients(context, clients);
                context.SaveChanges();
            }
        }

        private static void ProcessAndAddClients(ConfigurationDbContext context, List<Client> clients)
        {
            foreach (var client in clients)
            {
                context.Clients.Add(client.ToEntity());
            }
        }

        private static void InitializeApiScopes(ConfigurationDbContext context)
        {
            if (!context.ApiScopes.Any())
            {
                foreach (var apiScope in Config.ApiScopes)
                {
                    context.ApiScopes.Add(apiScope.ToEntity());
                }
                context.SaveChanges();
            }
        }
    }
}