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
    public class StoreCategoryRelationRepository : IStoreCategoryRelationRepository
    {
        private readonly IDbContextFactory<BusinessDbContext> _dbContextFactory;
        private readonly BusinessDbContext _context;
        private readonly IMapper _mapper;

        public StoreCategoryRelationRepository(IDbContextFactory<BusinessDbContext> dbContextFactory, BusinessDbContext context, IMapper mapper)
        {
            _dbContextFactory = dbContextFactory;
            _context = context;
            _mapper = mapper;
        }

        public void Add(StoreCategoryRelation storeCategoryRelation)
        {
            _context.StoreCategoryRelations.Add(storeCategoryRelation);
        }

        public void Delete(StoreCategoryRelation storeCategoryRelation)
        {
            _context.StoreCategoryRelations.Remove(storeCategoryRelation);
        }

        public Task<PagedList<StoreCategoryRelationDto>> GetAll(StoreCategoryRelationParams storeCategoryRelationParams)
        {
            var query = _context.StoreCategoryRelations.AsQueryable();

            return PagedList<StoreCategoryRelationDto>.CreateAsync(
                query.AsNoTracking().ProjectTo<StoreCategoryRelationDto>(_mapper.ConfigurationProvider),
                storeCategoryRelationParams.PageNumber,
                storeCategoryRelationParams.PageSize);
        }

        public async Task<List<StoreCategoryRelation>> GetAll()
        {
            return await _context.StoreCategoryRelations.ToListAsync();
        }

        public async Task<StoreCategoryRelation> GetById(Guid storeId, Guid storeCategoryId)
        {
            return (await _context.StoreCategoryRelations.FindAsync(storeId, storeCategoryId))!;
        }

        public async Task<List<Guid>> GetStoreCategoryIdsByStoreId(Guid storeId)
        {
            return await _context.StoreCategoryRelations
                .Where(scr => scr.StoreId == storeId)
                .Select(scr => scr.StoreCategoryId)
                .ToListAsync();
        }

        public Task<List<Guid>> GetStoreIdsByStoreCategoryId(Guid storeCategoryId)
        {
            return _context.StoreCategoryRelations
                .Where(scr => scr.StoreCategoryId == storeCategoryId)
                .Select(scr => scr.StoreId)
                .ToListAsync();
        }

        public void Update(StoreCategoryRelation storeCategoryRelation)
        {
            _context.StoreCategoryRelations.Update(storeCategoryRelation);
        }
    }
}
