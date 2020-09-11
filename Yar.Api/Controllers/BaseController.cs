using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Security.Claims;

namespace Yar.Api.Controllers
{
    public class BaseController : Controller
    {
        protected int UserId
        {
            get
            {
                if (!User.Identity.IsAuthenticated)
                {
                    return 0;
                }

                string idValue = ((ClaimsIdentity)User.Identity).Claims.SingleOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier)?.Value ?? "0";

                if (int.TryParse(idValue, out int id))
                {
                    return id;
                }

                return 0;
            }
        }
    }
}
