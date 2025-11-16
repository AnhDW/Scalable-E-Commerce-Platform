namespace PaymentService.Repositories.IRepositories
{
    public interface ISharedRepository
    {
        Task<bool> SaveAllChange();
    }
}
