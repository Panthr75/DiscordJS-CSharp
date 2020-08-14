namespace DiscordJS
{
    /// <summary>
    /// The interface for an object that is defaultable
    /// </summary>
    public interface IDefaultableObject
    {
        /// <summary>
        /// Implements the default values for this object
        /// </summary>
        void ImplementDefault();

        /// <summary>
        /// Overrides values from the given default object
        /// </summary>
        /// <param name="def">The default</param>
        void FromDefault(object def);
    }
}
