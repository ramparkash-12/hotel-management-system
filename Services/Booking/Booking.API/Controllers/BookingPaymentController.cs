using System;
using System.Threading.Tasks;
using AutoMapper;
using Booking.API.Model;
using Booking.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace Booking.API.Controllers
{
  [ApiController]
  [Route("api/v1/[controller]")]
  public class BookingPaymentController : ControllerBase
  {
    private readonly IGenericRepository _repo;
    private readonly IMapper _mapper;

    public BookingPaymentController(IGenericRepository repo, IMapper mapper)
    {
      _repo = repo;
      _mapper = mapper;
    }

    // Save: api/BookingPayment/model
    [HttpPost("Save")]
    public async Task<IActionResult> PostBookingPayment([FromBody] BookingPayment model)
    {
      if (!ModelState.IsValid)
        return BadRequest(ModelState);

      _repo.Insert(model);
      await _repo.SaveAll();

      return NoContent();
    }

    // Update: api/BookingPayment/model
    [HttpPut("Update")]
    public async Task<IActionResult> PutBookingPayment([FromBody] Model.BookingPayment model)
    {
      if (!ModelState.IsValid)
        return BadRequest(ModelState);

      _repo.Update(model);

      try
      {
        await _repo.SaveAll();
      }
      catch (Exception ex)
      {
        throw ex;
      }

      return Ok();
    }

    // Delete: api/BookingPayment/id
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteBookingPayment(int id)
    {
      if (!ModelState.IsValid)
        return BadRequest(ModelState);

        BookingPayment booking = new BookingPayment() { Id = id };

      _repo.Delete(booking);

      try
      {
        await _repo.SaveAll();
      }
      catch (Exception ex)
      {
        throw ex;
      }

      return Ok();
    }


  }
}