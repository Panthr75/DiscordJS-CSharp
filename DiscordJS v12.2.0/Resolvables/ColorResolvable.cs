using JavaScript;
using System;
using System.Collections.Generic;

namespace DiscordJS.Resolvables
{
    /// <summary>
    /// Can be a number, hex string, an RGB array like: <see langword="new"/> <see cref="int"/>[3] { 255, 0, 255 }
    /// <br/>or one of the following strings:
    /// <list type="bullet">
    /// <item>DEFAULT</item>
    /// <item>WHITE</item>
    /// <item>AQUA</item>
    /// <item>GREEN</item>
    /// <item>BLUE</item>
    /// <item>YELLOW</item>
    /// <item>PURPLE</item>
    /// <item>LUMINOUS_VIVID_PINK</item>
    /// <item>GOLD</item>
    /// <item>ORANGE</item>
    /// <item>RED</item>
    /// <item>GREY</item>
    /// <item>DARKER_GREY</item>
    /// <item>NAVY</item>
    /// <item>DARK_AQUA</item>
    /// <item>DARK_GREEN</item>
    /// <item>DARK_BLUE</item>
    /// <item>DARK_PURPLE</item>
    /// <item>DARK_VIVID_PINK</item>
    /// <item>DARK_GOLD</item>
    /// <item>DARK_ORANGE</item>
    /// <item>DARK_RED</item>
    /// <item>DARK_GREY</item>
    /// <item>LIGHT_GREY</item>
    /// <item>DARK_NAVY</item>
    /// <item>RANDOM</item>
    /// </list>
    /// </summary>
    public class ColorResolvable
    {
        internal enum Type
        {
            Color,
            String,
            RGB
        }

        internal readonly Type type;

        internal readonly int color;
        internal readonly string str;
        internal readonly int[] rgb;

        internal int Resolve()
        {
            if (type == Type.String)
            {
                if (str == "RANDOM") return new Random().Next(0, 0xFFFFFF + 1);
                else if (str == "DEFAULT") return 0;
                else if (str == "WHITE") return 0xffffff;
                else if (str == "AQUA") return 0x1abc9c;
                else if (str == "GREEN") return 0x2ecc71;
                else if (str == "BLUE") return 0x3498db;
                else if (str == "YELLOW") return 0xffff00;
                else if (str == "PURPLE") return 0x9b59b6;
                else if (str == "LUMINOUS_VIVID_PINK") return 0xe91e63;
                else if (str == "GOLD") return 0xf1c40f;
                else if (str == "ORANGE") return 0xe67e22;
                else if (str == "RED") return 0xe74c3c;
                else if (str == "GREY") return 0x95a5a6;
                else if (str == "NAVY") return 0x34495e;
                else if (str == "DARK_AQUA") return 0x11806a;
                else if (str == "DARK_GREEN") return 0x1f8b4c;
                else if (str == "DARK_BLUE") return 0x206694;
                else if (str == "DARK_PURPLE") return 0x71368a;
                else if (str == "DARK_VIVID_PINK") return 0xad1457;
                else if (str == "DARK_GOLD") return 0xc27c0e;
                else if (str == "DARK_ORANGE") return 0xa84300;
                else if (str == "DARK_RED") return 0x992d22;
                else if (str == "DARK_GREY") return 0x979c9f;
                else if (str == "DARKER_GREY") return 0x7f8c8d;
                else if (str == "LIGHT_GREY") return 0xbcc0c0;
                else if (str == "DARK_NAVY") return 0x2c3e50;
                else if (str == "BLURPLE") return 0x7289da;
                else if (str == "GREYPLE") return 0x99aab5;
                else if (str == "DARK_BUT_NOT_BLACK") return 0x2c2f33;
                else if (str == "NOT_QUITE_BLACK") return 0x23272a;
                else throw new DJSError.Error("COLOR_CONVERT");
            }

            int c;

            if (type == Type.RGB)
                c = (rgb[0] << 16) + (rgb[1] << 8) + rgb[2];
            else if (type == Type.Color)
                c = color;
            else
                throw new ArgumentException("The given type can't be cast to this resolvable");

            if (c < 0 || c > 0xFFFFFF) throw new DJSError.Error("COLOR_RANGE");
            return c;
        }

        public ColorResolvable(string str)
        {
            this.str = str.ToUpper();
            type = Type.String;
        }

        public ColorResolvable(JavaScript.String str)
        {
            this.str = str.ToUpperCase();
            type = Type.String;
        }

        public ColorResolvable(Snowflake str)
        {
            this.str = str.ToUpperCase();
            type = Type.String;
        }

        public ColorResolvable(byte color)
        {
            this.color = color;
            type = Type.Color;
        }

        public ColorResolvable(sbyte color)
        {
            this.color = color;
            type = Type.Color;
        }

        public ColorResolvable(short color)
        {
            this.color = color;
            type = Type.Color;
        }

        public ColorResolvable(ushort color)
        {
            this.color = color;
            type = Type.Color;
        }

        public ColorResolvable(int color)
        {
            this.color = color;
            type = Type.Color;
        }

        public ColorResolvable(uint color)
        {
            this.color = (int)color;
            type = Type.Color;
        }

        public ColorResolvable(long color)
        {
            this.color = (int)color;
            type = Type.Color;
        }

        public ColorResolvable(ulong color)
        {
            this.color = (int)color;
            type = Type.Color;
        }

        public ColorResolvable(IEnumerable<int> rgb)
        {
            int index = 0;
            this.rgb = new int[3];
            foreach (int c in rgb)
            {
                if (index < 3) this.rgb[index] = c;
                index++;
            }
            for (; index < 3; index++)
            {
                this.rgb[index] = 0;
            }
            type = Type.RGB;
        }



        public static implicit operator ColorResolvable(string str) => new ColorResolvable(str);
        public static implicit operator ColorResolvable(JavaScript.String str) => new ColorResolvable(str);
        public static implicit operator ColorResolvable(Snowflake str) => new ColorResolvable(str);
        public static implicit operator ColorResolvable(byte oolor) => new ColorResolvable(oolor);
        public static implicit operator ColorResolvable(sbyte oolor) => new ColorResolvable(oolor);
        public static implicit operator ColorResolvable(short oolor) => new ColorResolvable(oolor);
        public static implicit operator ColorResolvable(ushort oolor) => new ColorResolvable(oolor);
        public static implicit operator ColorResolvable(int oolor) => new ColorResolvable(oolor);
        public static implicit operator ColorResolvable(uint oolor) => new ColorResolvable(oolor);
        public static implicit operator ColorResolvable(long oolor) => new ColorResolvable(oolor);
        public static implicit operator ColorResolvable(ulong oolor) => new ColorResolvable(oolor);
        public static implicit operator ColorResolvable(int[] rgb) => new ColorResolvable(rgb);
        public static implicit operator ColorResolvable(List<int> rgb) => new ColorResolvable(rgb);
        public static implicit operator ColorResolvable(Array<int> rgb) => new ColorResolvable(rgb);
    }
}