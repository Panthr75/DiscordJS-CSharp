using System;
using System.IO;
using System.Net;

namespace JavaScript.Web
{
    /// <summary>
    /// The response to a request
    /// </summary>
    public class Response : Body
    {
        /// <summary>
        /// <list type="bullet">
        /// <item>"basic"</item>
        /// <item>"cors"</item>
        /// <item>"default"</item>
        /// <item>"error"</item>
        /// <item>"opaque"</item>
        /// <item>"opaqueredirect"</item>
        /// </list>
        /// </summary>
        public string Type { get; }

        public string URL { get; }
        public bool Redirected { get; }
        public ushort Status { get; }
        public bool Ok { get; }
        public string StatusText { get; }
        public Headers Headers { get; }

        private readonly Stream body;

        internal Response(WebResponse response)
        {
            //
        }

        public Response(Stream body = null, ResponseInit init = null)
        {
            if (init == null) init = new ResponseInit();
            if (init.Status < 200 || init.Status > 599) throw new ArgumentOutOfRangeException("init.Status", "Status must be within range 200 (inclusive) to 600 (exclusive)");
            Status = init.Status;
            StatusText = init.StatusText;
            if (body != null)
            {
                this.body = body;
            }
        }

        public override Stream GetBody() => body;

        public Response Clone()
        {
            //
        }

        public static Response Error()
        {
            //
        }

        public static Response Redirect(string url, ushort status = 302)
        {
            //
        }
    }
}