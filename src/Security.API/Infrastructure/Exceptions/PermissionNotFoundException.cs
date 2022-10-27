using System;
using System.Runtime.Serialization;

namespace N5.Challenge.Services.Security.API.Infrastructure.Exceptions
{
    [Serializable]
    public sealed class PermissionNotFoundException : NotFoundException
    {
        public PermissionNotFoundException(int permissionId)
            : base($"The permission with the identifier {permissionId} was not found.")
        { }

        private PermissionNotFoundException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        { }
    }
}