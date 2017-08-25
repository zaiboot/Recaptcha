namespace reCaptcha.webapi.Wrappers
{
    using System;
    using System.Net.Http;
    using System.Threading.Tasks;

    public interface IHttpClient : IDisposable
    {
        Task<HttpResponseMessage> PostAsync(string url, FormUrlEncodedContent content);

    }

    public class SurealHttpClient : IHttpClient
    {
        private readonly HttpClient _httpClient;


        public SurealHttpClient(HttpClient httpClient )
        {
            _httpClient = httpClient;
        }
        public async Task<HttpResponseMessage> PostAsync(string url, FormUrlEncodedContent content)
        {
            using (var httpClient = new HttpClient())
            {

                var result = await httpClient.PostAsync(url, content);
                Console.WriteLine("result = {0}", await result.Content.ReadAsStringAsync());
                return result;
            }

        }

        public void Dispose()
        {
            _httpClient?.Dispose();
        }
    }
}
