namespace BusinessService.Repositories.IRepositories
{
    public interface ISharedRepository
    {
        Task<bool> SaveAllChanges();
    }
}
