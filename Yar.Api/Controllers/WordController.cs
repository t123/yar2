using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Yar.Api.Models;
using Yar.BLL;

namespace Yar.Api.Controllers
{
    [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    [Route("words")]
    public class WordController : BaseController
    {
        private readonly IUnitOfWork _uow;

        public WordController(IUnitOfWork uow)
        {
            _uow = uow;
        }

        [HttpGet]
        [Route("")]
        public IActionResult Index()
        {
            var words = _uow
                .WordService
                .Get(UserId)
                .OrderBy(x => x.Language.Name)
                .ThenBy(x => x.PhraseLower)
                .Select(x => WordIndexModel.From(x));

            return View(words);
        }

        [HttpDelete]
        [Route("delete/{wordId}")]
        public IActionResult Delete(int wordId)
        {
            _uow.WordService.Delete(UserId, wordId);

            return View();
        }
    }
}
