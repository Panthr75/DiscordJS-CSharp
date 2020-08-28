using System;

namespace DiscordJS.Resolvables
{
    /// <summary>
    /// Data that can be resolved intro a VoiceState. This can be:
    /// <list type="bullet">
    /// <item>A VoiceState</item>
    /// <item>A Snowflake</item>
    /// </list>
    /// </summary>
    public class VoiceStateResolvable
    {
        internal enum Type
        {
            VoiceState,
            Snowflake
        }

        internal Type type;

        internal Snowflake snowflake;
        internal VoiceState voiceState;

        internal VoiceState Resolve(VoiceStateManager manager)
        {
            if (type == Type.VoiceState)
                return voiceState;
            else if (type == Type.Snowflake)
                return manager.Cache.Get(snowflake);
            else
                throw new ArgumentException("The given type can't be cast to this resolvable");
        }

        internal Snowflake ResolveID()
        {
            if (type == Type.VoiceState)
                return voiceState.ID;
            else if (type == Type.Snowflake)
                return snowflake;
            else
                throw new ArgumentException("The given type can't be cast to this resolvable");
        }

        public VoiceStateResolvable(Snowflake snowflake)
        {
            type = Type.Snowflake;
            this.snowflake = snowflake;
        }

        public VoiceStateResolvable(JavaScript.String snowflake) : this((Snowflake)snowflake.ToString())
        { }

        public VoiceStateResolvable(string snowflake) : this((Snowflake)snowflake)
        { }

        public VoiceStateResolvable(VoiceState voiceState)
        {
            type = Type.VoiceState;
            this.voiceState = voiceState;
        }

        public static implicit operator VoiceStateResolvable(Snowflake snowflake) => new VoiceStateResolvable(snowflake);
        public static implicit operator VoiceStateResolvable(JavaScript.String snowflake) => new VoiceStateResolvable(snowflake);
        public static implicit operator VoiceStateResolvable(string snowflake) => new VoiceStateResolvable(snowflake);
        public static implicit operator VoiceStateResolvable(VoiceState voiceState) => new VoiceStateResolvable(voiceState);
    }
}