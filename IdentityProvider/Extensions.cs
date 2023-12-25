using IdentityProvider.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using System.Reflection;
using Microsoft.EntityFrameworkCore;

namespace IdentityProvider
{
    public static class Extensions
    {

        public static IServiceCollection AddSwagger(this IServiceCollection services) =>
          services.AddSwaggerGen(c =>
          {
              c.SwaggerDoc("v1", new OpenApiInfo { Title = "IdnetityProvider.Api", Version = "v1" });

              c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
              {
                  In = ParameterLocation.Header,
                  Description = "Please enter token",
                  Name = "Authorization",
                  Type = SecuritySchemeType.ApiKey,
                  BearerFormat = "JWT",
                  Scheme = "bearer"
              });
              c.AddSecurityRequirement(new OpenApiSecurityRequirement
                 {
                        {
                                new OpenApiSecurityScheme
                                    {
                                        Reference = new OpenApiReference
                                        {
                                            Type=ReferenceType.SecurityScheme,
                                            Id="Bearer"
                                        }
                                    },

                                new string[]{}
                        }
                 });
              //var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
              //c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
          });

        public static IServiceCollection AddPermissions(this IServiceCollection services)
        {
            var _dbcontext = services.BuildServiceProvider().GetService<AppDbContext>();
            _dbcontext.Database.EnsureCreated();
            _dbcontext.Database.Migrate();
            IEnumerable<Type> enumerable = from x in Assembly.Load("IdentityProvider").GetExportedTypes()
                                           where x.Name.Contains("Controller")
                                           select x;
            foreach (Type item in enumerable)
            {

                Controller controllerInformation = new Controller
                {
                    Name = item.Name.Replace("Controller", "")
                };
                if (string.IsNullOrEmpty(controllerInformation.Name))
                    continue;
                MethodInfo[] methods = item.GetMethods();
                controllerInformation.Actions = (from x in methods
                                                 where GetMethodCondition(x)
                                                 select new Models.Action
                                                 {
                                                     Name = x.Name
                                                 }).ToList();
                var controller = _dbcontext.Controllers.FirstOrDefault(x => x.Name == controllerInformation.Name);
                if (controller != null)
                {
                    var controllerActions = _dbcontext.Actions.Where(x => x.ControllerId == controller.Id).ToList();
                    var addActions = controllerInformation.Actions.Select(x => x.Name).Except(controllerActions.Select(x => x.Name)).ToList();
                    var removeActions = controllerActions.Select(x => x.Name).Except(controllerInformation.Actions.Select(x => x.Name)).ToList();
                    _dbcontext.Actions.AddRange(addActions.Select(x => new Models.Action
                    {
                        ControllerId = controller.Id,
                        Name = x,
                    }));
                    _dbcontext.Actions.RemoveRange(_dbcontext.Actions.Where(x => removeActions.Contains(x.Name)));
                }
                else
                    _dbcontext.Controllers.AddRange(controllerInformation);


            }
            _dbcontext.SaveChanges();
            return services;
        }


        public static Func<MethodInfo, bool> GetMethodCondition = (x) =>
                x.DeclaringType.Namespace.Contains("IdentityProvider")
            &&
                x.GetCustomAttributes(typeof(IgnorePermissionAccessAttribute), false).Length == 0
            &&
                x.GetCustomAttributes(typeof(IAllowAnonymous), false).Length == 0
            ;
    }

    public class IgnorePermissionAccessAttribute : Attribute, IIgnorePermissionAccess
    {
    }
    public interface IIgnorePermissionAccess
    {
    }
}
