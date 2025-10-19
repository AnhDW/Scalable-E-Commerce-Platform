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
    public class StoreCategoryRepository : IStoreCategoryRepository
    {
        private readonly IDbContextFactory<BusinessDbContext> _dbContextFactory;
        private readonly BusinessDbContext _context;
        private readonly IMapper _mapper;

        public StoreCategoryRepository(IDbContextFactory<BusinessDbContext> dbContextFactory, BusinessDbContext context, IMapper mapper)
        {
            _dbContextFactory = dbContextFactory;
            _context = context;
            _mapper = mapper;
        }

        public void Add(StoreCategory storeCategory)
        {
            _context.StoreCategories.Add(storeCategory);
        }

        public void Delete(StoreCategory storeCategory)
        {
            _context.StoreCategories.Remove(storeCategory);
        }

        public Task<PagedList<StoreCategoryDto>> GetAll(StoreCategoryParams storeCategoryParams)
        {
            var query = _context.StoreCategories.AsQueryable();

            return PagedList<StoreCategoryDto>.CreateAsync(
                query.AsNoTracking().ProjectTo<StoreCategoryDto>(_mapper.ConfigurationProvider),
                storeCategoryParams.PageNumber,
                storeCategoryParams.PageSize);
        }

        public async Task<List<StoreCategory>> GetAll()
        {
            return await _context.StoreCategories.ToListAsync();
        }

        public async Task<StoreCategory> GetById(Guid id)
        {
            return (await _context.StoreCategories.FindAsync(id))!;
        }

        public async Task<List<StoreCategory>> GetByIds(List<Guid> ids)
        {
            return await _context.StoreCategories.Where(s => ids.Contains(s.Id)).ToListAsync();
        }

        public void Update(StoreCategory storeCategory)
        {
            _context.StoreCategories.Update(storeCategory);
        }
    }
}
