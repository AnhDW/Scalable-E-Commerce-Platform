using AutoMapper;
using AutoMapper.QueryableExtensions;
using Common.Enums;
using Common.Helper;
using Common.Helper.EntityParams;
using Contracts.DTOs.Relationship;
using Microsoft.EntityFrameworkCore;
using RelationshipService.Data;
using RelationshipService.Entities;
using RelationshipService.Repositories.IRepositories;

namespace RelationshipService.Repositories
{
    public class UserStoreRelationRepository : IUserStoreRelationRepository
    {
        private readonly IDbContextFactory<RelationshipDbContext> _dbContextFactory;
        private readonly RelationshipDbContext _context;
        private readonly IMapper _mapper;

        public UserStoreRelationRepository(IDbContextFactory<RelationshipDbContext> dbContextFactory, RelationshipDbContext context, IMapper mapper)
        {
            _dbContextFactory = dbContextFactory;
            _context = context;
            _mapper = mapper;
        }

        public void Add(UserStoreRelation userStoreRelation)
        {
            _context.UserStoreRelations.Add(userStoreRelation);
        }

        public void Delete(UserStoreRelation userStoreRelation)
        {
            _context.UserStoreRelations.Remove(userStoreRelation);
        }

        public Task<PagedList<UserStoreRelationDto>> GetAll(UserStoreRelationParams userStoreRelationParams)
        {
            var query = _context.UserStoreRelations.AsQueryable();

            return PagedList<UserStoreRelationDto>.CreateAsync(
                query.AsNoTracking().ProjectTo<UserStoreRelationDto>(_mapper.ConfigurationProvider),
                userStoreRelationParams.PageNumber,
                userStoreRelationParams.PageSize);
        }

        public async Task<List<UserStoreRelation>> GetAll()
        {
            return await _context.UserStoreRelations.ToListAsync();
        }

        public async Task<UserStoreRelation> GetById(string userId, Guid storeId)
        {
            return (await _context.UserStoreRelations.FindAsync(userId, storeId))!;
        }

        public async Task<List<(Guid StoreId, StoreRole StoreRole)>> GetStoreIdsByUserId(string userId)
        {
            var result = await _context.UserStoreRelations
            .Where(usr => usr.UserId == userId)
            .Select(usr => new { usr.StoreId, usr.Role })
            .ToListAsync();

            return result.Select(x => (x.StoreId, x.Role)).ToList();
        }

        public async Task<List<(string UserId, StoreRole StoreRole)>> GetUserIdsByStoreId(Guid storeId)
        {
            var result = await _context.UserStoreRelations
                .Where(usr => usr.StoreId == storeId)
                .Select(usr => new { usr.UserId, usr.Role })
                .ToListAsync();

            return result.Select(x => (x.UserId, x.Role)).ToList();
        }

        public void Update(UserStoreRelation userStoreRelation)
        {
            _context.UserStoreRelations.Update(userStoreRelation);
        }
    }
}
