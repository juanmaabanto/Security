using N5.Challenge.Services.Security.Domain.SeedWork;

namespace N5.Challenge.Services.Security.Domain.Entities
{
    public class PermissionType : Entity
    {
        #region Properties

        public string Description { get; private set; }

        #endregion

        public PermissionType(string description)
        {
            Description = description;
        }
    }
}