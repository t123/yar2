using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Yar.Api.Models;
using Yar.BLL;
using Yar.Data;

namespace Yar.Api.Controllers
{
    [Route("account")]
    public class AccountController : Controller
    {
        private readonly IUnitOfWork _uow;

        public AccountController(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public IActionResult Index()
        {
            var users = _uow
                .UserService
                .Get()
                .OrderBy(x => x.Username.ToLowerInvariant())
                .Select(x => UserIndexModel.From(x));

            return View(users);
        }

        [HttpPost]
        [Route("sign-in")]
        public async Task<IActionResult> SignIn(string username)
        {
            var user = _uow.UserService.Login(username);

            if (user!=null)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, username),
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Role, "user")
                };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = true,
                    ExpiresUtc = DateTime.UtcNow.AddYears(10)
                };

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);

                return RedirectToAction("Index", "Text");
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        [Route("create-profile")]
        public async Task<IActionResult> CreateProfile(string username)
        {
            var user = new User()
            {
                Username = username
            };

            try
            {
                _uow.UserService.Save(user);
            }
            catch (Exception e)
            {
                throw;
            }

            return RedirectToAction("Index");
        }

        [Route("sign-out")]
        public async Task<IActionResult> SignOut()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("Index", "Account");
        }
    }
}
