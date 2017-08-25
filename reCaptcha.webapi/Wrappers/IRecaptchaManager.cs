namespace reCaptcha.webapi.Wrappers
{
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Models;
    using Newtonsoft.Json;

    public interface IRecaptchaManager
    {
        Task<bool> VerifyAsync(RechaptchaRequest request);
    }

    public class RecaptchaManagerGoogle : IRecaptchaManager
    {
        private readonly IHttpClient _httpClient;
        private string _secretApi = "6LcO5C0UAAAAAOTKQTVM7GnJi0jnmVvsxycyrslX";
        private const string SITE_VERIFY_URL = "https://www.google.com/recaptcha/api/siteverify";

        public RecaptchaManagerGoogle(IHttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<bool> VerifyAsync(RechaptchaRequest request)
        {
            using (_httpClient)
            {

                var parameters = new[]
                {
                    new KeyValuePair<string, string>("secret", _secretApi),
                    new KeyValuePair<string, string>("response", request.Response),
                    new KeyValuePair<string, string>("remoteip", request.RemoteIp),
                };

                var content = new FormUrlEncodedContent(parameters);
                var result = await _httpClient.PostAsync(SITE_VERIFY_URL, content);
                var resultContent = await result.Content.ReadAsStringAsync();
              
                var googleResponse = JsonConvert.DeserializeObject<GoogleRecaptchaReponse>(resultContent);
                if (!googleResponse.Success)
                {
                    Console.WriteLine("Errors: {0}", String.Join(@"\n", googleResponse.ErrorCodes));
                }
                return googleResponse.Success;
            }
        }
    }

    public class GoogleRecaptchaReponse
    {
        public bool Success { get; set; }
        [JsonProperty(propertyName: "error-codes")]
        public string[] ErrorCodes { get; set; }
    }
}
