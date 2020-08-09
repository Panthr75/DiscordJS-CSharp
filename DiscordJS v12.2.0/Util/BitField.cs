using DiscordJS.Resolvables;
using JavaScript;
using System;

namespace DiscordJS
{
    /// <summary>
    /// Data structure that makes it easy to interact with a bitfield.
    /// </summary>
    public class BitField
    {
        /// <summary>
        /// Bitfield of the packed bits
        /// </summary>
        public long Bit { get; private set; }

        /// <summary>
        /// Whether or not this bitfield is frozen
        /// </summary>
        protected bool Frozen { get; private set; } = false;

        /// <summary>
        /// Type of the Flags
        /// </summary>
        protected virtual Type FlagsType { get; }

        /// <summary>
        /// Creates a new bitfield
        /// </summary>
        /// <param name="bits">Bit(s) to read from</param>
        public BitField(BitFieldResolvable bits)
        {
            Bit = Resolve(bits);
        }

        /// <summary>
        /// Checks whether the bitfield has a bit, or any of multiple bits.
        /// </summary>
        /// <param name="bit">Bit(s) to check for</param>
        /// <returns></returns>
        public bool Any(BitFieldResolvable bit) => (Bit & Resolve(bit)) != 0;

        /// <summary>
        /// Checks if this bitfield equals another
        /// </summary>
        /// <param name="bit">Bit(s) to check for</param>
        /// <returns></returns>
        public bool Equals(BitFieldResolvable bit) => Bit == Resolve(bit);

        /// <summary>
        /// Checks whether the bitfield has a bit, or multiple bits.
        /// </summary>
        /// <param name="bit">Bit(s) to check for</param>
        /// <returns></returns>
        public bool Has(BitFieldResolvable bit)
        {
            if (bit.isArray)
            {
                Array<BitFieldResolvable> bits = new Array<BitFieldResolvable>(bit.array);
                Func<BitFieldResolvable, bool> fn = (p) => Has(p);
                return bits.Every(fn);
            }
            long b = Resolve(bit);
            return (Bit & b) == b;
        }

        /// <summary>
        /// Freezes these bits, making them immutable.
        /// </summary>
        /// <returns>These bits</returns>
        public BitField Freeze()
        {
            Frozen = true;
            return this;
        }

        /// <summary>
        /// Adds bits to these ones.
        /// </summary>
        /// <param name="bits">Bits to add</param>
        /// <returns>These bits or new BitField if the instance is frozen.</returns>
        public BitField Add(params BitFieldResolvable[] bits)
        {
            long total = 0;
            for (int index = 0, length = bits.Length; index < length; index++)
            {
                total |= Resolve(bits[index]);
            }
            if (Frozen) return new BitField(Bit | total);
            Bit |= total;
            return this;
        }

        /// <summary>
        /// Removes bits from these.
        /// </summary>
        /// <param name="bits">Bits to remove</param>
        /// <returns>These bits or new BitField if the instance is frozen.</returns>
        public BitField Remove(params BitFieldResolvable[] bits)
        {
            long total = 0;
            for (int index = 0, length = bits.Length; index < length; index++)
            {
                total |= Resolve(bits[index]);
            }
            if (Frozen) return new BitField(Bit & ~total);
            Bit &= ~total;
            return this;
        }

        /// <summary>
        /// Gets an Array of bitfield names based on the bits available.
        /// </summary>
        /// <returns></returns>
        public Array<string> ToArray()
        {
            Func<string, bool> filter = (bit) => Has(bit);
            return new Array<string>(Enum.GetNames(FlagsType)).Filter(filter);
        }

        private long Resolve(BitFieldResolvable bit)
        {
            if (bit != null) return bit.Resolve(FlagsType);
            return 0;
        }
    }
}