namespace AuthService.Entities
{
    public class PermissionRole
    {
        public Guid PermissionId { get; set; }
        public string RoleId { get; set; }

        public ApplicationRole Role { get; set; }
        public Permission Permission { get; set; }
    }
}
