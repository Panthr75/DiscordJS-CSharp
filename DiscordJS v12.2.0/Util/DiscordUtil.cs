using DiscordJS.Resolvables;
using JavaScript;

namespace DiscordJS
{
    /// <summary>
    /// Contains various general-purpose utility methods. These functions are also available on the base <c>Discord</c> object.
    /// </summary>
    public static class DiscordUtil
    {
        /// <summary>
        /// Resolves a ColorResolvable into a color number.
        /// </summary>
        /// <param name="color">Color to resolve</param>
        /// <returns>A color</returns>
        public static int ResolveColor(ColorResolvable color)
        {
            if (color == null) throw new DJSError.Error("COLOR_CONVERT");
            else return color.Resolve();
        }

        /// <summary>
        /// Moves an element in an array <i>in place</i>.
        /// </summary>
        /// <typeparam name="T">The type of items in the array</typeparam>
        /// <param name="array">Array to modify</param>
        /// <param name="element">Element to move</param>
        /// <param name="newIndex">Index or offset to move the element to</param>
        /// <param name="offset">Move the element by an offset amount rather than to a set index</param>
        /// <returns></returns>
        public static int MoveElementInArray<T>(Array<T> array, T element, int newIndex, bool offset = false)
        {
            int index = array.IndexOf(element);
            newIndex = (offset ? index : 0) + newIndex;
            if (newIndex > -1 && newIndex < array.Length)
            {
                T removedItem = array.Splice(index, 1)[0];
                array.Splice(newIndex, 0, removedItem);
            }
            return array.IndexOf(element);
        }

        /// <summary>
        /// Sets the position of a Channel or Role.
        /// </summary>
        /// <typeparam name="T">The type of item</typeparam>
        /// <param name="item">Object to set the position of</param>
        /// <param name="position">New position for the object</param>
        /// <param name="relative">Whether <paramref name="position"/> is relative to its current position</param>
        /// <param name="sorted">A collection of the objects sorted properly</param>
        /// <param name="route">Route to call PATCH on</param>
        /// <param name="reason">Reason for the change</param>
        /// <returns>Updated item list, with <c>id</c> and <c>position</c> properties</returns>
        public static IPromise<Array<ItemWithPositionAndID>> SetPosition<T>(T item, int position, bool relative, Collection<Snowflake, T> sorted, dynamic route, string reason) where T : IHasID
        {
            Array<T> initialItems = sorted.Array();
            MoveElementInArray(initialItems, item, position, relative);
            Array<ItemWithPositionAndID> updatedItems = initialItems.Map((r, i) => new ItemWithPositionAndID(r.ID, i));
            IPromise promise = route.Patch(new { data = updatedItems.ToArray(), reason });
            return promise.Then(() => Promise<Array<ItemWithPositionAndID>>.Resolved(updatedItems));
        }
    }

    public class ItemWithPositionAndID
    {
        public readonly string id;
        public readonly int position;

        public ItemWithPositionAndID(Snowflake id, int position)
        {
            this.id = id;
            this.position = position;
        }
    }
}