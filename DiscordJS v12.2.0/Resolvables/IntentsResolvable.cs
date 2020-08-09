using JavaScript;
using System;
using System.Collections.Generic;

namespace DiscordJS.Resolvables
{
    /// <summary>
    /// Data that can be resolved to give a permission number. This can be:
    /// <list type="bullet">
    /// <item>A string</item>
    /// <item>A bit number</item>
    /// <item>An instance of Intents</item>
    /// <item>An Array of IntentsResolvable</item>
    /// </list>
    /// </summary>
    public class IntentsResolvable : BitResolvableBase<IntentsResolvable>
    {
        public IntentsResolvable(PermissionResolvable bit) : base(
            (int)bit.type,
            bit.bit,
            bit.flag,
            bit.array == null ? null : Array<PermissionResolvable>.Map<IntentsResolvable>(bit.array, (b) => b)
            )
        { }

        public IntentsResolvable(BitFieldResolvable bit) : base(
            (int)bit.type,
            bit.bit,
            bit.flag,
            bit.array == null ? null : Array<BitFieldResolvable>.Map<IntentsResolvable>(bit.array, (b) => b)
            )
        { }

        public IntentsResolvable(SystemChannelFlagsResolvable bit) : base(
            (int)bit.type,
            bit.bit,
            bit.flag,
            bit.array == null ? null : Array<SystemChannelFlagsResolvable>.Map<IntentsResolvable>(bit.array, (b) => b)
            )
        { }

        public IntentsResolvable(PermissionResolvable[] bits) : base(Array<PermissionResolvable>.Map<IntentsResolvable>(bits, (b) => b))
        { }

        public IntentsResolvable(BitFieldResolvable[] bits) : base(Array<BitFieldResolvable>.Map<IntentsResolvable>(bits, (b) => b))
        { }

        public IntentsResolvable(SystemChannelFlagsResolvable[] bits) : base(Array<SystemChannelFlagsResolvable>.Map<IntentsResolvable>(bits, (b) => b))
        { }

        public IntentsResolvable(Array<PermissionResolvable> bits) : this(bits.ToArray())
        { }

        public IntentsResolvable(Array<BitFieldResolvable> bits) : this(bits.ToArray())
        { }

        public IntentsResolvable(Array<SystemChannelFlagsResolvable> bits) : this(bits.ToArray())
        { }


        public IntentsResolvable(Array<IntentsResolvable> bits) : this(bits.ToArray())
        { }

        public IntentsResolvable(IEnumerable<IntentsResolvable> bits) : base(bits)
        { }

        public IntentsResolvable(IntentsResolvable[] bits) : base(bits)
        { }

        public IntentsResolvable(string flag) : base(flag)
        { }

        public IntentsResolvable(sbyte bit) : base(bit)
        { }

        public IntentsResolvable(byte bit) : base(bit)
        { }

        public IntentsResolvable(short bit) : base(bit)
        { }

        public IntentsResolvable(ushort bit) : base(bit)
        { }

        public IntentsResolvable(int bit) : base(bit)
        { }

        public IntentsResolvable(uint bit) : base(bit)
        { }

        public IntentsResolvable(long bit) : base(bit)
        { }

        public IntentsResolvable(BitField bit) : base(bit.Bit)
        { }

        public IntentsResolvable(Intents bit) : base(bit.Bit)
        { }

        public IntentsResolvable(Permissions bit) : base(bit.Bit)
        { }

        public IntentsResolvable(SystemChannelFlags bit) : base(bit.Bit)
        { }

        public IntentsResolvable(Enum flag) : base(flag)
        { }

        public static implicit operator IntentsResolvable(string bit) => new IntentsResolvable(bit);
        public static implicit operator IntentsResolvable(sbyte bit) => new IntentsResolvable(bit);
        public static implicit operator IntentsResolvable(byte bit) => new IntentsResolvable(bit);
        public static implicit operator IntentsResolvable(short bit) => new IntentsResolvable(bit);
        public static implicit operator IntentsResolvable(ushort bit) => new IntentsResolvable(bit);
        public static implicit operator IntentsResolvable(int bit) => new IntentsResolvable(bit);
        public static implicit operator IntentsResolvable(uint bit) => new IntentsResolvable(bit);
        public static implicit operator IntentsResolvable(long bit) => new IntentsResolvable(bit);
        public static implicit operator IntentsResolvable(BitField bit) => new IntentsResolvable(bit);
        public static implicit operator IntentsResolvable(BitFieldResolvable bit) => new IntentsResolvable(bit);
        public static implicit operator IntentsResolvable(Intents bits) => new IntentsResolvable(bits);
        public static implicit operator IntentsResolvable(IntentsResolvable[] bits) => new IntentsResolvable(bits);
        public static implicit operator IntentsResolvable(Array<IntentsResolvable> bits) => new IntentsResolvable(bits);
        public static implicit operator IntentsResolvable(List<IntentsResolvable> bits) => new IntentsResolvable(bits);
        public static implicit operator IntentsResolvable(Permissions bits) => new IntentsResolvable(bits);
        public static implicit operator IntentsResolvable(PermissionResolvable bits) => new IntentsResolvable(bits);
        public static implicit operator IntentsResolvable(SystemChannelFlags bits) => new IntentsResolvable(bits);
        public static implicit operator IntentsResolvable(SystemChannelFlagsResolvable bits) => new IntentsResolvable(bits);
        public static implicit operator IntentsResolvable(Enum flag) => new IntentsResolvable(flag);
    }
}