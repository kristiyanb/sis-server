namespace SIS.HTTP.Cookies
{
    using System.Text;
    using System.Collections;
    using System.Collections.Generic;
    using Common;
    using Contracts;

    public class HttpCookieCollection : IHttpCookieCollection
    {
        private readonly Dictionary<string, HttpCookie> cookies;

        public HttpCookieCollection()
        {
            this.cookies = new Dictionary<string, HttpCookie>();
        }

        public void AddCookie(HttpCookie cookie)
        {
            CoreValidator.ThrowIfNull(cookie, nameof(cookie));

            if (!this.cookies.ContainsKey(cookie.Key))
            {
                this.cookies[cookie.Key] = cookie;
            }
        }

        public bool ContainsCookie(string key)
        {
            CoreValidator.ThrowIfNullOrEmpty(key, nameof(key));

            return this.cookies.ContainsKey(key);
        }

        public HttpCookie GetCookie(string key)
        {
            CoreValidator.ThrowIfNullOrEmpty(key, nameof(key));

            return this.cookies[key];
        }

        public bool HasCookies() => this.cookies.Count != 0;

        public IEnumerator<HttpCookie> GetEnumerator() => this.cookies.Values.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();

        public override string ToString()
        {
            var output = new StringBuilder();

            foreach (var cookie in this.cookies.Values)
            {
                output.Append($"Set-Cookie: {cookie}").Append(GlobalConstants.HttpNewLine);
            }

            return output.ToString();
        }
    }
}
