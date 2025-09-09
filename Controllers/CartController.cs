using System.Net;
using cart.api.Entities;
using cart.api.Repository;
using Microsoft.AspNetCore.Mvc;

namespace cart.api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class CartController : ControllerBase
{
    private readonly ICartRepository cartRepository;

    public CartController(ICartRepository cartRepository)
    {
        this.cartRepository = cartRepository;
    }

    [HttpGet]
    public async Task<Basket> GetCart(string userName)
    {
        return await cartRepository.GetBasket(userName);
    }

    [HttpPost]
    public async Task<Basket> UpdateBasket(Basket basket)
    {
        return await cartRepository.UpdateBasket(basket);
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteBasket(string userName)
    {
        await cartRepository.RemoveBasket(userName);
        return Ok();
    }
}