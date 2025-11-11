namespace OrderService.Repositories.IRepositories
{
    public interface ISharedRepository
    {
        Task<bool> SaveAllChange();
    }
}
