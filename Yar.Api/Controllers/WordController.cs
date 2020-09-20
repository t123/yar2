using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Linq;
using System.Text;
using Yar.Api.Models;
using Yar.BLL;
using Yar.Data;

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

        [HttpGet]
        [Route("edit/{wordId}")]
        public IActionResult Edit(int wordId)
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        [Route("delete/{wordId}")]
        public IActionResult Delete(int wordId)
        {
            _uow.WordService.Delete(UserId, wordId);

            return RedirectToAction("Index");
        }

        [HttpGet]
        [Route("download/{languageName}")]
        public IActionResult Download(string languageName)
        {
            var words = _uow
                .WordService
                .Get(UserId)
                .Where(x => x.State == WordState.NotKnown && x.Language.Name.ToUpper() == languageName.ToUpper())
                .OrderBy(x => x.Language.Name)
                .ThenBy(x => x.PhraseLower);

            using (var memoryStream = new MemoryStream())
            {
                using (var writer = new StreamWriter(memoryStream, Encoding.UTF8))
                {
                    const string Tab = "\t";

                    foreach (var word in words)
                    {
                        var notes = word.Notes.Replace("\n", "<br/>");
                        writer.WriteLine($@"""{word.Uuid}""{Tab}""{word.Phrase}""{Tab}""{(string.IsNullOrWhiteSpace(word.PhraseBase) ? word.Phrase : word.PhraseBase)}""{Tab}""{word.Translation}""{Tab}""{word.Sentence}""{Tab}""{notes}""{Tab}""{word.Created}""{Tab}""{word.Updated}""");
                    }

                    writer.Flush();
                    memoryStream.Seek(0, SeekOrigin.Begin);

                    var bytes = memoryStream.ToArray();

                    return new FileContentResult(bytes, "text/plain")
                    {
                        FileDownloadName = $"{languageName}.csv"
                    };
                }
            }
        }
    }
}
