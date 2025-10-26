using Common.Helper;
using Common.Helper.EntityParams;
using Contracts.DTOs.ShoppingCart;
using ShoppingCartService.Entities;

namespace ShoppingCartService.Repositories.IRepositories
{
    public interface ICartItemRepository
    {
        Task<PagedList<CartItemDto>> GetAll(CartItemParams cartItemParams);
        Task<List<CartItem>> GetAll();
        Task<CartItem> GetById(string userId, Guid storeId, Guid productVariantId);
        void Add(CartItem cartItem);
        void Update(CartItem cartItem);
        void Delete(CartItem cartItem);
        Task<bool> CheckUserCartItemExists(string userId, Guid storeId, Guid productVariantId);
    }
}
