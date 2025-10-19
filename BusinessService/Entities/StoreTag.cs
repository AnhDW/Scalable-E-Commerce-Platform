namespace BusinessService.Entities
{
    public class StoreTag
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; }
        public string? Description { get; set; }

        public List<StoreTagRelation> StoreTagRelations { get; set; } = new();
    }
}
