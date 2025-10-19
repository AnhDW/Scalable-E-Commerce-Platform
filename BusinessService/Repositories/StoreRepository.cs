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
    public class StoreRepository : IStoreRepository
    {
        private readonly IDbContextFactory<BusinessDbContext> _dbContextFactory;
        private readonly BusinessDbContext _context;
        private readonly IMapper _mapper;

        public StoreRepository(IDbContextFactory<BusinessDbContext> dbContextFactory, BusinessDbContext context, IMapper mapper)
        {
            _dbContextFactory = dbContextFactory;
            _context = context;
            _mapper = mapper;
        }

        public void Add(Store store)
        {
            _context.Stores.Add(store);
        }

        public void Delete(Store store)
        {
            _context.Stores.Remove(store);
        }

        public Task<PagedList<StoreDto>> GetAll(StoreParams storeParams)
        {
            var query = _context.Stores.AsQueryable();

            return PagedList<StoreDto>.CreateAsync(
                query.AsNoTracking().ProjectTo<StoreDto>(_mapper.ConfigurationProvider),
                storeParams.PageNumber,
                storeParams.PageSize);
        }

        public async Task<List<Store>> GetAll()
        {
            return await _context.Stores.ToListAsync();
        }

        public async Task<Store> GetById(Guid id)
        {
            return (await _context.Stores.FindAsync(id))!;
        }

        public async Task<List<Store>> GetByIds(List<Guid> ids)
        {
            return await _context.Stores.Where(s => ids.Contains(s.Id)).ToListAsync();
        }

        public void Update(Store store)
        {
            _context.Stores.Update(store);
        }
    }
}
