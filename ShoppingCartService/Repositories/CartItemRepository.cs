using AutoMapper;
using AutoMapper.QueryableExtensions;
using Common.Helper;
using Common.Helper.EntityParams;
using Contracts.DTOs.ShoppingCart;
using Microsoft.EntityFrameworkCore;
using ShoppingCartService.Data;
using ShoppingCartService.Entities;
using ShoppingCartService.Repositories.IRepositories;

namespace ShoppingCartService.Repositories
{
    public class CartItemRepository : ICartItemRepository
    {
        private readonly IDbContextFactory<ShoppingCartDbContext> _dbContextFactory;
        private readonly ShoppingCartDbContext _context;
        private readonly IMapper _mapper;

        public CartItemRepository(IDbContextFactory<ShoppingCartDbContext> dbContextFactory, ShoppingCartDbContext context, IMapper mapper)
        {
            _dbContextFactory = dbContextFactory;
            _context = context;
            _mapper = mapper;
        }

        public void Add(CartItem cartItem)
        {
            _context.CartItems.Add(cartItem);
        }

        public async Task<bool> CheckUserCartItemExists(string userId, Guid storeId, Guid productVariantId)
        {
            return await _context.CartItems.AnyAsync(x => x.UserId == userId && x.StoreId == storeId && x.ProductVariantId == productVariantId);
        }

        public void Delete(CartItem cartItem)
        {
            _context.CartItems.Remove(cartItem);
        }

        public async Task<PagedList<CartItemDto>> GetAll(CartItemParams cartItemParams)
        {
            var query = _context.CartItems.AsQueryable();

            return await PagedList<CartItemDto>.CreateAsync(
                query.AsNoTracking().ProjectTo<CartItemDto>(_mapper.ConfigurationProvider),
                cartItemParams.PageNumber,
                cartItemParams.PageSize);
        }

        public async Task<List<CartItem>> GetAll()
        {
            return await _context.CartItems.ToListAsync();
        }

        public async Task<CartItem> GetById(string userId, Guid storeId, Guid productVariantId)
        {
            return (await _context.CartItems.FindAsync(userId, storeId, productVariantId))!;
        }

        public void Update(CartItem cartItem)
        {
            _context.CartItems.Update(cartItem);
        }
    }
}
