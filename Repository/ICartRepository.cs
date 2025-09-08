using cart.api.Entities;

namespace cart.api.Repository;

public interface ICartRepository
{
    Task<Basket> GetBasket(string userName);

    Task<Basket> UpdateBasket(Basket basket);

    Task RemoveBasket(string userName);
}