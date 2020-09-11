using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Yar.Api.Controllers
{
    [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    [Route("users")]
    public class UserController : BaseController
    {
        public IActionResult Index()
        {
            return View(HttpContext.User.Identity.IsAuthenticated);
        }
    }
}
