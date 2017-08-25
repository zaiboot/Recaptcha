namespace reCaptcha.webapi.Models
{
    public class RechaptchaRequest
    {
        public string Secret { get; set; }
        public string Response { get; set; }
        public string RemoteIp { get; set; }
    }
}