namespace SIS.WebServer.Results
{
    using HTTP.Responses;
    using HTTP.Enums;
    using HTTP.Headers;
    using System.Text;

    public class TextResult : HttpResponse
    {
        private const string ContentType = "text/plain; charset=utf-8";

        public TextResult(string content, HttpResponseStatusCode responseStatusCode, string contentType = ContentType)
            :base (responseStatusCode)
        {
            this.Headers.AddHeader(new HttpHeader("Content-Type", contentType));
            this.Content = Encoding.UTF8.GetBytes(content);
        }

        public TextResult(byte[] content, HttpResponseStatusCode responseStatusCode, string contentType = ContentType)
            :base (responseStatusCode)
        {
            this.Headers.AddHeader(new HttpHeader("Content-Type", contentType));
            this.Content = content;
        }
    }
}
