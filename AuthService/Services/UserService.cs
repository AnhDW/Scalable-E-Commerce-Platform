using AuthService.Data;
using AuthService.Entities;
using AuthService.Services.IServices;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Common.Helper;
using Common.Helper.EntityParams;
using Contracts.DTOs.Auth;
using Microsoft.EntityFrameworkCore;
using System;

namespace AuthService.Services
{
    public class UserService : IUserService
    {
        private readonly IDbContextFactory<AuthDbContext> _dbContextFactory;
        private readonly AuthDbContext _context;
        private readonly IMapper _mapper;

        public UserService(IDbContextFactory<AuthDbContext> dbContextFactory, AuthDbContext context, IMapper mapper)
        { 
            _dbContextFactory = dbContextFactory;
            _context = context;
            _mapper = mapper;
        }
        public void Add(ApplicationUser user)
        {
            _context.Users.Add(user);
        }

        public void Delete(ApplicationUser user)
        {
            _context.Users.Remove(user);
        }

        public async Task<PagedList<UserDto>> GetAll(UserParams userParams)
        {
            var query = _context.Users.AsQueryable();

            if (userParams.UserCode != null)
            {
                query = query.Where(x => x.Code.Contains(userParams.UserCode));
            }

            if (userParams.FullName != null)
            {
                query = query.Where(x => x.FullName.Contains(userParams.FullName));
            }

            return await PagedList<UserDto>.CreateAsync(
                query.AsNoTracking().ProjectTo<UserDto>(_mapper.ConfigurationProvider),
                userParams.PageNumber,
                userParams.PageSize);
        }

        public async Task<List<ApplicationUser>> GetAll()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<ApplicationUser> GetByCode(string code)
        {
            return (await _context.Users.FirstOrDefaultAsync(x => x.Code == code))!;
        }

        public async Task<List<ApplicationUser>> GetByFullName(string fullName)
        {
            return await _context.Users.Where(x => x.FullName == fullName).ToListAsync();
        }

        public async Task<ApplicationUser> GetById(string id)
        {
            return (await _context.Users.FirstOrDefaultAsync(x => x.Id == id))!;
        }

        public Task<List<ApplicationUser>> GetByRoleName(string roleName)
        {
            throw new NotImplementedException();
        }

        public async Task<List<ApplicationUser>> GetByUserIds(List<string> userIds)
        {
            return await _context.Users.Where(x=>userIds.Contains(x.Id)).ToListAsync();
        }

        public void Update(ApplicationUser user)
        {
            _context.Users.Update(user);
        }
    }
}
