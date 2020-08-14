using Newtonsoft.Json;
using System.IO;
using System.Net;
using System.Text;

namespace JavaScript.Web
{
    /// <summary>
    /// The response to a request
    /// </summary>
    public class Body
    {
        public Body()
        {

        }

        internal bool bodyUsed = false;

        /// <summary>
        /// Whether or not the body has been read
        /// </summary>
        public bool BodyUsed => bodyUsed;

        /// <summary>
        /// Gets the contents of this body
        /// </summary>
        public virtual Stream GetBody()
        {
            return null;
        }

        private IPromise<byte[]> GetBytes()
        {
            return new Promise<byte[]>(async (resolve, reject) =>
            {
                var b = GetBody();
                byte[] buffer = new byte[b.Length];
                await b.ReadAsync(buffer, 0, (int)b.Length);
                resolve(buffer);
            });
        }

        /// <summary>
        /// Returns a promise that resolves with an ArrayBuffer for the data
        /// </summary>
        /// <returns></returns>
        public IPromise<ArrayBuffer> ArrayBuffer()
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Returns a promise that resolves with a Blob for the data
        /// </summary>
        /// <returns></returns>
        public IPromise<Blob> Blob()
        {
            return GetBytes().Then((buffer) =>
            {
                return new Blob(buffer, "Body");
            });
        }

        /// <summary>
        /// Returns a promise that resolves with FormData for the data
        /// </summary>
        /// <returns></returns>
        public IPromise<FormData> FormData()
        {
            //
        }

        /// <summary>
        /// Returns a promise that resolves to T, converting the data to JSON
        /// </summary>
        /// <typeparam name="T">The type to convert the JSON to</typeparam>
        /// <returns></returns>
        public IPromise<T> JSON<T>()
        {
            return GetBytes().Then((buffer) =>
            {
                string text = Encoding.UTF8.GetString(buffer);
                return JsonConvert.DeserializeObject<T>(text);
            });
        }

        /// <summary>
        /// Returns a promise that resolves to the text of this body.
        /// <br/>
        /// <br/>
        /// <b>The response is <i>always</i> decoded using UTF-8.</b>
        /// </summary>
        /// <returns></returns>
        public IPromise<string> Text()
        {
            return GetBytes().Then((buffer) =>
            {
                return Encoding.UTF8.GetString(buffer);
            });
        }
    }
}