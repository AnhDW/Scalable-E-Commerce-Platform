using AuthService.Data;
using AuthService.Entities;
using AuthService.Repositories.IRepositories;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Common.Helper;
using Common.Helper.EntityParams;
using Contracts.DTOs.Auth;
using Microsoft.EntityFrameworkCore;

namespace AuthService.Repositories
{
    public class UserRoleRepository : IUserRoleRepository
    {
        private readonly IDbContextFactory<AuthDbContext> _dbContextFactory;
        private readonly AuthDbContext _context;
        private readonly IMapper _mapper;

        public UserRoleRepository(IDbContextFactory<AuthDbContext> dbContextFactory, AuthDbContext context, IMapper mapper)
        {
            _dbContextFactory = dbContextFactory;
            _context = context;
            _mapper = mapper;
        }

        public void Add(ApplicationUserRole userRole)
        {
            _context.UserRoles.Add(userRole);
        }

        public void Delete(ApplicationUserRole userRole)
        {
            _context.UserRoles.Remove(userRole);
        }

        public async Task<PagedList<UserRoleDto>> GetAll(UserRoleParams userRoleParams)
        {
            var query = _context.UserRoles.AsQueryable();
            return await PagedList<UserRoleDto>.CreateAsync(
                query.AsNoTracking().ProjectTo<UserRoleDto>(_mapper.ConfigurationProvider),
                userRoleParams.PageNumber,
                userRoleParams.PageSize);
        }

        public async Task<List<ApplicationUserRole>> GetAll()
        {
            return await _context.UserRoles.ToListAsync();
        }

        public async Task<ApplicationUserRole> GetById(string userId, string roleId)
        {
            return (await _context.UserRoles.FindAsync(userId, roleId))!;
        }

        public async Task<List<string>> GetRoleIdsByUserId(string userId)
        {
            return (await _context.UserRoles.Where(x=>x.UserId == userId).ToListAsync())
                .Select(x=>x.RoleId)
                .ToList();
        }

        public async Task<List<string>> GetUserIdsByRoleId(string roleId)
        {
            return (await _context.UserRoles.Where(x=>x.RoleId.ToString() == roleId).ToListAsync())
                .Select(x=>x.UserId)
                .ToList();
        }

        public void Update(ApplicationUserRole userRole)
        {
            _context.UserRoles.Update(userRole);
        }
    }
}
