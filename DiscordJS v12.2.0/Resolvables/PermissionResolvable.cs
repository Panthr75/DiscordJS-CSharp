using System;
using System.Collections.Generic;
using JavaScript;

namespace DiscordJS.Resolvables
{
    /// <summary>
    /// Data that can be resolved to give a permission number. This can be:
    /// <list type="bullet">
    /// <item>A string. (See <see cref="Permissions.FLAGS"/>)</item>
    /// <item>A permission number</item>
    /// <item>An instance of Permissions</item>
    /// <item>An Array of PermissionResolvable</item>
    /// </list>
    /// </summary>
    public class PermissionResolvable : BitResolvableBase<PermissionResolvable>
    {
        public PermissionResolvable(BitFieldResolvable bit) : base(
            (int)bit.type,
            bit.bit,
            bit.flag,
            bit.array == null ? null : Array<BitFieldResolvable>.Map<PermissionResolvable>(bit.array, (b) => b)
            )
        { }

        public PermissionResolvable(IntentsResolvable bit) : base(
            (int)bit.type,
            bit.bit,
            bit.flag,
            bit.array == null ? null : Array<IntentsResolvable>.Map<PermissionResolvable>(bit.array, (b) => b)
            )
        { }

        public PermissionResolvable(SystemChannelFlagsResolvable bit) : base(
            (int)bit.type,
            bit.bit,
            bit.flag,
            bit.array == null ? null : Array<SystemChannelFlagsResolvable>.Map<PermissionResolvable>(bit.array, (b) => b)
            )
        { }

        public PermissionResolvable(BitFieldResolvable[] bits) : base(Array<BitFieldResolvable>.Map<PermissionResolvable>(bits, (b) => b))
        { }

        public PermissionResolvable(IntentsResolvable[] bits) : base(Array<IntentsResolvable>.Map<PermissionResolvable>(bits, (b) => b))
        { }

        public PermissionResolvable(SystemChannelFlagsResolvable[] bits) : base(Array<SystemChannelFlagsResolvable>.Map<PermissionResolvable>(bits, (b) => b))
        { }

        public PermissionResolvable(Array<BitFieldResolvable> bits) : this(bits.ToArray())
        { }

        public PermissionResolvable(Array<IntentsResolvable> bits) : this(bits.ToArray())
        { }

        public PermissionResolvable(Array<SystemChannelFlagsResolvable> bits) : this(bits.ToArray())
        { }


        public PermissionResolvable(Array<PermissionResolvable> bits) : this(bits.ToArray())
        { }

        public PermissionResolvable(IEnumerable<PermissionResolvable> bits) : base(bits)
        { }

        public PermissionResolvable(PermissionResolvable[] bits) : base(bits)
        { }

        public PermissionResolvable(string flag) : base(flag)
        { }

        public PermissionResolvable(sbyte bit) : base(bit)
        { }

        public PermissionResolvable(byte bit) : base(bit)
        { }

        public PermissionResolvable(short bit) : base(bit)
        { }

        public PermissionResolvable(ushort bit) : base(bit)
        { }

        public PermissionResolvable(int bit) : base(bit)
        { }

        public PermissionResolvable(uint bit) : base(bit)
        { }

        public PermissionResolvable(long bit) : base(bit)
        { }

        public PermissionResolvable(BitField bit) : base(bit.Bit)
        { }

        public PermissionResolvable(Intents bit) : base(bit.Bit)
        { }

        public PermissionResolvable(Permissions bit) : base(bit.Bit)
        { }

        public PermissionResolvable(SystemChannelFlags bit) : base(bit.Bit)
        { }

        public PermissionResolvable(Enum flag) : base(flag)
        { }

        public static implicit operator PermissionResolvable(string bit) => new PermissionResolvable(bit);
        public static implicit operator PermissionResolvable(sbyte bit) => new PermissionResolvable(bit);
        public static implicit operator PermissionResolvable(byte bit) => new PermissionResolvable(bit);
        public static implicit operator PermissionResolvable(short bit) => new PermissionResolvable(bit);
        public static implicit operator PermissionResolvable(ushort bit) => new PermissionResolvable(bit);
        public static implicit operator PermissionResolvable(int bit) => new PermissionResolvable(bit);
        public static implicit operator PermissionResolvable(uint bit) => new PermissionResolvable(bit);
        public static implicit operator PermissionResolvable(long bit) => new PermissionResolvable(bit);
        public static implicit operator PermissionResolvable(BitField bit) => new PermissionResolvable(bit);
        public static implicit operator PermissionResolvable(BitFieldResolvable bit) => new PermissionResolvable(bit);
        public static implicit operator PermissionResolvable(Intents bits) => new PermissionResolvable(bits);
        public static implicit operator PermissionResolvable(IntentsResolvable bits) => new PermissionResolvable(bits);
        public static implicit operator PermissionResolvable(Permissions bits) => new PermissionResolvable(bits);
        public static implicit operator PermissionResolvable(PermissionResolvable[] bits) => new PermissionResolvable(bits);
        public static implicit operator PermissionResolvable(Array<PermissionResolvable> bits) => new PermissionResolvable(bits);
        public static implicit operator PermissionResolvable(List<PermissionResolvable> bits) => new PermissionResolvable(bits);
        public static implicit operator PermissionResolvable(SystemChannelFlags bits) => new PermissionResolvable(bits);
        public static implicit operator PermissionResolvable(SystemChannelFlagsResolvable bits) => new PermissionResolvable(bits);
        public static implicit operator PermissionResolvable(Enum flag) => new PermissionResolvable(flag);
    }
}