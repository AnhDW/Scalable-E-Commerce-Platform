namespace AuthService.Entities
{
    public class PermissionRole
    {
        public string RoleId { get; set; }
        public Guid PermissionId { get; set; }

        public ApplicationRole Role { get; set; }
        public Permission Permission { get; set; }
    }
}
