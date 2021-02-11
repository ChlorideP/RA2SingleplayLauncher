using System;

namespace BattleLauncher.Exceptions
{
    [Serializable]
    public class RendererException : Exception
    {
        public RendererException() { }
        public RendererException(string message) : base(message) { }
        public RendererException(string message, Exception inner) : base(message, inner) { }
        protected RendererException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
