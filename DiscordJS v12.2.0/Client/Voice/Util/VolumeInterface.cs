using System;

namespace DiscordJS
{
    static class VolumeInterface
    {
        /// <summary>
        /// Whether or not the volume of this stream is editable
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <returns></returns>
        public static bool VolumeEditable<T>(T t) where T : IVolume
        {
            return true;
        }

        /// <summary>
        /// The current volume of the stream
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <returns></returns>
        public static double Volume<T>(T t) where T : IVolume
        {
            dynamic tDyn = t;
            try
            {
                return tDyn._volume;
            }
            catch (Exception)
            {
                return t.Volume;
            }
        }

        /// <summary>
        /// The current volume of the stream in decibels
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <returns></returns>
        public static double VolumeDecibels<T>(T t) where T : IVolume
        {
            return Math.Log10(t.Volume) * 20;
        }

        /// <summary>
        /// The current volume of the stream from a logarithmic scale
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <returns></returns>
        public static double VolumeLogarithmic<T>(T t) where T : IVolume
        {
            return Math.Pow(t.Volume, 1.660964d);
        }

        public static byte[] ApplyVolume<T>(T t, byte[] buffer, double? volume = null) where T : IVolume
        {
            double vol;
            if (volume.HasValue) vol = volume.Value;
            else
            {
                dynamic tDyn = t;
                try
                {
                    vol = tDyn._volume;
                }
                catch(Exception)
                {
                    vol = t.Volume;
                }
            }

            if (vol == 1) return buffer;

            var @out = new byte[buffer.Length];

            for (int i = 0, l = buffer.Length; i < l; i += 2)
            {
                if (i >= l - 1) break;
                short @uint = Math.Min((short)32767, (short)Math.Round(vol * BitConverter.ToInt16(buffer, i)));
                byte[] bytesToWrite = BitConverter.GetBytes(@uint);
                for (int index = 0, length = bytesToWrite.Length; index < length; index++)
                    @out[i + index] = bytesToWrite[index];
            }

            return @out;
        }

        /// <summary>
        /// Sets the volume relative to the input stream - i.e. 1 is normal, 0.5 is half, 2 is double.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <param name="volume">The volume that you want to set</param>
        public static void SetVolume<T>(T t, double volume) where T : IVolume
        {
            dynamic tDyn = t;

            try
            {
                tDyn._volume = volume;
            }
            catch(Exception)
            {
                t.SetVolume(volume);
            }
        }

        /// <summary>
        /// Sets the volume in decibels.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <param name="db">The decibels</param>
        public static void SetVolumeDecibels<T>(T t, double db) where T : IVolume
        {
            t.SetVolume(Math.Pow(10, db / 20));
        }

        /// <summary>
        /// Sets the volume so that a perceived value of 0.5 is half the perceived volume etc.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <param name="value">The value for the volume</param>
        public static void SetVolumeLogarithmic<T>(T t, double value) where T : IVolume
        {
            t.SetVolume(Math.Pow(value, 1.660964D));
        }
    }
}