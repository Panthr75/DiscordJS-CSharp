using System;

namespace DiscordJS
{
    /// <summary>
    /// An entry in the audit log representing a specific change.
    /// </summary>
    public interface IGuildAuditLogChange
    {
        /// <summary>
        /// The property that was changed, e.g. <c>nick</c> for nickname changes
        /// </summary>
        string Key { get; set; }

        /// <summary>
        /// The old value of the change, e.g. for nicknames, the old nickname
        /// </summary>
        object Old { get; set; }

        /// <summary>
        /// The new value of the change, e.g. for nicknames, the new nickname
        /// </summary>
        object New { get; set; }
    }

    /// <summary>
    /// An entry in the audit log representing a specific change.
    /// </summary>
    /// <typeparam name="T">The type of the old/new values</typeparam>
    public interface IGuildAuditLogChange<T>
    {
        /// <summary>
        /// The old value of the change, e.g. for nicknames, the old nickname
        /// </summary>
        T Old { get; set; }

        /// <summary>
        /// The new value of the change, e.g. for nicknames, the new nickname
        /// </summary>
        T New { get; set; }
    }

    /// <summary>
    /// An entry in the audit log representing a specific change.
    /// </summary>
    public class GuildAuditLogChange : IGuildAuditLogChange
    {
        /// <inheritdoc/>
        public string Key { get; set; }
        /// <inheritdoc/>
        public object Old { get; set; }
        /// <inheritdoc/>
        public object New { get; set; }
    }

    /// <summary>
    /// An entry in the audit log representing a specific change.
    /// </summary>
    /// <typeparam name="T">The type of the old and new values</typeparam>
    public class GuildAuditLogChange<T> : IGuildAuditLogChange<T>, IGuildAuditLogChange
    {
        /// <inheritdoc/>
        public string Key { get; set; }
        /// <inheritdoc/>
        public T Old { get; set; }
        /// <inheritdoc/>
        public T New { get; set; }

        private bool IsCompatibleObject(object value) => (value is T) || (value == null && default(T) == null);
        object IGuildAuditLogChange.Old
        {
            get => Old;
            set
            {
                if (IsCompatibleObject(value))
                    Old = (T)value;
                else
                    throw new ArgumentException("Invalid argument type given. Expected '" + typeof(T).Name + "'");
            }
        }
        object IGuildAuditLogChange.New
        {
            get => New;
            set
            {
                if (IsCompatibleObject(value))
                    New = (T)value;
                else
                    throw new ArgumentException("Invalid argument type given. Expected '" + typeof(T).Name + "'");
            }
        }
    }
}