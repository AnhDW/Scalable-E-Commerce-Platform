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
    public class PermissionRepository : IPermissionRepository
    {
        private readonly IDbContextFactory<AuthDbContext> _dbContextFactory;
        private readonly AuthDbContext _context;
        private readonly IMapper _mapper;

        public PermissionRepository(IDbContextFactory<AuthDbContext> dbContextFactory, AuthDbContext context, IMapper mapper)
        {
            _dbContextFactory = dbContextFactory;
            _context = context;
            _mapper = mapper;
        }

        public void Add(Permission Permission)
        {
            _context.Permissions.Add(Permission);
        }

        public Task<bool> CheckExists(string code)
        {
            return _context.Permissions.AnyAsync(p => p.Code == code);
        }

        public void Delete(Permission Permission)
        {
            _context.Permissions.Remove(Permission);
        }

        public Task<PagedList<PermissionDto>> GetAll(PermissionParams PermissionParams)
        {
            var query = _context.Permissions.AsQueryable();

            return PagedList<PermissionDto>.CreateAsync(
                query.AsNoTracking().ProjectTo<PermissionDto>(_mapper.ConfigurationProvider),
                PermissionParams.PageNumber,
                PermissionParams.PageSize);
        }

        public Task<List<Permission>> GetAll()
        {
            return _context.Permissions.ToListAsync();
        }

        public async Task<Permission> GetById(Guid id)
        {
            return (await _context.Permissions.FindAsync(id))!;
        }

        public void Update(Permission Permission)
        {
            _context.Permissions.Update(Permission);    
        }
    }
}
