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
    private readonly ILogger<CartController> logger;

    public CartController(ICartRepository cartRepository, ILogger<CartController> logger)
    {
        this.cartRepository = cartRepository;
        this.logger = logger;
    }

    [HttpGet]
    public async Task<Basket> GetCart(string userName)
    {
        logger.LogInformation("Getting cart for user: {UserName}", userName);
        var basket = await cartRepository.GetBasket(userName);
        if (basket == null)
        {
            logger.LogWarning("Basket not found for user: {UserName}", userName);
        }
        return basket;
    }

    [HttpPost]
    public async Task<Basket> UpdateBasket(Basket basket)
    {
        logger.LogInformation("Updating basket for user: {UserName}", basket.UserName);
        try
        {
            var updatedBasket = await cartRepository.UpdateBasket(basket);
            logger.LogInformation("Basket updated for user: {UserName}", basket.UserName);
            return updatedBasket;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error updating basket for user: {UserName}", basket.UserName);
            throw;
        }
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteBasket(string userName)
    {
        logger.LogInformation("Deleting basket for user: {UserName}", userName);
        try
        {
            await cartRepository.RemoveBasket(userName);
            logger.LogInformation("Basket deleted for user: {UserName}", userName);
            return Ok();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error deleting basket for user: {UserName}", userName);
            return StatusCode(500, "Internal server error");
        }
    }
}