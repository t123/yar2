﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Yar.Data;
using Yar.BLL;
using Yar.Api.Models;
using Yar.BLL.Dto;
using System;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace Yar.Api.Controllers
{
    [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    [Route("read")]
    public class ReadController : BaseController
    {
        private readonly IUnitOfWork _uow;

        public ReadController(IUnitOfWork uow)
        {
            _uow = uow;
        }

        [HttpGet]
        [Route("{textId}")]
        public IActionResult Index(int textId, bool asParallel)
        {
            var text = _uow.TextService.Get(UserId, textId);
            var other = _uow.TextService.GetPreviousAndNext(text);

            return View(new ReadIndexModel(textId, asParallel, text.Language.Id, text.IsParallel, other.Previous?.Id, other.Next?.Id, text.FullTitle));
        }

        [HttpGet]
        [Route("read/{textId}")]
        public IActionResult Read(int textId, bool asParallel)
        {
            var text = _uow.TextService.Get(UserId, textId);
            _uow.TextService.SetLastRead(UserId, text.Id);
            var result = _uow.ParserService.Parse(text, _uow.WordService.Get(UserId, text.Language.Id).ToArray(), asParallel);
            _uow.TextService.Parse(UserId, textId, result);

            var model = TextReadModel.From(text, result);

            return Ok(model);
        }

        [HttpPost]
        [Route("parse/{textId}")]
        public IActionResult Parse(int textId)
        {
            _uow.TextService.Parse(UserId, textId);
            return Ok();
        }

        [HttpPost]
        [Route("undo")]
        public IActionResult Undo([FromBody] UndoRequestModel model)
        {
            if (!Guid.TryParse(model.Uuid, out Guid uuid))
            {
                return Ok(new UndoResponseModel { Success = false });
            }

            var word = _uow.WordService.Get(UserId, uuid);

            if (word == null)
            {
                return Ok(new UndoResponseModel { Success = false });
            }

            _uow.WordService.Delete(UserId, word.Id);

            return Ok(new UndoResponseModel
            {
                Phrase = word.Phrase,
                PhraseLower = word.PhraseLower,
                Success = true
            });
        }

        [HttpPost]
        [Route("save")]
        public IActionResult Save([FromBody] SavePhraseRequestModel model)
        {
            var word = _uow.WordService.Get(UserId, model.LanguageId, model.Phrase);

            if (word == null)
            {
                var createWord = new WordCreateDto
                {
                    LanguageId = model.LanguageId,
                    Notes = model.Notes,
                    Phrase = model.Phrase,
                    PhraseBase = model.PhraseBase,
                    Sentence = model.Sentence,
                    Translation = model.Translation,
                    State = Enum.Parse<WordState>(model.State, true),
                    TextId = model.TextId
                };

                word = _uow.WordService.Save(UserId, createWord);
            }
            else
            {
                var updateWord = new WordUpdateDto
                {
                    Id = word.Id,
                    Sentence = model.Sentence,
                    Notes = model.Notes,
                    Phrase = model.Phrase,
                    PhraseBase = model.PhraseBase,
                    Translation = model.Translation,
                    State = Enum.Parse<WordState>(model.State, true),
                    LanguageId = model.LanguageId,
                    TextId = model.TextId
                };

                word = _uow.WordService.Save(UserId, updateWord);
            }

            return Ok(CreateWordResponseModel(word));
        }

        [HttpPost]
        [Route("updatestate")]
        public IActionResult UpdateState([FromBody] ChangePhraseStateRequestModel model)
        {
            var word = _uow.WordService.Get(UserId, model.LanguageId, model.Phrase);

            if (word == null)
            {
                return NotFound();
            }

            var updateWord = new WordStateUpdateDto
            {
                Id = word.Id
            };

            var updated = _uow.WordService.CycleState(UserId, updateWord);

            return Ok(CreateWordResponseModel(updated));
        }

        [HttpPost]
        [Route("retranslate")]
        public IActionResult Retranslate([FromBody] TranslationRequestModel model)
        {
            var word = _uow.WordService.Get(UserId, model.LanguageId, model.Phrase);
            var language = _uow.LanguageService.Get(UserId, model.LanguageId);
            var translationService = TranslationFactory.GetService(model.Method);
            var translation = translationService.GetTranslation(language, model.Phrase);

            var vm = new RetranslateResponseModel
            {
                Phrase = word.Phrase,
                PhraseLower = word.PhraseLower,
                Translation = translation.Result.Translation
            };

            return Ok(vm);
        }

        [HttpPost]
        [Route("translate")]
        public IActionResult Translate([FromBody] TranslationRequestModel model)
        {
            var word = _uow.WordService.Get(UserId, model.LanguageId, model.Phrase);
            var language = _uow.LanguageService.Get(UserId, model.LanguageId);
            var canUndo = false;

            if (word == null)
            {
                var translationService = TranslationFactory.GetService(language);
                var translation = translationService.GetTranslation(language, model.Phrase);

                if (translation.Result.Success)
                {
                    var createWord = new WordTranslateCreateDto
                    {
                        LanguageId = model.LanguageId,
                        Phrase = model.Phrase,
                        Sentence = model.Sentence,
                        Translation = translation.Result.Translation,
                        TextId = model.TextId
                    };

                    word = _uow.WordService.Save(UserId, createWord);
                    canUndo = true;
                }
                else
                {
                    throw new Exception(translation.Result.Translation);
                }
            }
            else
            {
                var updateWord = new WordTranslateUpdateDto
                {
                    Id = word.Id,
                    LanguageId = model.LanguageId,
                    Sentence = model.Sentence,
                    TextId = model.TextId
                };

                word = _uow.WordService.Save(UserId, updateWord);
            }

            return Ok(CreateWordResponseModel(word, canUndo));
        }

        private ReadWordResponseModel CreateWordResponseModel(Word word, bool canUndo = false)
        {
            var model = new ReadWordResponseModel
            {
                Phrase = word.Phrase,
                PhraseBase = word.PhraseBase,
                PhraseLower = word.PhraseLower,
                State = word.State.ToString().ToLower(),
                Notes = word.Notes,
                Translation = word.Translation,
                CanUndo = canUndo,
                Uuid = word.Uuid.ToString()
            };

            return model;
        }
    }
}