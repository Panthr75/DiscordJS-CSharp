using System;

namespace DiscordJS.Data
{
    /// <summary>
    /// Represents something that is not Discord data.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class NotDataAttribute : Attribute
    {
        public NotDataAttribute()
        { }
    }
}