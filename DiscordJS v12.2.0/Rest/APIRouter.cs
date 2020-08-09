using JavaScript;
using DiscordJS.Data;
using Newtonsoft.Json;

namespace DiscordJS.Rest
{
    public class APIRouter
    {
        public class TypingRouter
        {
            internal TypingRouter(ChannelsRouter router)
            {
                //
            }

            public IPromise<object> Post()
            {
                //
            }
        }

        public class ReactionsRouter
        {
            internal ReactionsRouter(MessagesRouter router, string emoji, string user)
            {
                //
            }

            public IPromise<object> Put()
            {
                //
            }

            public IPromise<object> Delete()
            {
                //
            }

            public IPromise<object> Delete(string userID)
            {
                //
            }
        }

        public class BulkDeleteRouter
        {
            internal BulkDeleteRouter(MessagesRouter router)
            {
                //
            }

            public IPromise<object> Post(dynamic data)
            {
                string dataToSend = JsonConvert.SerializeObject(data.data);
            }
        }

        public class MessagesRouter
        {
            internal MessagesRouter(ChannelsRouter router, string id)
            {
                //
            }

            /// <summary>
            /// Gets the messages for a channel
            /// </summary>
            /// <returns></returns>
            public IPromise<MessageData[]> Get(dynamic data)
            {
                string dataToSend = JsonConvert.SerializeObject(data.data);
            }

            /// <summary>
            /// Gets the messages for a channel
            /// </summary>
            /// <returns></returns>
            public IPromise<MessageData[]> Get()
            {
            }

            public IPromise<MessageData> Post(dynamic data)
            {
                string dataToSend = JsonConvert.SerializeObject(data.data);
            }

            public IPromise<MessageData> Patch(dynamic data)
            {
                string dataToSend = JsonConvert.SerializeObject(data.data);
            }

            public IPromise<object> Delete(dynamic data)
            {
                //
            }

            public ReactionsRouter Reactions(string emoji, string user) => new ReactionsRouter(this, emoji, user);
            public BulkDeleteRouter BulkDelete() => new BulkDeleteRouter(this);
        }

        public class ChannelsRouter
        {
            internal string url;
            internal ChannelsRouter(string id)
            {
                //
            }

            public PermissionsRouter Permissions(string id) => new PermissionsRouter(this, id);
            public MessagesRouter Messages(string id = null) => new MessagesRouter(this, id);

            /// <summary>
            /// Get a channel by ID
            /// </summary>
            /// <returns></returns>
            public IPromise<ChannelData> Get()
            {
                //
            }

            /// <summary>
            /// Update a channel's settings
            /// </summary>
            /// <param name="data"></param>
            /// <returns></returns>
            public IPromise<ChannelData> Patch(dynamic data)
            {
                string dataToSend = JsonConvert.SerializeObject(data.data);
            }

            /// <summary>
            /// Delete a channel, or close a private message
            /// </summary>
            /// <returns></returns>
            public IPromise<ChannelData> Delete(dynamic data)
            {
                //
            }

            public IPromise<ChannelData> Post(dynamic options)
            {
                //
            }

            public TypingRouter Typing => new TypingRouter(this);
            public PinsRouter Pins(string id) => new PinsRouter(this, id);
            public PinsRouter Pins() => new PinsRouter(this, null);
            public RecipientRouter Recipient(string id) => new RecipientRouter(this, id);
            public InvitesRouter Invites => new InvitesRouter(this);
        }

        public class InvitesRouter
        {
            internal InvitesRouter(ChannelsRouter router)
            {
                //
            }

            public IPromise<InviteData[]> Get()
            {
                //
            }

            public IPromise<InviteData> Post(dynamic data)
            {
                //
            }
        }

        public class PermissionsRouter
        {
            internal PermissionsRouter(ChannelsRouter router, string id)
            {
                //
            }

            public IPromise<object> Put(dynamic data)
            {
                //
            }

            public IPromise<object> Delete()
            {
                //
            }
        }

        public class GuildsRouter
        {
            internal GuildsRouter(string id)
            {
                //
            }

            public GuildEmojisRouter Emojis(string id) => new GuildEmojisRouter(this, id);
            public GuildMembersRouter Members(string id) => new GuildMembersRouter(this, id);
        }

        public class GuildEmojisRouter
        {
            internal GuildEmojisRouter(GuildsRouter router, string id)
            {
                //
            }

            /// <summary>
            /// Gets the full emoji data
            /// </summary>
            /// <returns></returns>
            public IPromise<EmojiData> Get()
            {
                //
            }
        }

        public class GuildMembersRouter
        {
            internal GuildMembersRouter(GuildsRouter router, string id)
            {
                //
            }

            /// <summary>
            /// Gets the full guild member data
            /// </summary>
            /// <returns></returns>
            public IPromise<GuildMemberData> Get()
            {
                //
            }
        }

        public class RecipientRouter
        {
            internal RecipientRouter(ChannelsRouter router, string id)
            {

            }

            public IPromise<UserData> Put(dynamic data)
            {
                //
            }

            public IPromise<UserData> Delete()
            {
                //
            }
        }

        public class PinsRouter
        {
            internal PinsRouter(ChannelsRouter router, string id)
            {
                //
            }

            public IPromise<MessageData[]> Get()
            {
                //
            }

            public IPromise<object> Put()
            {
                //
            }

            public IPromise<object> Delete()
            {
                //
            }
        }

        public class UsersRouter
        {
            private readonly string id;
            internal UsersRouter(string id)
            {
                this.id = id;
            }

            public ChannelsRouter Channels => new ChannelsRouter(id);

            /// <summary>
            /// Gets the full user data
            /// </summary>
            /// <returns></returns>
            public IPromise<UserData> Get()
            {
                //
            }
        }

        internal APIRouter(RESTManager manager)
        {
            //
        }

        public ChannelsRouter Channels(string id) => new ChannelsRouter(id);
        public GuildsRouter Guilds(string id) => new GuildsRouter(id);
        public UsersRouter Users(string id) => new UsersRouter(id);
    }
}