using System.Text.Json;
using cart.api.Entities;
using Microsoft.Extensions.Caching.Distributed;

namespace cart.api.Repository;

public class CartRepository(IDistributedCache redis) : ICartRepository
{
    private readonly IDistributedCache redis = redis;

    public async Task<Basket> GetBasket(string userName)
    {
        var value = await redis.GetStringAsync(userName ?? "");
        if (string.IsNullOrEmpty(value))
            return null;

        var basket = JsonSerializer.Deserialize<Basket>(value);
        return basket;
    }

    public async Task RemoveBasket(string userName)
    {
        await redis.RemoveAsync(userName);

    }

    public async Task<Basket> UpdateBasket(Basket basket)
    {
        var value = JsonSerializer.Serialize(basket);

        await redis.SetStringAsync(basket.UserName, value);

        return await GetBasket(basket.UserName);
    }
}
