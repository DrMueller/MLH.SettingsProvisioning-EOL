using System;
using System.Runtime.Serialization;

namespace Mmu.Mlh.SettingsProvisioning.Infrastructure.Exceptions
{
    [Serializable]
    public class AppSettingsNotFoundException : Exception
    {
        public AppSettingsNotFoundException()
        {
        }

        public AppSettingsNotFoundException(string message)
            : base(message)
        {
        }

        public AppSettingsNotFoundException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected AppSettingsNotFoundException(SerializationInfo serializationInfo, StreamingContext streamingContext)
            : base(serializationInfo, streamingContext)
        {
        }
    }
}