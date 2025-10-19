namespace BusinessService.Entities
{
    public class StoreCategory
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; }
        public string? Description { get; set; }

        public List<StoreCategoryRelation> StoreCategoryRelations { get; set; } = new();
    }
}
