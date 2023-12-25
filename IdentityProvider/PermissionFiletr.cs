using IdentityProvider.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;

namespace IdentityProvider
{

    public class Permission : IAuthorizationFilter
    {

        private readonly AppDbContext _dbContext;

        public Permission(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }


        public void OnAuthorization(AuthorizationFilterContext context)
        {

            var endpoint = context.HttpContext.GetEndpoint();
            if (endpoint?.Metadata?.GetMetadata<IAllowAnonymous>() != null)
            {
                return;
            }

            var isSystemAdmin = bool.Parse(context.HttpContext.GetClaim("Admin"));
            if (isSystemAdmin)
            {
                return;
            }

            var controller = context.ActionDescriptor.RouteValues["controller"].ToString();
            var action = context.ActionDescriptor.RouteValues["action"].ToString();
            var userRoles = context.HttpContext.GetClaims("role");
            var rolesPermission = _dbContext.RoleActions.Include(x => x.Action).ThenInclude(x => x.Controller).Where(x => userRoles.Contains(x.RoleId.ToString())).ToList();
            if (!rolesPermission.Any(x => x.Action.Name == action && x.Action.Controller.Name == controller))

                throw new UnauthorizedAccessException();
            return;
        }


    }


}
