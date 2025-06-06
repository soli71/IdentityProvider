using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace WebApp.Pages.Account
{
    public class LogoutModel : PageModel
    {
        public async Task OnPost()
        {
            await HttpContext.SignOutAsync("Cookies"); // Sign out of the cookie authentication
            await HttpContext.SignOutAsync("OpenIdConnect"); // Sign out of OpenID Connect
        }
    }
}