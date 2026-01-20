using AuthService.Entities;
using Common.Enums;
using Common.Helper;
using Common.Helper.EntityParams;
using Contracts.DTOs.Auth;
using Contracts.DTOs.Relationship;

namespace AuthService.Repositories.IRepositories
{
    public interface IPermissionRoleRepository
    {
        Task<PagedList<PermissionRoleDto>> GetAll(PermissionRoleParams permissionRoleParams);
        Task<List<PermissionRole>> GetAll();
        Task<List<string>> GetRoleIdsByPermissionId(Guid permissionId);
        Task<List<Guid>> GetPermissionIdsByRoleId(string roleId);
        Task<PermissionRole> GetById(Guid permissionId, string roleId);
        void Add(PermissionRole permissionRole);
        void Update(PermissionRole permissionRole);
        void Delete(PermissionRole permissionRole);
    }
}
