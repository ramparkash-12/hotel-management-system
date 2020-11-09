using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Booking.API.Helpers;
using Booking.API.Idempotency;
using Booking.API.Model.Dtos;
using Booking.API.Services;
using Microsoft.AspNetCore.Mvc;
using Stripe;

namespace Booking.API.Controllers
{
  [ApiController]
  [Route("api/v1/[controller]")]
  public class BookingController : ControllerBase
  {
    private readonly IBookingRepository _repo;
    private readonly IMapper _mapper;
    private readonly IEventRequestManager _eventRequestManager;

    private readonly IEmailNotifier _emailNotifier;
    private readonly IRazorViewToStringRenderer _razorViewToStringRenderer;

    public BookingController(IBookingRepository repo, IMapper mapper, IEventRequestManager eventRequestManager, IEmailNotifier emailNotifier, IRazorViewToStringRenderer razorViewToStringRenderer)
    {
      _repo = repo;
      _mapper = mapper;
      _eventRequestManager = eventRequestManager;
      _emailNotifier = emailNotifier;
      _razorViewToStringRenderer = razorViewToStringRenderer;
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
            var booking = new Model.Booking();

            //** Charge Payment
            
            //** Map DTO's to models and add to repo...
            var paymentMethod = _mapper.Map<Model.PaymentMethod>(model);
            _repo.Insert(paymentMethod);

            booking = _mapper.Map<Model.Booking>(model);
            var bookingPayment = _mapper.Map<Model.BookingPayment>(model);
            booking.BookingPayments.Add(bookingPayment);

            _repo.Insert(booking);
            await _repo.SaveAll();
            
            /*
            //** Send email of booking
            var _emailText = await _razorViewToStringRenderer.RenderViewToStringAsync("~/Views/Email/BookingConfirmation.cshtml", booking);
   
            Booking.API.Model.EmailModel ObjEmail = new Booking.API.Model.EmailModel();
            ObjEmail.Email = _emailText;
            ObjEmail.To.Name = booking.CustomerName;
            ObjEmail.To.Email = "haristauqir@gmail.com";
            ObjEmail.Subject = "Booking Confirmation #: " + booking.Id;

            await _emailNotifier.SendEmailAsync(ObjEmail);
            */

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

            await _repo.SaveAll();
            
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