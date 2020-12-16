using System;
using System.IO;

namespace BDLauncherCSharp.Exceptions
{
    [Serializable]
    public class AresNotFoundException : FileNotFoundException
    {
        public AresNotFoundException() { }
        public AresNotFoundException(string message) : base(message) { }
        public AresNotFoundException(string message, Exception inner) : base(message, inner) { }
        protected AresNotFoundException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
