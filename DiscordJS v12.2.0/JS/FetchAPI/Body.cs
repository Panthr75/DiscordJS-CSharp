using JavaScript.Web.Utils;
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

        internal Body(string contentType, Stream stream)
        {
            this.contentType = contentType;
            this.stream = stream;
        }

        private readonly string contentType;
        private readonly Stream stream;

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
            bodyUsed = true;
            return stream;
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
            return GetBytes().Then<ArrayBuffer>((buffer) =>
            {
                try
                {
                    ArrayBuffer b = new ArrayBuffer();
                    b.contents = buffer;
                    return b;
                }
                catch(System.Exception ex)
                {
                    throw new System.Exception("Could not convert to array buffer", ex);
                }
            });
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
            return new Promise<FormData>((resolve, reject) =>
            {
                GetBytes().Then((data) =>
                {
                    if (contentType.Contains("application/x-www-form-urlencoded"))
                    {
                        HttpContentParser parser = new HttpContentParser(data);
                        if (parser.Success)
                        {
                            FormData result = new FormData(parser.Parameters);
                            resolve(result);
                        }
                        else
                            reject(new System.Exception("Parsing failed"));
                    }
                    else if (contentType.Contains("multipart/form-data"))
                    {
                        HttpMultipartParser parser = new HttpMultipartParser(data, "file");
                        if (parser.Success)
                        {
                            FormData result = new FormData(parser.Parameters);
                            result.Append(parser.Filename, new File(parser.FileContents, parser.Filename));
                            resolve(result);
                        }
                        else
                            reject(new System.Exception("Parsing failed"));
                    }
                    else
                        reject(new System.Exception("The content type of this body does not contain form data"));
                });
            });
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