namespace AuthRepositories.Repositories.IRepositories
{
    public interface ISharedRepository
    {
        Task<bool> SaveAllChanges();
    }
}
