using System;
using System.IO;

namespace JavaScript.Web
{
    /// <summary>
    /// A file-like object of immutable, raw data. Blobs represent data that isn't necessarily in a JavaScript-native format. The File interface is based on Blob, inheriting blob functionality and expanding it to support files on the user's system.
    /// </summary>
    public class Blob
    {
        public int Size { get; }
        public string Type { get; }

        public Blob()
        {
            Size = 0;
            Type = string.Empty;
            byteSequence = new byte[0] { };
        }

        internal Blob(Blob buffer, string type)
        {
            //
        }

        internal Blob(byte[] buffer, string type)
        {
            //
        }

        internal byte[] byteSequence;

        private Stream GetStream()
        {
            return new MemoryStream(byteSequence);
        }

        public IPromise<ArrayBuffer> ArrayBuffer()
        {
            return new Promise<ArrayBuffer>(async (resolve, reject) =>
            {
                try
                {
                    Stream stream = GetStream();
                    byte[] buffer = new byte[stream.Length];
                    await stream.ReadAsync(buffer, 0, (int)stream.Length);
                    ArrayBuffer result = new ArrayBuffer();
                    result.contents = buffer;
                    resolve(result);
                }
                catch (Exception ex)
                {
                    reject(ex);
                }
            });
        }

        public Blob Slice(int start, int end, string contentType)
        {
            if (start < 0)
                start = Math.Max(Size + start, 0);
            else
                start = Math.Min(start, Size);

            if (end < 0)
                end = Math.Max(Size + end, 0);
            else
                end = Math.Min(end, Size);

            if (contentType == null)
                contentType = "";
            else
            {
                //TODO: set contentType to empty string if it contains any characters outside the range
                // of U+0020 to U+007E
                contentType = contentType.ToLower();
            }

            int span = Math.Max(end - start, 0);

            byte[] bytes = new byte[span];

            Array.Copy(byteSequence, 0, bytes, start, span);
            return new Blob(bytes, contentType);
        }

        public Stream Stream() => GetStream();

        public IPromise<string> Text()
        {
            return new Promise<string>(async (resolve, reject) =>
            {
                try
                {
                    StreamReader reader = new StreamReader(GetStream(), System.Text.Encoding.UTF8);
                    string result = await reader.ReadToEndAsync();
                    reader.Close();
                    resolve(result);
                }
                catch(Exception ex)
                {
                    reject(ex);
                }
            });
        }
    }
}