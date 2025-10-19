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
    public class StoreTagRepository : IStoreTagRepository
    {
        private readonly IDbContextFactory<BusinessDbContext> _dbContextFactory;
        private readonly BusinessDbContext _context;
        private readonly IMapper _mapper;

        public StoreTagRepository(IDbContextFactory<BusinessDbContext> dbContextFactory, BusinessDbContext context, IMapper mapper)
        {
            _dbContextFactory = dbContextFactory;
            _context = context;
            _mapper = mapper;
        }

        public void Add(StoreTag storeTag)
        {
            _context.StoreTags.Add(storeTag);
        }

        public void Delete(StoreTag storeTag)
        {
            _context.StoreTags.Remove(storeTag);
        }

        public Task<PagedList<StoreTagDto>> GetAll(StoreTagParams storeTagParams)
        {
            var query = _context.StoreTags.AsQueryable();

            return PagedList<StoreTagDto>.CreateAsync(
                query.AsNoTracking().ProjectTo<StoreTagDto>(_mapper.ConfigurationProvider),
                storeTagParams.PageNumber,
                storeTagParams.PageSize);
        }

        public async Task<List<StoreTag>> GetAll()
        {
            return await _context.StoreTags.ToListAsync();
        }

        public async Task<StoreTag> GetById(Guid id)
        {
            return (await _context.StoreTags.FindAsync(id))!;
        }

        public async Task<List<StoreTag>> GetByIds(List<Guid> ids)
        {
            return await _context.StoreTags.Where(s => ids.Contains(s.Id)).ToListAsync();
        }

        public void Update(StoreTag storeTag)
        {
            _context.StoreTags.Update(storeTag);
        }
    }
}
