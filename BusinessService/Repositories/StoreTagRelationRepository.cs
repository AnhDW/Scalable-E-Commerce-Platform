using AutoMapper;
using AutoMapper.QueryableExtensions;
using BusinessService.Data;
using BusinessService.Entities;
using BusinessService.Repositories.IRepositories;
using Common.Helper;
using Common.Helper.EntityParams;
using Contracts.DTOs.Business;
using Microsoft.EntityFrameworkCore;

namespace BusinessService.Repositories
{
    public class StoreTagRelationRepository : IStoreTagRelationRepository
    {
        private readonly IDbContextFactory<BusinessDbContext> _dbContextFactory;
        private readonly BusinessDbContext _context;
        private readonly IMapper _mapper;

        public StoreTagRelationRepository(IDbContextFactory<BusinessDbContext> dbContextFactory, BusinessDbContext context, IMapper mapper)
        {
            _dbContextFactory = dbContextFactory;
            _context = context;
            _mapper = mapper;
        }

        public void Add(StoreTagRelation storeTagRelation)
        {
            _context.StoreTagRelations.Add(storeTagRelation);
        }

        public void Delete(StoreTagRelation storeTagRelation)
        {
            _context.StoreTagRelations.Remove(storeTagRelation);
        }

        public Task<PagedList<StoreTagRelationDto>> GetAll(StoreTagRelationParams storeTagRelationParams)
        {
            var query = _context.StoreTagRelations.AsQueryable();

            return PagedList<StoreTagRelationDto>.CreateAsync(
                query.AsNoTracking().ProjectTo<StoreTagRelationDto>(_mapper.ConfigurationProvider),
                storeTagRelationParams.PageNumber,
                storeTagRelationParams.PageSize);
        }

        public async Task<List<StoreTagRelation>> GetAll()
        {
            return await _context.StoreTagRelations.ToListAsync();
        }

        public async Task<StoreTagRelation> GetById(Guid storeId, Guid storeTagId)
        {
            return (await _context.StoreTagRelations.FindAsync(storeId, storeTagId))!;
        }

        public async Task<List<Guid>> GetStoreIdsByStoreTagId(Guid storeTagId)
        {
            return await _context.StoreTagRelations
                .Where(str => str.StoreTagId == storeTagId)
                .Select(str => str.StoreId)
                .ToListAsync();
        }

        public async Task<List<Guid>> GetStoreTagIdsByStoreId(Guid storeId)
        {
            return await _context.StoreTagRelations
                .Where(str => str.StoreId == storeId)
                .Select(str => str.StoreTagId)
                .ToListAsync();
        }

        public void Update(StoreTagRelation storeTagRelation)
        {
            _context.StoreTagRelations.Update(storeTagRelation);
        }
    }
}
