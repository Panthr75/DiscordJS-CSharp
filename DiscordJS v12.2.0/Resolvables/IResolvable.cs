namespace DiscordJS.Resolvables
{
    /// <summary>
    /// Represents something that is resolvable
    /// </summary>
    /// <typeparam name="T">The type this resolvable resolves to</typeparam>
    public interface IResolvable<T>
    {
        /// <summary>
        /// Resolves this resolvable using the given args.
        /// </summary>
        /// <param name="args">The args to resolve this</param>
        /// <returns>The resolved object. May throw a potential exception if the args are not the correct type</returns>
        T Resolve(params object[] args);
    }

    /// <summary>
    /// Represents something that is resolvable
    /// </summary>
    /// <typeparam name="T">The type this resolvable resolves to</typeparam>
    /// <typeparam name="P1">The first parameter type of the resolve method</typeparam>
    public interface IResolvable<P1, T>
    {
        /// <summary>
        /// Resolves this resolvable using the given args.
        /// </summary>
        /// <param name="arg1">The first argument</param>
        /// <returns>The resolved object</returns>
        T Resolve(P1 arg1);
    }
}