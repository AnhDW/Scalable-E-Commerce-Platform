using AuthService.Entities;
using Common.Helper;
using Common.Helper.EntityParams;
using Contracts.DTOs.Auth;

namespace AuthService.Repositories.IRepositories
{
    public interface IRoleRepository
    {
        Task<PagedList<RoleDto>> GetAll(RoleParams roleParams);
        Task<List<ApplicationRole>> GetAll();
        Task<ApplicationRole> GetById(string id);
        void Add(ApplicationRole applicationRole);
        void Update(ApplicationRole applicationRole);
        void Delete(ApplicationRole applicationRole);

        Task<bool> CheckExists(string roleName);
    }
}
