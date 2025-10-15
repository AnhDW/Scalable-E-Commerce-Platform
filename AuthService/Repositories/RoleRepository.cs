using AuthService.Data;
using AuthService.Entities;
using AuthService.Repositories.IRepositories;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Common.Helper;
using Common.Helper.EntityParams;
using Contracts.DTOs.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AuthService.Repositories
{
    public class RoleRepository : IRoleRepository
    {
        private readonly IDbContextFactory<AuthDbContext> _dbContextFactory;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly AuthDbContext _context;
        private readonly IMapper _mapper;

        public RoleRepository(IDbContextFactory<AuthDbContext> dbContextFactory, AuthDbContext context, IMapper mapper, RoleManager<ApplicationRole> roleManager)
        {
            _dbContextFactory = dbContextFactory;
            _context = context;
            _mapper = mapper;
            _roleManager = roleManager;
        }

        public void Add(ApplicationRole applicationRole)
        {
            _context.Roles.Add(applicationRole);
        }

        public async Task<bool> CheckExists(string roleName)
        {
            return await _roleManager.RoleExistsAsync(roleName);
        }

        public void Delete(ApplicationRole applicationRole)
        {
            _context.Roles.Remove(applicationRole);
        }

        public async Task<PagedList<RoleDto>> GetAll(RoleParams roleParams)
        {
            var query = _context.Roles.AsQueryable();

            return await PagedList<RoleDto>.CreateAsync(
                query.AsNoTracking().ProjectTo<RoleDto>(_mapper.ConfigurationProvider),
                roleParams.PageNumber,
                roleParams.PageSize);
        }

        public async Task<List<ApplicationRole>> GetAll()
        {
            return await _context.Roles.ToListAsync();
        }

        public async Task<ApplicationRole> GetById(string id)
        {
            return (await _context.Roles.FindAsync(id))!;
        }

        public void Update(ApplicationRole applicationRole)
        {
            _context.Roles.Update(applicationRole);
        }
    }
}
