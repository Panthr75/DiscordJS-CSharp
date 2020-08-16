using System.Net;

namespace JavaScript.Web
{
    public static class FetchAPI
    {
        public static IPromise<Response> Fetch(string url) => Fetch(url, new RequestInit());

        public static IPromise<Response> Fetch(string url, RequestInit options)
        {
            var req = new Request(url, options);
            return new Promise<HttpWebResponse>(async (resolve, reject) =>
            {
                try
                {
                    resolve((HttpWebResponse)await req.request.GetResponseAsync());
                }
                catch(WebException exception)
                {
                    reject(exception);
                }
            }).Then((res) =>
            {
                var headerInit = new HeadersInit();
                string[] keys = res.Headers.AllKeys;
                for (int index = 0, length = keys.Length; index < length; index++)
                {
                    string key = keys[index];
                    headerInit[key] = res.Headers.Get(key);
                }
                return new Response(res.GetResponseStream(), new ResponseInit()
                {
                    Headers = headerInit,
                    Status = (ushort)res.StatusCode,
                    StatusText = res.StatusDescription
                });
            });
        }
    }
}