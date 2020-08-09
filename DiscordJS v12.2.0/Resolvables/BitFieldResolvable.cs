using JavaScript;
using System;
using System.Collections.Generic;

namespace DiscordJS.Resolvables
{
    /// <summary>
    /// Data that can be resolved to give a bitfield. This can be:
    /// <list type="bullet">
    /// <item>A string</item>
    /// <item>A bit number</item>
    /// <item>An instance of BitField</item>
    /// <item>An Array of BitFieldResolvable</item>
    /// </list>
    /// </summary>
    public class BitFieldResolvable : BitResolvableBase<BitFieldResolvable>
    {
        public BitFieldResolvable(PermissionResolvable bit) : base(
            (int)bit.type,
            bit.bit,
            bit.flag,
            bit.array == null ? null : Array<PermissionResolvable>.Map<BitFieldResolvable>(bit.array, (b) => b)
            )
        { }

        public BitFieldResolvable(IntentsResolvable bit) : base(
            (int)bit.type,
            bit.bit,
            bit.flag,
            bit.array == null ? null : Array<IntentsResolvable>.Map<BitFieldResolvable>(bit.array, (b) => b)
            )
        { }

        public BitFieldResolvable(SystemChannelFlagsResolvable bit) : base(
            (int)bit.type,
            bit.bit,
            bit.flag,
            bit.array == null ? null : Array<SystemChannelFlagsResolvable>.Map<BitFieldResolvable>(bit.array, (b) => b)
            )
        { }

        public BitFieldResolvable(PermissionResolvable[] bits) : base(Array<PermissionResolvable>.Map<BitFieldResolvable>(bits, (b) => b))
        { }

        public BitFieldResolvable(IntentsResolvable[] bits) : base(Array<IntentsResolvable>.Map<BitFieldResolvable>(bits, (b) => b))
        { }

        public BitFieldResolvable(SystemChannelFlagsResolvable[] bits) : base(Array<SystemChannelFlagsResolvable>.Map<BitFieldResolvable>(bits, (b) => b))
        { }

        public BitFieldResolvable(Array<PermissionResolvable> bits) : this(bits.ToArray())
        { }

        public BitFieldResolvable(Array<IntentsResolvable> bits) : this(bits.ToArray())
        { }

        public BitFieldResolvable(Array<SystemChannelFlagsResolvable> bits) : this(bits.ToArray())
        { }


        public BitFieldResolvable(Array<BitFieldResolvable> bits) : this(bits.ToArray())
        { }

        public BitFieldResolvable(IEnumerable<BitFieldResolvable> bits) : base(bits)
        { }

        public BitFieldResolvable(BitFieldResolvable[] bits) : base(bits)
        { }

        public BitFieldResolvable(string flag) : base(flag)
        { }

        public BitFieldResolvable(sbyte bit) : base(bit)
        { }

        public BitFieldResolvable(byte bit) : base(bit)
        { }

        public BitFieldResolvable(short bit) : base(bit)
        { }

        public BitFieldResolvable(ushort bit) : base(bit)
        { }

        public BitFieldResolvable(int bit) : base(bit)
        { }

        public BitFieldResolvable(uint bit) : base(bit)
        { }

        public BitFieldResolvable(long bit) : base(bit)
        { }

        public BitFieldResolvable(BitField bit) : base(bit.Bit)
        { }

        public BitFieldResolvable(Intents bit) : base(bit.Bit)
        { }

        public BitFieldResolvable(Permissions bit) : base(bit.Bit)
        { }

        public BitFieldResolvable(SystemChannelFlags bit) : base(bit.Bit)
        { }

        public BitFieldResolvable(Enum flag) : base(flag)
        { }

        public static implicit operator BitFieldResolvable(string bit) => new BitFieldResolvable(bit);
        public static implicit operator BitFieldResolvable(sbyte bit) => new BitFieldResolvable(bit);
        public static implicit operator BitFieldResolvable(byte bit) => new BitFieldResolvable(bit);
        public static implicit operator BitFieldResolvable(short bit) => new BitFieldResolvable(bit);
        public static implicit operator BitFieldResolvable(ushort bit) => new BitFieldResolvable(bit);
        public static implicit operator BitFieldResolvable(int bit) => new BitFieldResolvable(bit);
        public static implicit operator BitFieldResolvable(uint bit) => new BitFieldResolvable(bit);
        public static implicit operator BitFieldResolvable(long bit) => new BitFieldResolvable(bit);
        public static implicit operator BitFieldResolvable(BitField bit) => new BitFieldResolvable(bit);
        public static implicit operator BitFieldResolvable(BitFieldResolvable[] bits) => new BitFieldResolvable(bits);
        public static implicit operator BitFieldResolvable(Array<BitFieldResolvable> bits) => new BitFieldResolvable(bits);
        public static implicit operator BitFieldResolvable(List<BitFieldResolvable> bits) => new BitFieldResolvable(bits);
        public static implicit operator BitFieldResolvable(Intents bits) => new BitFieldResolvable(bits);
        public static implicit operator BitFieldResolvable(IntentsResolvable bits) => new BitFieldResolvable(bits);
        public static implicit operator BitFieldResolvable(Permissions bits) => new BitFieldResolvable(bits);
        public static implicit operator BitFieldResolvable(PermissionResolvable bits) => new BitFieldResolvable(bits);
        public static implicit operator BitFieldResolvable(SystemChannelFlags bits) => new BitFieldResolvable(bits);
        public static implicit operator BitFieldResolvable(SystemChannelFlagsResolvable bits) => new BitFieldResolvable(bits);
        public static implicit operator BitFieldResolvable(Enum flag) => new BitFieldResolvable(flag);
    }
}