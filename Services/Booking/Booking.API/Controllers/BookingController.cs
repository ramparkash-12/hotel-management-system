using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Booking.API.Helpers;
using Booking.API.Idempotency;
using Booking.API.Model.Dtos;
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
    private readonly IEventRequestManager _eventRequestManager;

    public BookingController(IBookingRepository repo, IMapper mapper, IEventRequestManager eventRequestManager)
    {
      _repo = repo;
      _mapper = mapper;
      _eventRequestManager = eventRequestManager;
    }

    // Get All: api/Booking/BookingsList
    [HttpGet("BookingsList")]
    public async Task<IActionResult> BookingsList()
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
        public async Task<IActionResult> PostBooking([FromBody]BookingDto model, 
        [FromHeader(Name = "x-requestid")] Guid requestId)
        {
            if (requestId != Guid.Empty)    
                return BadRequest("RequestId is missing!");

            //** Check idempotency
            var requestExists = _eventRequestManager.ExistAsync(requestId).Result;

            if (!requestExists)
            {
                await _eventRequestManager.SaveEventRequest<Model.Booking>(requestId);

                //** Charge Payment
                try
                {

                }
                catch (Exception ex) 
                {
                    return BadRequest(ex.Message);
                }
                
                //** Map DTO's to models and add to repo...
                var paymentMethod = _mapper.Map<Model.PaymentMethod>(model);
                _repo.Insert(paymentMethod);

                var booking = _mapper.Map<Model.Booking>(model);
                var bookingPayment = _mapper.Map<Model.BookingPayment>(model);
                booking.BookingPayments.Add(bookingPayment);

                _repo.Insert(booking);
                await _repo.SaveAll();
            }
            else
            {
                return BadRequest("Request is already processed...");
            }

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