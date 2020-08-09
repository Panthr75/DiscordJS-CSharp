using System;

namespace DiscordJS
{
    /// <summary>
    /// Options for Image URLs.
    /// </summary>
    public class ImageURLOptions
    {
        private static readonly int[] validSizes = new int[] { 16, 32, 64, 128, 256, 512, 1035, 2048, 4096 };
        private static readonly string[] validFormats = new string[] { "webp", "png", "jpg", "jpeg", "gif" };

        /// <summary>
        /// One of webp, png, jpg, jpeg, gif. If no format is provided, defaults to webp.
        /// </summary>
        public readonly string format;

        /// <summary>
        /// If true, the format will dynamically change to gif for animated avatars; the default is false.
        /// </summary>
        public readonly bool dynamic;

        /// <summary>
        /// One of 16, 32, 64, 128, 256, 512, 1024, 2048, 4096
        /// </summary>
        public readonly int size;

        public ImageURLOptions() : this("webp", false, 512)
        { }

        public ImageURLOptions(string format, bool dynamic, int size)
        {
            this.dynamic = dynamic;
            bool setFormat = false, setSize = false;
            for (int index = 0, length = validSizes.Length; index < length; index++)
            {
                if (validSizes[index] == size)
                {
                    this.size = size;
                    setSize = true;
                    break;
                }
            }
            if (!setSize) throw new ArgumentOutOfRangeException("size", "Size must be one of 16, 32, 64, 128, 256, 512, 1024, 2048, 4096");
            if (format == null)
            {
                this.format = validFormats[0];
            }
            else
            {
                for (int index = 0, length = validFormats.Length; index < length; index++)
                {
                    if (validFormats[index] == format)
                    {
                        this.format = format;
                        setFormat = true;
                        break;
                    }
                }
                if (!setFormat) throw new ArgumentOutOfRangeException("format", "Format must be one of webp, png, jpg, jpeg, gif.");
            }
        }
    }
}