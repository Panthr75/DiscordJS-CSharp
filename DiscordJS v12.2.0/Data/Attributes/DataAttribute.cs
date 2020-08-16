using System;

namespace DiscordJS.Data
{
    /// <summary>
    /// Represents an attribute that describes whether a class or property is raw data from Discord
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class DataAttribute : Attribute
    {
        public DataAttribute()
        { }
    }
}