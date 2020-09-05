using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Booking.API.Helpers;
using Booking.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace Booking.API.Controllers
{
  [ApiController]
  [Route("api/v1/[controller]")]
  public class BookingController : ControllerBase
  {
    private readonly IBookingRepository _repo;
    private readonly IMapper _mapper;

    public BookingController(IBookingRepository repo, IMapper mapper)
    {
      _repo = repo;
      _mapper = mapper;
    }

    // Get All: api/Booking/BookingssList
    [HttpGet("BookingssList")]
    public async Task<IActionResult> BookingssList()
    {
      var bookings = await _repo.GetAll();
      return Ok(bookings);
    }

    // Get: api/Booking/id
    [HttpGet("{id}")]
    public async Task<IActionResult> Booking(int id)
    {
      if (!ModelState.IsValid)
        return BadRequest(ModelState);

      var booking = await _repo.Get(id);

      if (booking == null)
        return NotFound();

      return Ok(booking);
    }

    [HttpGet("SearchBooking")]
        // Search: api/booking/HotelId=2"
        public async Task<IActionResult> SearchBooking([FromQuery] BookingSearchParams bookingSearchParams)
        {
            var bookings = await _repo.Search(bookingSearchParams);
            return Ok(bookings);
        }

        // Save: api/Booking/model
        [HttpPost("Save")]
        public async Task<IActionResult> PostBooking([FromBody]Model.Booking model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _repo.Insert(model);
            await _repo.SaveAll();

            return NoContent();
        }

        // Update: api/Booking/model
        [HttpPut("Update")]
        public async Task<IActionResult> PutBooking([FromBody]Model.Booking model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var booking = await _repo.Get(model.Id);
            if (booking == null)
                return NotFound($"Booking with id {model.Id} not found.");

            _repo.Update(model);

            try
            {
                await _repo.SaveAll();
            }catch(Exception ex)
            {
                throw ex;
            }
            
            return Ok();
        }

        // Delete: api/Booking/id
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBooking(int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var booking = await _repo.Get(id);
            if (booking == null)
                return NotFound($"Booking with id {id} not found.");

            _repo.Delete(booking);

            try
            {
                await _repo.SaveAll();
            }catch(Exception ex)
            {
                throw ex;
            }
            
            return Ok();
        }
  }
}