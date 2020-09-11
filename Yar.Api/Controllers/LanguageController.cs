using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Yar.Api.Models;
using Yar.BLL;
using Yar.BLL.Dto;

namespace Yar.Api.Controllers
{
    [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    [Route("languages")]
    public class LanguageController : BaseController
    {
        private readonly IUnitOfWork _uow;

        public LanguageController(IUnitOfWork uow)
        {
            _uow = uow;
        }

        [HttpGet]
        [Route("")]
        public IActionResult Index()
        {
            var languages = _uow
                .LanguageService
                .Get(UserId)
                .OrderBy(x => x.Name)
                .Select(x => LanguageIndexModel.From(x));

            return View(languages);
        }

        [HttpGet]
        [Route("create")]
        public IActionResult Create()
        {
            return View("Edit");
        }

        [HttpGet]
        [Route("edit/{languageId}")]
        public IActionResult Edit(int languageId)
        {
            var language = _uow.LanguageService.Get(UserId, languageId);
            var model = LanguageViewModel.From(language);

            return View(model);
        }

        [HttpPost]
        [Route("save")]
        public IActionResult Save(PostLanguageDto language)
        {
            _uow.LanguageService.Save(UserId, language);

            return RedirectToAction("Index");
        }

        [HttpPost]
        [Route("delete/{languageId}")]
        public IActionResult Delete(int languageId)
        {
            _uow.LanguageService.Delete(UserId, languageId);

            return RedirectToAction("Index");
        }
    }
}
