using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Yar.Data;

namespace Yar.BLL
{
    public interface ITranslationService
    {
        Task<TranslationResponse> GetTranslation(Language language, string phrase);
    }

    public sealed class TranslationFactory
    {
        public static ITranslationService GetService(Language language)
        {
            if (language == null)
            {
                return new NoTranslationService();
            }

            return GetService(language.GetOption(LanguageOptions.TranslationMethod, ""));
        }

        public static ITranslationService GetService(string method)
        {
            if (string.IsNullOrWhiteSpace(method))
            {
                return new NoTranslationService();
            }

            if (method.Equals("None", StringComparison.InvariantCultureIgnoreCase))
            {
                return new NoTranslationService();
            }

            if (method.Equals("Bing", StringComparison.InvariantCultureIgnoreCase))
            {
                return new BingTranslationService();
            }

            if (method.Equals("Google", StringComparison.InvariantCultureIgnoreCase))
            {
                return new GoogleTranslationService();
            }

            if (method.Equals("Dummy", StringComparison.InvariantCultureIgnoreCase))
            {
                return new DummyTranslationService();
            }

            return new NoTranslationService();
        }
    }

    public class TranslationResponse
    {
        public bool Success { get; }
        public string Translation { get; }

        public TranslationResponse(bool success, string translation)
        {
            Success = success;
            Translation = translation;
        }
    }

    public class BingTranslationService : ITranslationService
    {
        public async Task<TranslationResponse> GetTranslation(Language language, string phrase)
        {
            string uri = language.GetOption(LanguageOptions.BingTranslationUrl, "");

            if (string.IsNullOrWhiteSpace(uri))
            {
                return new TranslationResponse(false, "BingTranslationUrl not found");
            }

            try
            {
                var body = new object[] { new { Text = phrase } };
                var requestBody = JsonConvert.SerializeObject(body);

                using (var client = new HttpClient())
                {
                    using (var request = new HttpRequestMessage())
                    {
                        request.Method = HttpMethod.Post;
                        request.RequestUri = new Uri(uri);
                        request.Content = new StringContent(requestBody, Encoding.UTF8, "application/json");
                        request.Headers.Add("Ocp-Apim-Subscription-Key", language.GetOption(LanguageOptions.BingTranslationKey, ""));

                        var response = await client.SendAsync(request);
                        var responseBody = await response.Content.ReadAsStringAsync();
                        dynamic responseObject = JsonConvert.DeserializeObject(responseBody);
                        var translation = responseObject[0].translations[0].text.ToString();

                        return new TranslationResponse(true, translation);
                    }
                }
            }
            catch (Exception e)
            {
                return new TranslationResponse(false, e.Message);
            }
        }
    }

    public class GoogleTranslationService : ITranslationService
    {
        public async Task<TranslationResponse> GetTranslation(Language language, string phrase)
        {
            string uri = $"https://translation.googleapis.com/language/translate/v2?key={language.GetOption<string>(LanguageOptions.GoogleTranslationKey)}";

            try
            {
                var body = new
                {
                    q = phrase,
                    source = language.GetOption<string>(LanguageOptions.GoogleTranslationSource),
                    target = language.GetOption<string>(LanguageOptions.GoogleTranslationTarget),
                    format = "text"
                };

                var requestBody = JsonConvert.SerializeObject(body);

                using (var client = new HttpClient())
                {
                    using (var request = new HttpRequestMessage())
                    {
                        request.Method = HttpMethod.Post;
                        request.RequestUri = new Uri(uri);
                        request.Content = new StringContent(requestBody, Encoding.UTF8, "application/json");

                        var response = await client.SendAsync(request);
                        var responseBody = await response.Content.ReadAsStringAsync();
                        dynamic responseObject = JsonConvert.DeserializeObject(responseBody);
                        var translation = responseObject.data.translations[0].translatedText.ToString();

                        return new TranslationResponse(true, translation);
                    }
                }
            }
            catch (Exception e)
            {
                return new TranslationResponse(false, e.Message);
            }
        }
    }

    public class DummyTranslationService : ITranslationService
    {
        public async Task<TranslationResponse> GetTranslation(Language language, string phrase)
        {
            return new TranslationResponse(true, $"t-{phrase}");
        }
    }

    public class NoTranslationService : ITranslationService
    {
        public async Task<TranslationResponse> GetTranslation(Language language, string phrase)
        {
            return new TranslationResponse(true, $"");
        }
    }
}
