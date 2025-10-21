namespace RelationshipService.Repositories.IRepositories
{
    public interface ISharedRepository
    {
        Task<bool> SaveAllChanges();
    }
}
