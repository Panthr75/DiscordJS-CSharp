namespace DiscordJS
{
    /// <summary>
    /// Represents an Object that has an ID
    /// </summary>
    public interface IHasID
    {
        /// <summary>
        /// The ID of this Object
        /// </summary>
        Snowflake ID { get; }
    }
}