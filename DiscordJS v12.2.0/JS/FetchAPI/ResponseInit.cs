namespace JavaScript.Web
{
    public sealed class ResponseInit
    {
        public ushort Status { get; set; } = 200;
        public string StatusText { get; set; } = "";
        public HeadersInit Headers { get; set; } = null;
    }
}