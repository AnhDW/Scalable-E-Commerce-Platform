namespace ShoppingCartService.Repositories.IRepositories
{
    public interface ISharedRepository
    {
        Task<bool> SaveAllChange();
    }
}
