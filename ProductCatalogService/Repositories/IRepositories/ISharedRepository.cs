namespace ProductCatalogService.Repositories.IRepositories
{
    public interface ISharedRepository
    {
        Task<bool> SaveAllChange();
    }
}
