namespace JavaScript.Web
{
    public class File : Blob
    {
        public File(byte[] fileBits, string fileName) : base(fileBits, "file")
        {
            //
        }

        public File(Blob fileBits, string fileName) : base(fileBits, "file")
        {
            //
        }
    }
}