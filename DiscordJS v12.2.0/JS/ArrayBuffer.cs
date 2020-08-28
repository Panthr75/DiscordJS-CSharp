namespace JavaScript
{
    public class ArrayBuffer
    {
        internal int byteLength;
        internal int byteOffset;
        internal byte[] block;

        public static bool IsView(object arg)
        {
            //
        }

        public int ByteLength { get; }

        public ArrayBuffer Slice(int begin, int end)
        {
            //
        }

        internal static void RawBytesToNumber()
        {
            //
        }

        internal static int ElementSize<T, This>(This typedArray) where This : TypedArray<T, This>, new()
        {
            if (typedArray is Int8Array) return 1;
            return 0;
        }
    }
}