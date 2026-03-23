using Application.Services;
using Application.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Api.Controllers;

[ApiController]
[Route("api/customers")]
public class CustomerController : ControllerBase
{
    private readonly CustomerService _customerService;

    public CustomerController(CustomerService customerService)
    {
        _customerService = customerService;
    }

    // --- Create a new customer ---
    [HttpPost]
    public async Task<IActionResult> CreateCustomer([FromBody] CustomerDto dto)
    {
        var id = await _customerService.CreateCustomerAsync(dto.Name, dto.Email, dto.Phone);
        return Ok(new { CustomerId = id });
    }

    // --- Update customer info ---
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateCustomer(int id, [FromBody] CustomerDto dto)
    {
        await _customerService.UpdateCustomerAsync(id, dto.Name, dto.Email, dto.Phone);
        return NoContent();
    }

    // --- Get all customers ---
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var customers = await _customerService.GetAllCustomersAsync();
        return Ok(customers);
    }

    // --- Search customers ---
    [HttpGet("search")]
    public async Task<IActionResult> Search([FromQuery] string search)
    {
        var customers = await _customerService.SearchCustomersAsync(search);
        return Ok(customers);
    }
}