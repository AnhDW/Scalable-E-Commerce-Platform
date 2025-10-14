using AuthService.Entities;
using Common.Helper;
using Common.Helper.EntityParams;
using Contracts.DTOs.Auth;

namespace AuthService.Services.IServices
{
    public interface IUserService
    {
        Task<PagedList<UserDto>> GetAll(UserParams userParams);
        Task<List<ApplicationUser>> GetAll();
        Task<List<ApplicationUser>> GetByUserIds(List<string> userIds);
        Task<List<ApplicationUser>> GetByRoleName(string roleName);
        Task<List<ApplicationUser>> GetByFullName(string fullName);
        Task<ApplicationUser> GetById(string id);
        Task<ApplicationUser> GetByCode(string code);
        void Add(ApplicationUser user);
        void Update(ApplicationUser user);
        void Delete(ApplicationUser user);
    }
}
