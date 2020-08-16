using DiscordJS.Data;

namespace DiscordJS.Actions
{
    public class UserUpdateAction : GenericAction<UserUpdateAction.ActionResult>
    {
        public class ActionResult
        {
            public User Old { get; }
            public User Updated { get; }

            internal ActionResult(User old, User updated)
            {
                Old = old;
                Updated = updated;
            }
        }

        public UserUpdateAction(Client client) : base(client)
        { }

        public override ActionResult Handle(dynamic data) => Handle(DiscordUtil.FromDynamic<UserData>(data));

        public ActionResult Handle(UserData data)
        {
            User newUser = Client.Users.Cache.Get(data.id);
            User oldUser = newUser._Update(data);

            if (!oldUser.Equals(newUser))
            {
                Client.EmitUserUpdate(oldUser, newUser);

                return new ActionResult(oldUser, newUser);
            }

            return new ActionResult(null, null);
        }
    }
}
