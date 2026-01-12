using AuthService.Entities;
using Common.Helper;
using Common.Helper.EntityParams;
using Contracts.DTOs.Auth;

namespace AuthService.Repositories.IRepositories
{
    public interface IPermissionRepository
    {
        Task<PagedList<PermissionDto>> GetAll(PermissionParams PermissionParams);
        Task<List<Permission>> GetAll();
        Task<Permission> GetById(Guid id);
        void Add(Permission Permission);
        void Update(Permission Permission);
        void Delete(Permission Permission);

        Task<bool> CheckExists(string code);
    }
}
