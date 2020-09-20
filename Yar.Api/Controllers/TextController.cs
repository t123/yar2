using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Security.Cryptography.X509Certificates;
using Yar.Api.Models;
using Yar.BLL;
using Yar.BLL.Dto;

namespace Yar.Api.Controllers
{
    [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    [Route("texts")]
    public class TextController : BaseController
    {
        private readonly IUnitOfWork _uow;

        public TextController(IUnitOfWork uow)
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
                .Select(x => x.Name);

            return View(languages);
        }

        [HttpPost]
        [Route("search")]
        public IActionResult Search([FromBody] TextSearchDtoModel model)
        {
            var texts = _uow
                .TextService
                .Get(UserId);

            if (!string.IsNullOrWhiteSpace(model?.Filter))
            {
                var terms = model.Filter.Split(' ', StringSplitOptions.RemoveEmptyEntries);

                foreach (var term in terms)
                {
                    if (term.Contains(":"))
                    {
                        var split = term.Split(':', 2, StringSplitOptions.RemoveEmptyEntries);

                        if (split.Length == 2)
                        {
                            switch (split[0].ToUpper())
                            {
                                case "TITLE":
                                    texts = texts.Where(x => x.Title.Contains(split[1], StringComparison.InvariantCultureIgnoreCase));
                                    break;

                                case "LANGUAGE":
                                    texts = texts.Where(x => x.Language?.Name.Contains(split[1], StringComparison.InvariantCultureIgnoreCase) ?? false);
                                    break;

                                case "COLLECTION":
                                    texts = texts.Where(x => x.Collection.Contains(split[1], StringComparison.InvariantCultureIgnoreCase));
                                    break;

                                case "PARALLEL":
                                    if (split[1].ToUpper() == "YES")
                                    {
                                        texts = texts.Where(x => x.IsParallel);
                                    }
                                    else if (split[1].ToUpper() == "NO")
                                    {
                                        texts = texts.Where(x => !x.IsParallel);
                                    }
                                    break;

                                case "READ":
                                    if (split[1].ToUpper() == "YES")
                                    {
                                        texts = texts.Where(x => x.LastRead != null);
                                    }
                                    else if (split[1].ToUpper() == "NO")
                                    {
                                        texts = texts.Where(x => x.LastRead == null);
                                    }
                                    else if (int.TryParse(split[1], out int days))
                                    {
                                        texts = texts.Where(x => x.LastRead.HasValue && (DateTime.Now - x.LastRead.Value).TotalDays < days);
                                    }
                                    break;
                            }

                            continue;
                        }
                    }

                    texts = texts.Where(x =>
                        x.Title.Contains(term, StringComparison.InvariantCultureIgnoreCase) ||
                        (x.Language?.Name.Contains(term, StringComparison.InvariantCultureIgnoreCase) ?? false) ||
                        x.Collection.Contains(term, StringComparison.InvariantCultureIgnoreCase)
                    );
                }
            }

            texts = texts
                .OrderBy(x => x.Language.Name)
                .ThenBy(x => x.Collection)
                .ThenBy(x => x.CollectionNo)
                .ThenBy(x => x.Title);

            return Ok(texts.Select(x => TextSearchModel.From(x, "/texts/edit", "/read")));
        }

        [HttpGet]
        [Route("create")]
        public IActionResult Create()
        {
            var collections = _uow.TextService.GetCollections(UserId, 0);
            var languages = _uow.LanguageService.Get(UserId);

            var model = TextViewModel.From(null, collections, languages);

            return View("Edit", model);
        }

        [HttpGet]
        [Route("edit/{textId}")]
        public IActionResult Edit(int textId)
        {
            var text = _uow.TextService.Get(UserId, textId);
            var collections = _uow.TextService.GetCollections(UserId, 0);
            var languages = _uow.LanguageService.Get(UserId);

            var model = TextViewModel.From(text, collections, languages);

            return View(model);
        }

        [HttpPost]
        [Route("save")]
        public IActionResult Save(PostTextDto text)
        {
            var result = _uow.TextService.Save(UserId, text);

            switch (text.Action)
            {
                case "submit-read": return RedirectToAction("Index", "Read", new { textId = result.Id, asParallel = result.IsParallel });
                default: return RedirectToAction("Index");
            }
        }

        [HttpPost]
        [Route("delete/{textid}")]
        public IActionResult Delete(int textId)
        {
            _uow.TextService.Delete(UserId, textId);

            return Ok("Index");
        }
    }
}
