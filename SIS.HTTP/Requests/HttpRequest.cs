namespace SIS.HTTP.Requests
{
    using Enums;
    using Common;
    using Headers;
    using Cookies;
    using Contracts;
    using Extensions;
    using Exceptions;
    using Headers.Contracts;
    using Cookies.Contracts;
    using Sessions.Contracts;

    using System;
    using System.Linq;
    using System.Collections.Generic;

    public class HttpRequest : IHttpRequest
    {
        public HttpRequest(string requestString)
        {
            CoreValidator.ThrowIfNullOrEmpty(requestString, nameof(requestString));

            this.FormData = new Dictionary<string, object>();
            this.QueryData = new Dictionary<string, object>();
            this.Headers = new HttpHeaderCollection();
            this.Cookies = new HttpCookieCollection();

            this.ParseRequest(requestString);
        }

        public string Path { get; private set; }

        public string Url { get; private set; }

        public Dictionary<string, object> FormData { get; }

        public Dictionary<string, object> QueryData { get; }

        public IHttpHeaderCollection Headers { get; }

        public HttpRequestMethod RequestMethod { get; private set; }

        public IHttpCookieCollection Cookies { get; }

        public IHttpSession Session { get; set; }

        private void ParseRequest(string requestString)
        {
            var splitRequestContent = requestString.Split(new[] { GlobalConstants.HttpNewLine }, StringSplitOptions.None);
            var requestLine = splitRequestContent[0].Trim().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            if (!this.IsValidRequestLine(requestLine))
            {
                throw new BadRequestException();
            }

            this.ParseRequestMethod(requestLine);
            this.ParseRequestUrl(requestLine);
            this.ParseRequestPath();

            this.ParseRequestHeaders(this.ParsePlainRequestHeaders(splitRequestContent).ToArray());
            this.ParseCookies();

            this.ParseRequestParameters(splitRequestContent[splitRequestContent.Length - 1]);
        }

        private bool IsValidRequestLine(string[] requestLine)
        {
            if (requestLine.Length != 3 || requestLine[2] != GlobalConstants.HttpOneProtocolFragment)
            {
                return false;
            }

            return true;
        }

        private bool IsValidRequestQueryString(string queryString, string[] queryParameters)
        {
            if (string.IsNullOrEmpty(queryString) || queryParameters.Length < 1)
            {
                return false;
            }

            return true;
        }

        private IEnumerable<string> ParsePlainRequestHeaders(string[] requestLines)
        {
            for (int i = 1; i < requestLines.Length - 1; i++)
            {
                if (!string.IsNullOrEmpty(requestLines[i]))
                {
                    yield return requestLines[i];
                }
            }
        }

        private void ParseRequestMethod(string[] requestLine)
        {
            HttpRequestMethod method;
            var parseResult = HttpRequestMethod.TryParse(requestLine[0], true, out method);

            if (!parseResult)
            {
                throw new BadRequestException(string.Format(GlobalConstants.UnsupportedHttpMethodExceptionMessage, requestLine[0]));
            }

            this.RequestMethod = method;
        }

        private void ParseRequestUrl(string[] requestLine)
        {
            this.Url = requestLine[1];
        }

        private void ParseRequestPath()
        {
            this.Path = this.Url.Split('?')[0];
        }

        private void ParseRequestHeaders(string[] requestContent)
        {
            requestContent
                .Select(x => x.Split(new[] { ": " }, StringSplitOptions.RemoveEmptyEntries))
                .ToList()
                .ForEach(x => this.Headers.AddHeader(new HttpHeader(x[0], x[1])));
        }

        private void ParseCookies()
        {
            if (this.Headers.ContainsHeader(HttpHeader.Cookie))
            {
                var value = this.Headers.GetHeader(HttpHeader.Cookie).Value;
                var unparsedCookies = value.Split(new[] { "; " }, StringSplitOptions.RemoveEmptyEntries);

                foreach (var unparsedCookie in unparsedCookies)
                {
                    var cookieKvp = unparsedCookie.Split(new[] { '=' }, 2);

                    var httpCookie = new HttpCookie(cookieKvp[0], cookieKvp[1], false);

                    this.Cookies.AddCookie(httpCookie);
                }
            }
        }

        private void ParseQueryParameters()
        {
            if (this.Url.Contains('?'))
            {
                this.Url.Split('?', '#')[1]
                .Split('&')
                .Select(queryParam => queryParam.Split('='))
                .ToList()
                .ForEach(queryKvp => this.QueryData.Add(queryKvp[0], queryKvp[1]));
            }
        }

        private void ParseFormDataParameters(string formData)
        {
            //TODO: Parse Multiple Parameters By Name
            if (formData.Length > 0)
            {
                formData.Split('&')
                .Select(queryParam => queryParam.Split('='))
                .ToList()
                .ForEach(queryKvp => this.FormData.Add(queryKvp[0], queryKvp[1]));
            }
        }

        private void ParseRequestParameters(string formData)
        {
            this.ParseQueryParameters();
            this.ParseFormDataParameters(formData);
        }
    }
}