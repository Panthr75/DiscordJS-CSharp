namespace DiscordJS
{
    /// <summary>
    /// Options for splitting a message.
    /// </summary>
    public class SplitOptions
    {
        /// <summary>
        /// Maximum character length per message piece
        /// <br/>
        /// <br/>
        /// <b>Default</b>: 2000
        /// </summary>
        public int MaxLength { get; set; }

        /// <summary>
        /// Character to split the message with
        /// <br/>
        /// <br/>
        /// <b>Default</b>: "\n"
        /// </summary>
        public string Char { get; set; }

        /// <summary>
        /// Text to prepend to every piece except the first
        /// <br/>
        /// <br/>
        /// <b>Default</b>: ""
        /// </summary>
        public string Prepend { get; set; }

        /// <summary>
        /// Text to append to every piece except the last
        /// <br/>
        /// <br/>
        /// <b>Default</b>: ""
        /// </summary>
        public string Append { get; set; }

        /// <summary>
        /// Instantiates new SplitOptions with the given parameters.
        /// </summary>
        public SplitOptions()
        {
            MaxLength = 2000;
            Char = "\n";
            Prepend = "";
            Append = "";
        }

        /// <summary>
        /// Instantiates new SplitOptions with the given parameters.
        /// </summary>
        /// <param name="maxLength">Maximum character length per message piece</param>
        public SplitOptions(int maxLength)
        {
            MaxLength = maxLength;
            Char = "\n";
            Prepend = "";
            Append = "";
        }

        /// <summary>
        /// Instantiates new SplitOptions with the given parameters.
        /// </summary>
        /// <param name="maxLength">Maximum character length per message piece</param>
        /// <param name="character">Character to split the message with</param>
        public SplitOptions(int maxLength, string character)
        {
            MaxLength = maxLength;
            Char = character;
            Prepend = "";
            Append = "";
        }

        /// <summary>
        /// Instantiates new SplitOptions with the given parameters.
        /// </summary>
        /// <param name="maxLength">Maximum character length per message piece</param>
        /// <param name="character">Character to split the message with</param>
        /// <param name="prepend">Text to prepend to every piece except the first</param>
        public SplitOptions(int maxLength, string character, string prepend)
        {
            MaxLength = maxLength;
            Char = character;
            Prepend = prepend;
            Append = "";
        }

        /// <summary>
        /// Instantiates new SplitOptions with the given parameters.
        /// </summary>
        /// <param name="maxLength">Maximum character length per message piece</param>
        /// <param name="character">Character to split the message with</param>
        /// <param name="prepend">Text to prepend to every piece except the first</param>
        /// <param name="append">Text to append to every piece except the last</param>
        public SplitOptions(int maxLength, string character, string prepend, string append)
        {
            MaxLength = maxLength;
            Char = character;
            Prepend = prepend;
            Append = append;
        }
    }
}