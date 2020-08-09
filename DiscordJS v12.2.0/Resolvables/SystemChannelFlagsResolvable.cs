using System;
using System.Collections.Generic;
using JavaScript;

namespace DiscordJS.Resolvables
{
    /// <summary>
    /// Data that can be resolved to give a sytem channel flag bitfield. This can be:
    /// <list type="bullet">
    /// <item>A string (see <see cref="SystemChannelFlags.FLAGS"/>)</item>
    /// <item>A sytem channel flag</item>
    /// <item>An instance of SystemChannelFlags</item>
    /// <item>An Array of SystemChannelFlagsResolvable</item>
    /// </list>
    /// </summary>
    public class SystemChannelFlagsResolvable : BitResolvableBase<SystemChannelFlagsResolvable>
    {
        public SystemChannelFlagsResolvable(PermissionResolvable bit) : base(
            (int)bit.type,
            bit.bit,
            bit.flag,
            bit.array == null ? null : Array<PermissionResolvable>.Map<SystemChannelFlagsResolvable>(bit.array, (b) => b)
            )
        { }

        public SystemChannelFlagsResolvable(IntentsResolvable bit) : base(
            (int)bit.type,
            bit.bit,
            bit.flag,
            bit.array == null ? null : Array<IntentsResolvable>.Map<SystemChannelFlagsResolvable>(bit.array, (b) => b)
            )
        { }

        public SystemChannelFlagsResolvable(BitFieldResolvable bit) : base(
            (int)bit.type,
            bit.bit,
            bit.flag,
            bit.array == null ? null : Array<BitFieldResolvable>.Map<SystemChannelFlagsResolvable>(bit.array, (b) => b)
            )
        { }

        public SystemChannelFlagsResolvable(PermissionResolvable[] bits) : base(Array<PermissionResolvable>.Map<SystemChannelFlagsResolvable>(bits, (b) => b))
        { }

        public SystemChannelFlagsResolvable(IntentsResolvable[] bits) : base(Array<IntentsResolvable>.Map<SystemChannelFlagsResolvable>(bits, (b) => b))
        { }

        public SystemChannelFlagsResolvable(BitFieldResolvable[] bits) : base(Array<BitFieldResolvable>.Map<SystemChannelFlagsResolvable>(bits, (b) => b))
        { }

        public SystemChannelFlagsResolvable(Array<PermissionResolvable> bits) : this(bits.ToArray())
        { }

        public SystemChannelFlagsResolvable(Array<IntentsResolvable> bits) : this(bits.ToArray())
        { }

        public SystemChannelFlagsResolvable(Array<BitFieldResolvable> bits) : this(bits.ToArray())
        { }


        public SystemChannelFlagsResolvable(Array<SystemChannelFlagsResolvable> bits) : this(bits.ToArray())
        { }

        public SystemChannelFlagsResolvable(IEnumerable<SystemChannelFlagsResolvable> bits) : base(bits)
        { }

        public SystemChannelFlagsResolvable(SystemChannelFlagsResolvable[] bits) : base(bits)
        { }

        public SystemChannelFlagsResolvable(string flag) : base(flag)
        { }

        public SystemChannelFlagsResolvable(sbyte bit) : base(bit)
        { }

        public SystemChannelFlagsResolvable(byte bit) : base(bit)
        { }

        public SystemChannelFlagsResolvable(short bit) : base(bit)
        { }

        public SystemChannelFlagsResolvable(ushort bit) : base(bit)
        { }

        public SystemChannelFlagsResolvable(int bit) : base(bit)
        { }

        public SystemChannelFlagsResolvable(uint bit) : base(bit)
        { }

        public SystemChannelFlagsResolvable(long bit) : base(bit)
        { }

        public SystemChannelFlagsResolvable(BitField bit) : base(bit.Bit)
        { }

        public SystemChannelFlagsResolvable(Intents bit) : base(bit.Bit)
        { }

        public SystemChannelFlagsResolvable(Permissions bit) : base(bit.Bit)
        { }

        public SystemChannelFlagsResolvable(SystemChannelFlags bit) : base(bit.Bit)
        { }

        public SystemChannelFlagsResolvable(Enum flag) : base(flag)
        { }

        public static implicit operator SystemChannelFlagsResolvable(string bit) => new SystemChannelFlagsResolvable(bit);
        public static implicit operator SystemChannelFlagsResolvable(sbyte bit) => new SystemChannelFlagsResolvable(bit);
        public static implicit operator SystemChannelFlagsResolvable(byte bit) => new SystemChannelFlagsResolvable(bit);
        public static implicit operator SystemChannelFlagsResolvable(short bit) => new SystemChannelFlagsResolvable(bit);
        public static implicit operator SystemChannelFlagsResolvable(ushort bit) => new SystemChannelFlagsResolvable(bit);
        public static implicit operator SystemChannelFlagsResolvable(int bit) => new SystemChannelFlagsResolvable(bit);
        public static implicit operator SystemChannelFlagsResolvable(uint bit) => new SystemChannelFlagsResolvable(bit);
        public static implicit operator SystemChannelFlagsResolvable(long bit) => new SystemChannelFlagsResolvable(bit);
        public static implicit operator SystemChannelFlagsResolvable(BitField bit) => new SystemChannelFlagsResolvable(bit);
        public static implicit operator SystemChannelFlagsResolvable(BitFieldResolvable bits) => new SystemChannelFlagsResolvable(bits);
        public static implicit operator SystemChannelFlagsResolvable(Intents bits) => new SystemChannelFlagsResolvable(bits);
        public static implicit operator SystemChannelFlagsResolvable(IntentsResolvable bits) => new SystemChannelFlagsResolvable(bits);
        public static implicit operator SystemChannelFlagsResolvable(Permissions bits) => new SystemChannelFlagsResolvable(bits);
        public static implicit operator SystemChannelFlagsResolvable(PermissionResolvable bits) => new SystemChannelFlagsResolvable(bits);
        public static implicit operator SystemChannelFlagsResolvable(SystemChannelFlags bits) => new SystemChannelFlagsResolvable(bits);
        public static implicit operator SystemChannelFlagsResolvable(SystemChannelFlagsResolvable[] bits) => new SystemChannelFlagsResolvable(bits);
        public static implicit operator SystemChannelFlagsResolvable(Array<SystemChannelFlagsResolvable> bits) => new SystemChannelFlagsResolvable(bits);
        public static implicit operator SystemChannelFlagsResolvable(List<SystemChannelFlagsResolvable> bits) => new SystemChannelFlagsResolvable(bits);
        public static implicit operator SystemChannelFlagsResolvable(Enum flag) => new SystemChannelFlagsResolvable(flag);
    }
}