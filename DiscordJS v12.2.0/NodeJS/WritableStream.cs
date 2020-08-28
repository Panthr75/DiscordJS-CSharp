namespace NodeJS
{
    public class WritableStream : EventEmitter
    {
        public class Options
        {
            private bool locked = false;

            private double seek;
            private double? volume;
            private double? plp;
            private bool? fec;
            private int bitrate;
            private int highWaterMark;

            /// <summary>
            /// The time to seek to
            /// </summary>
            public double Seek
            {
                get => seek;
                set
                {
                    if (!locked)
                        seek = value;
                }
            }

            /// <summary>
            /// The volume to play at. Set this to null to disable volume transforms for
            /// this stream to improve performance.
            /// </summary>
            public double? Volume
            {
                get => volume;
                set
                {
                    if (!locked)
                        volume = value;
                }
            }

            /// <summary>
            /// Expected packet loss percentage
            /// </summary>
            public double? PLP
            {
                get => plp;
                set
                {
                    if (!locked)
                        plp = value;
                }
            }

            /// <summary>
            /// Enabled forward error correction
            /// </summary>
            public bool? FEC
            {
                get => fec;
                set
                {
                    if (!locked)
                        fec = value;
                }
            }

            /// <summary>
            /// The bitrate (quality) of the audio in kbps.
            /// </summary>
            public int Bitrate
            {
                get => bitrate;
                set
                {
                    if (!locked)
                        bitrate = value;
                }
            }

            /// <summary>
            /// The maximum number of opus packets to make and store before they are
            /// </summary>
            public int HighWaterMark
            {
                get => highWaterMark;
                set
                {
                    if (!locked)
                        highWaterMark = value;
                }
            }

            /// <summary>
            /// Locks the options from being modified
            /// </summary>
            internal void Lock() => locked = true;

            /// <summary>
            /// Unlocks the options, allowing modification
            /// </summary>
            internal void Unlock() => locked = false;

            internal void SetSeek(double newValue) => seek = newValue;
            internal void SetVolume(double? newValue) => volume = newValue;
            internal void SetPLP(double? newValue) => plp = newValue;
            internal void SetFEC(bool? newValue) => fec = newValue;
            internal void SetBitrate(int newValue) => bitrate = newValue;
            internal void SetHighWaterMark(int newValue) => highWaterMark = newValue;
        }

        public WritableStream(Options options)
        {
            //
        }
    }
}