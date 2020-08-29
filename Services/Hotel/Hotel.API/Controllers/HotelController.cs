using System;
using Microsoft.AspNetCore.Mvc;
using Hotel.API.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Hotel.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HotelController: ControllerBase
    {
        private readonly HotelContext _context;

        public HotelController(HotelContext context)
        {
            _context = context;
        }

        // Get All: api/Hotel/HotelsList
        [HttpGet("HotelsList")]
        public IActionResult HotelsList()
        {
            var hotels = _context.Hotels;
            return Ok(hotels);
        }

        // Get: api/Hotel/id
        [HttpGet("{id}")]
        public async Task<IActionResult> Hotel(int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var hotel = await _context.Hotels.FindAsync(id);
            
            if (hotel == null)
                return NotFound();

            return Ok(hotel);
        }

        // Save: api/Hotel/model
        [HttpPost]
        public async Task<IActionResult> PostHotel([FromBody]Model.Hotel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _context.Hotels.Add(model);
            await _context.SaveChangesAsync();

            return Ok();
        }

        // Update: api/Hotel/model
        [HttpPut]
        public async Task<IActionResult> PutHotel([FromBody]Model.Hotel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var hotel = _context.Hotels.SingleOrDefaultAsync(h => h.Id == model.Id);
            if (hotel == null)
                return NotFound($"Hotel with id {hotel.Id} not found.");

            _context.Hotels.Update(model);

            try
            {
                await _context.SaveChangesAsync();
            }catch(Exception ex)
            {
                throw ex;
            }
            
            return Ok();
            //return CreatedAtAction(nameof(Hotel), new { id = hotel.Id }, null);
        }
        
        // Delete: api/Hotel/id
        [HttpDelete("id")]
        public async Task<IActionResult> DeleteHotel(int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var hotel = _context.Hotels.SingleOrDefault(h => h.Id == id);
            if (hotel == null)
                return NotFound($"Hotel with id {hotel.Id} not found.");

            _context.Hotels.Remove(hotel);

            try
            {
                await _context.SaveChangesAsync();
            }catch(Exception ex)
            {
                throw ex;
            }
            
            return Ok();
        }


        /*// Search: api/Hotel/
        [HttpGet]
        public async Task<IActionResult> SearchHotel(string name, string city)
        {
            
            return Ok();
        }
        */

        
    }
}