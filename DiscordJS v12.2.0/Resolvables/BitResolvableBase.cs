using System;
using System.Collections.Generic;
using JavaScript;

namespace DiscordJS.Resolvables
{
    /// <summary>
    /// The base for bitfield resolvables
    /// </summary>
    /// <typeparam name="T">The type that extends this</typeparam>
    public abstract class BitResolvableBase<T> where T : BitResolvableBase<T>
    {
        internal enum Type
        {
            None = 0,
            Bit = 1,
            Flag = 2,
            Array = 3
        }

        internal readonly Type type;

        internal bool isResolved = false;
        internal long resolvedValue;

        internal readonly long bit = 0;
        internal readonly string flag = null;
        internal readonly T[] array = null;

        internal long Resolve(System.Type enumType)
        {
            if (isResolved)
                return resolvedValue;
            else
            {
                if (type == Type.Bit)
                {
                    if (bit < 0)
                        throw new ArgumentOutOfRangeException("bit", "Bitfield Values may not be less than zero");
                    resolvedValue = bit;
                    isResolved = true;
                }
                else if (type == Type.Array)
                {
                    resolvedValue = new Array<T>(array).Map((bit) => bit.Resolve(enumType)).Reduce((prev, p) => prev | p, 0L);
                    isResolved = true;
                }
                else if (type == Type.Flag)
                {
                    try
                    {
                        long value = (long)Enum.Parse(enumType, flag);
                        resolvedValue = value;
                        isResolved = true;
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Invalid flag to resolve", ex);
                    }
                }
                else
                {
                    throw new ArgumentException("The given type can't be cast to this resolvable");
                }
                return resolvedValue;
            }
        }

        internal BitResolvableBase(int type, long bit, string flag, T[] array)
        {
            this.type = (Type)type;
            this.bit = bit;
            this.flag = flag;
            this.array = array;
        }

        public BitResolvableBase(Array<T> bits) : this(bits.ToArray())
        { }

        public BitResolvableBase(IEnumerable<T> bits)
        {
            List<T> list = new List<T>();
            foreach (T bit in bits)
                list.Add(bit);
            array = list.ToArray();
            type = Type.Array;
        }

        public BitResolvableBase(T[] bits)
        {
            array = new T[bits.Length];
            Array.Copy(bits, array, bits.Length);
            type = Type.Array;
        }

        public BitResolvableBase(string flag)
        {
            this.flag = flag;
            type = Type.Flag;
        }

        public BitResolvableBase(sbyte bit)
        {
            type = Type.Bit;
            this.bit = bit;
        }

        public BitResolvableBase(byte bit)
        {
            type = Type.Bit;
            this.bit = bit;
        }

        public BitResolvableBase(short bit)
        {
            type = Type.Bit;
            this.bit = bit;
        }

        public BitResolvableBase(ushort bit)
        {
            type = Type.Bit;
            this.bit = bit;
        }

        public BitResolvableBase(int bit)
        {
            type = Type.Bit;
            this.bit = bit;
        }

        public BitResolvableBase(uint bit)
        {
            type = Type.Bit;
            this.bit = bit;
        }

        public BitResolvableBase(long bit)
        {
            type = Type.Bit;
            this.bit = bit;
        }

        public BitResolvableBase(Enum flag)
        {
            type = Type.Flag;
            this.flag = Enum.GetName(flag.GetType(), flag);
        }
    }
}