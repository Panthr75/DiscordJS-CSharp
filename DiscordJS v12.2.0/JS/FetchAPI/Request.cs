using System.Net;

namespace JavaScript.Web
{
    public class Request
    {
        internal HttpWebRequest request;

        public Request(string url, RequestInit init)
        {
            var req = (HttpWebRequest)WebRequest.Create(url);
            req.Headers = new WebHeaderCollection();
            if (init.Headers != null)
            {
                var h = init.Headers.values;
                foreach (string name in h.Keys)
                {
                    req.Headers.Add(name, h[name]);
                }
            }
            req.Method = init.Method.ToUpper();
            request = req;
        }
    }
}