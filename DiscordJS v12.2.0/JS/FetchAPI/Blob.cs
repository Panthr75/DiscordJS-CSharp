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

        internal Blob(byte[] buffer, string type)
        {
            //
        }

        public IPromise<ArrayBuffer> ArrayBuffer()
        {
            //
        }

        public Blob Slice(int start, int end, string contentType)
        {
            //
        }

        public Stream Stream()
        {
            //
        }

        public IPromise<string> Text()
        {
            //
        }
    }
}