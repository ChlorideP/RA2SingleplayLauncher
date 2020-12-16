using System;

namespace BDLauncherCSharp.Exceptions
{

    [Serializable]
    public class SpawnerInvalidException : Exception
    {
        public SpawnerInvalidException() { }
        public SpawnerInvalidException(string message) : base(message) { }
        public SpawnerInvalidException(string message, Exception inner) : base(message, inner) { }
        protected SpawnerInvalidException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
