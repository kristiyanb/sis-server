namespace SIS.HTTP.Cookies
{
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

            this.cookies.Add(cookie.Key, cookie);
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
        public IEnumerator<HttpCookie> GetEnumerator()
        {
            foreach (var cookie in this.cookies.Values)
            {
                yield return cookie;
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();

        public override string ToString()
        {
            //TODO: fix separator
            return string.Join(";", this.cookies.Values);
        }
    }
}
