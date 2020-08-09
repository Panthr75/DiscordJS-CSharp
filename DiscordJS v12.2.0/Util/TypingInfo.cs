using JavaScript;
using System;

namespace DiscordJS
{
    public class TypingInfo
    {
        public int count;
        public int interval;
        public Action resolve;
        public IPromise promise;
    }
}