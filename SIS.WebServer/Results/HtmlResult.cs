namespace SIS.WebServer.Results
{
    using HTTP.Responses;
    using HTTP.Enums;
    using HTTP.Headers;
    using System.Text;

    public class HtmlResult : HttpResponse
    {
        public HtmlResult(string content, HttpResponseStatusCode responseStatusCode)
            : base(responseStatusCode)
        {
            base.Headers.AddHeader(new HttpHeader("Content-Type", "text/html; charset=utf-8"));
            base.Content = Encoding.UTF8.GetBytes(content);
        }
    }
}
