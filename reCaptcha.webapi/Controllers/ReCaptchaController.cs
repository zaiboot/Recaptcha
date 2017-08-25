namespace reCaptcha.webapi.Controllers
{
    using System;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;
    using System.Web.Http;
    using Models;
    using Wrappers;

    public class ReCaptchaController : ApiController
    {
        private readonly IRecaptchaManager _recaptchaManager = new RecaptchaManagerGoogle( new SurealHttpClient(new HttpClient()));
        [HttpPost]
        // POST api/values 
        public async Task<HttpResponseMessage> PostAsync()
        {
            var contentAsString = await Request.Content.ReadAsStringAsync();
            var response = new HttpResponseMessage(HttpStatusCode.BadRequest)
            {
                Content = new StringContent("Recaptcha response is invalid")
            };
            var parameters = contentAsString.Split('&');
            var gRecaptchaResponse = parameters.FirstOrDefault(p => p.StartsWith("g-recaptcha-response="));

            gRecaptchaResponse = gRecaptchaResponse?.Replace("g-recaptcha-response=", string.Empty);

            var googleRechaptchaRequest = new RechaptchaRequest
            {
                Response = gRecaptchaResponse,
                RemoteIp = Request.GetOwinContext().Request.RemoteIpAddress
            };
            

            var isCorrect = await _recaptchaManager.VerifyAsync(googleRechaptchaRequest);

            if (isCorrect)
            {
                response.StatusCode = HttpStatusCode.OK;
                response.Content = null;
                Console.WriteLine("Response = {0} is correct" , googleRechaptchaRequest.Response);    
            }
            else
            {
                Console.WriteLine("Response = {0} is correct", googleRechaptchaRequest.Response);
            }

            return response;



        }

    }
}