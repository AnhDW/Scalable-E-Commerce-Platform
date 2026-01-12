using AuthService.Entities;
using Common.Helper;
using Common.Helper.EntityParams;
using Contracts.DTOs.Auth;
using Microsoft.AspNetCore.Identity;

namespace AuthService.Repositories.IRepositories
{
    public interface IUserRoleRepository
    {
        Task<PagedList<UserRoleDto>> GetAll(UserRoleParams userRoleParams);
        Task<List<ApplicationUserRole>> GetAll();
        Task<List<string>> GetRoleIdsByUserId(string userId);
        Task<List<string>> GetUserIdsByRoleId(string roleId);
        Task<ApplicationUserRole> GetById(string userId, string roleId);
        void Add(ApplicationUserRole userRole);
        void Update(ApplicationUserRole userRole);
        void Delete(ApplicationUserRole userRole);
    }
}
