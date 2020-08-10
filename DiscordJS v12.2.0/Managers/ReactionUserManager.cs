using DiscordJS.Data;
using DiscordJS.Resolvables;
using JavaScript;
using System.Collections.Generic;

namespace DiscordJS
{
    /// <summary>
    /// Manages API methods for users who reacted to a reaction and stores their cache.
    /// </summary>
    public class ReactionUserManager : BaseManager<Collection<Snowflake, User>, User, UserData>
    {
        /// <summary>
        /// The reaction that this manager belongs to
        /// </summary>
        public MessageReaction Reaction { get; internal set; }

        /// <inheritdoc/>
        public ReactionUserManager(Client client, IEnumerable<User> iterable, MessageReaction reaction) : base(Client, iterable)
        {
            Reaction = reaction;
        }

        /// <inheritdoc/>
        protected override User AddItem(User item)
        {
            Cache.Set(item.ID, item);
            return item;
        }

        /// <summary>
        /// Fetches all the users that gave this reaction. Resolves with a collection of users, mapped by their IDs.
        /// </summary>
        /// <param name="limit">The maximum amount of users to fetch, defaults to 100</param>
        /// <param name="before">Limit fetching users to those with an id lower than the supplied id</param>
        /// <param name="after">Limit fetching users to those with an id greater than the supplied id</param>
        /// <returns></returns>
        public IPromise<Collection<Snowflake, User>> Fetch(int limit = 100, Snowflake before = null, Snowflake after = null)
        {
            var message = Reaction.Message;
            return Client.API.Channels(message.Channel.ID).Messages(message.ID).Reactions(Reaction.Emoji.Identifier, null).Get(new
            {
                query = new
                {
                    before = before == null ? null : before.ToString(),
                    after = after == null ? null : after.ToString(),
                    limit
                }
            }).Then((UserData[] data) =>
            {
                var users = new Collection<Snowflake, User>();
                for (int index = 0, length = data.Length; index < length; index++)
                {
                    var user = Client.Users.Add(data[index]);
                    Cache.Set(user.ID, user);
                    users.Set(user.ID, user);
                }
                return users;
            });
        }

        /// <summary>
        /// Removes a user from this reaction.
        /// </summary>
        /// <returns></returns>
        public IPromise<MessageReaction> Remove() => Remove(Reaction.Message.Client.User);

        /// <summary>
        /// Removes a user from this reaction.
        /// </summary>
        /// <param name="user">The user to remove the reaction of</param>
        /// <returns></returns>
        public IPromise<MessageReaction> Remove(UserResolvable user)
        {
            var message = Reaction.Message;
            var userID = message.Client.Users.ResolveID(user);
            if (userID == null) return Promise<MessageReaction>.Rejected(new DJSError.Error("REACTION_RESOLVE_USER"));
            return message.Client.API.Channels(message.Channel.ID).Messages(message.ID).Reactions(Reaction.Emoji.Identifier, userID == message.Client.User.ID ? "@me" : userID.ToString()).Delete().Then((_) => Reaction);
        }
    }
}