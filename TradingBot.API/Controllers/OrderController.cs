using Microsoft.AspNetCore.Mvc;
using TradingBot.Core.Abstractions;
using TradingBot.Core.Models;

namespace TradingBot.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class OrderController : ControllerBase
{
    private readonly IOrderRepository _orderRepository;

    public OrderController(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Order>> Get(string id)
    {
        return await _orderRepository.GetAsync(id);
    }

    [HttpPost]
    public async Task<ActionResult<string>> Add(Order order)
    {
        if (string.IsNullOrWhiteSpace(order.Id))
        {
            order.Id = Guid.NewGuid().ToString();
        }

        await _orderRepository.CreateAsync(order);
        return order.Id;
    }

    [HttpPatch]
    public async Task<ActionResult> Update(Order order)
    {
        if (string.IsNullOrWhiteSpace(order.Id))
        {
            return BadRequest("ID mandatory on update");
        }

        await _orderRepository.UpdateAsync(order);
        return Ok();
    }

    [HttpDelete]
    public async Task<ActionResult<bool>> Delete(string id)
    {
        return await _orderRepository.DeleteAsync(id);
    }

}