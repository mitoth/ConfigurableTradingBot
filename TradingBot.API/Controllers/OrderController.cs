using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using TradingBot.Core.Abstractions;
using TradingBot.Core.Models;

namespace TradingBot.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class OrderController : ControllerBase
{
    private readonly IOrderRepository _orderRepository;
    private readonly IValidator<Order> _validator;


    public OrderController(IOrderRepository orderRepository, IValidator<Order> validator)
    {
        _orderRepository = orderRepository;
        _validator = validator;
    }

    [HttpGet("all", Name = "GetAllOrders")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IEnumerable<Order>> GetAllOrders()
    {
        return await _orderRepository.GetAsync();
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
        await _validator.ValidateAsync(order);

        await _orderRepository.CreateAsync(order);
        return order.Id;
    }

    [HttpPatch]
    public async Task<ActionResult> Update(Order order)
    {
        await _validator.ValidateAsync(order);

        await _orderRepository.UpdateAsync(order);
        return Ok();
    }

    [HttpDelete]
    public async Task<ActionResult<bool>> Delete(string id)
    {
        return await _orderRepository.DeleteAsync(id);
    }

}