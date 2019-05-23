namespace SIS.HTTP.Responses
{
    using Enums;
    using Common;
    using Headers;
    using Cookies;
    using Contracts;
    using Extensions;
    using Headers.Contracts;
    using Cookies.Contracts;
    using System.Text;

    public class HttpResponse : IHttpResponse
    {
        public HttpResponse()
        {
            this.Headers = new HttpHeaderCollection();
            this.Cookies = new HttpCookieCollection();
            this.Content = new byte[0];
        }

        public HttpResponse(HttpResponseStatusCode statusCode)
            : this()
        {
            CoreValidator.ThrowIfNull(statusCode, nameof(statusCode));

            this.StatusCode = statusCode;
        }

        public HttpResponseStatusCode StatusCode { get; set; }

        public IHttpHeaderCollection Headers { get; }

        public IHttpCookieCollection Cookies { get; }

        public byte[] Content { get; set; }

        public void AddHeader(HttpHeader header)
        {
            this.Headers.AddHeader(header);
        }

        public byte[] GetBytes()
        {
            var httpReponseBytesWithoutBody =  Encoding.UTF8.GetBytes(this.ToString());
            var httpReponseBytesWithBody = new byte[httpReponseBytesWithoutBody.Length + this.Content.Length];

            for (int i = 0; i < httpReponseBytesWithoutBody.Length; i++)
            {
                httpReponseBytesWithBody[i] = httpReponseBytesWithoutBody[i];
            }

            for (int i = 0; i < httpReponseBytesWithBody.Length - httpReponseBytesWithoutBody.Length; i++)
            {
                httpReponseBytesWithBody[i + httpReponseBytesWithoutBody.Length] = this.Content[i];
            }

            return httpReponseBytesWithBody;
        }

        public void AddCookie(HttpCookie cookie) => this.Cookies.AddCookie(cookie);

        public override string ToString()
        {
            var output = new StringBuilder();

            output
                .Append($"{GlobalConstants.HttpOneProtocolFragment} {this.StatusCode.GetStatusLine()}")
                .Append($"{GlobalConstants.HttpNewLine}")
                .Append($"{this.Headers}")
                .Append($"{GlobalConstants.HttpNewLine}");

            if (this.Cookies.HasCookies())
            {
                output.Append($"Set-Cookie: {this.Cookies.ToString()}").Append(GlobalConstants.HttpNewLine);
            }

            output.Append($"{GlobalConstants.HttpNewLine}");

            return output.ToString(); 
        } 
    }
}
