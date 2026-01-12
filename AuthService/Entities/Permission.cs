namespace AuthService.Entities
{
    public class Permission
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; }
        public string Action { get; set; }
        public string Code { get; set; }

        public List<PermissionRole> PermissionRoles { get; set; } = new();
    }
}
