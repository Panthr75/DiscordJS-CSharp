﻿using System;

namespace JavaScript.PromiseExceptions
{
    /// <summary>
    /// Base class for promise exceptions.
    /// </summary>
    public class PromiseException : Exception
    {
        public PromiseException() { }

        public PromiseException(string message) : base(message) { }

        public PromiseException(string message, Exception inner) : base(message, inner) { }
    }
}