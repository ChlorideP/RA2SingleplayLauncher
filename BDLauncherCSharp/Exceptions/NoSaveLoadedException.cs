using System;

namespace BDLauncherCSharp.Exceptions
{
    [Serializable]
    public class NoSaveLoadedException : Exception
    {
        public NoSaveLoadedException() { }
        public NoSaveLoadedException(string message) : base(message) { }
        public NoSaveLoadedException(string message, Exception inner) : base(message, inner) { }
        protected NoSaveLoadedException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
