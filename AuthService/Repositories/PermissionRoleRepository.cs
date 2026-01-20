using AuthService.Data;
using AuthService.Entities;
using AuthService.Repositories.IRepositories;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Common.Enums;
using Common.Helper;
using Common.Helper.EntityParams;
using Contracts.DTOs.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AuthService.Repositories
{
    public class PermissionRoleRepository : IPermissionRoleRepository
    {
        private readonly IDbContextFactory<AuthDbContext> _dbContextFactory;
        private readonly AuthDbContext _context;
        private readonly IMapper _mapper;

        public PermissionRoleRepository(IDbContextFactory<AuthDbContext> dbContextFactory, AuthDbContext context, IMapper mapper)
        {
            _dbContextFactory = dbContextFactory;
            _context = context;
            _mapper = mapper;
        }

        public void Add(PermissionRole permissionRole)
        {
            _context.PermissionRoles.Add(permissionRole);
        }

        public void Delete(PermissionRole permissionRole)
        {
            _context.PermissionRoles.Remove(permissionRole);
        }

        public async Task<PagedList<PermissionRoleDto>> GetAll(PermissionRoleParams permissionRoleParams)
        {
            var query = _context.PermissionRoles.AsQueryable();

            return await PagedList<PermissionRoleDto>.CreateAsync(
                query.ProjectTo<PermissionRoleDto>(_mapper.ConfigurationProvider),
                permissionRoleParams.PageNumber,
                permissionRoleParams.PageSize);
        }

        public async Task<List<PermissionRole>> GetAll()
        {
            return await _context.PermissionRoles.ToListAsync();
        }

        public async Task<PermissionRole> GetById(Guid permissionId, string roleId)
        {
            return (await _context.PermissionRoles.FindAsync(permissionId, roleId))!;
        }

        public async Task<List<Guid>> GetPermissionIdsByRoleId(string roleId)
        {
            return await _context.PermissionRoles
                .Where(pr => pr.RoleId == roleId)
                .Select(pr => pr.PermissionId)
                .ToListAsync();
        }

        public async Task<List<string>> GetRoleIdsByPermissionId(Guid permissionId)
        {
            return await _context.PermissionRoles
                .Where(pr => pr.PermissionId == permissionId)
                .Select(pr => pr.RoleId)
                .ToListAsync();
        }

        public void Update(PermissionRole permissionRole)
        {
            _context.PermissionRoles.Update(permissionRole);
        }
    }
}
