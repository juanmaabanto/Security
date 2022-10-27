using N5.Challenge.Services.Security.Domain.SeedWork;

namespace N5.Challenge.Services.Security.Domain.Entities
{
    public class Permission : Entity
    {
        #region Properties

        public int PermissionTypeId { get; private set; }
        public string EmployeeForename { get; private set; }
        public string EmployeeSurname { get; private set; }
        public DateTime PermissionDate { get; private set; }
        public PermissionType? PermissionType { get; private set; }

        #endregion

        public Permission(int permissionTypeId, string employeeForename, string employeeSurname)
        {
            PermissionTypeId = permissionTypeId;
            EmployeeForename = employeeForename.Trim();
            EmployeeSurname = employeeSurname.Trim();
            PermissionDate = DateTime.Now;
        }

        public void Modify(int permissionTypeId, string employeeForename, string employeeSurname)
        {
            PermissionTypeId = permissionTypeId;
            EmployeeForename = employeeForename.Trim();
            EmployeeSurname = employeeSurname.Trim();
        }
    }
}