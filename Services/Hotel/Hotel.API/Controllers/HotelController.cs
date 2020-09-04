using System;
using Microsoft.AspNetCore.Mvc;
using Hotel.API.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Hotel.API.Services;
using Hotel.API.Helpers;

namespace Hotel.API.Controllers
{
    [ApiController]
      [Route("api/v1/[controller]")]
    public class HotelController: ControllerBase
    {
        private readonly IHotelRepository _repo;

        public HotelController(IHotelRepository repo)
        {
            _repo = repo;
        }

   
        // Get All: api/Hotel/HotelsList
        [HttpGet("HotelsList")]
        public async Task<IActionResult> HotelsList()
        {
            var hotels = await _repo.GetAll();
            return Ok(hotels);
        }

        // Get: api/Hotel/id
        [HttpGet("{id}")]
        public async Task<IActionResult> Hotel(int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var hotel = await _repo.Get(id);
            
            if (hotel == null)
                return NotFound();

            return Ok(hotel);
        }

        // Save: api/Hotel/model
        [HttpPost("Save")]
        public async Task<IActionResult> PostHotel([FromBody]Model.Hotel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _repo.Insert(model);
            await _repo.SaveAll();

            return Ok();
        }

        // Update: api/Hotel/model
        [HttpPut("Update")]
        public async Task<IActionResult> PutHotel([FromBody]Model.Hotel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var hotel = await _repo.Get(model.Id);
            if (hotel == null)
                return NotFound($"Hotel with id {model.Id} not found.");

            _repo.Update(model);

            try
            {
                await _repo.SaveAll();
            }catch(Exception ex)
            {
                throw ex;
            }
            
            return Ok();
            //return CreatedAtAction(nameof(Hotel), new { id = hotel.Id }, null);
        }
        
        // Delete: api/Hotel/id
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteHotel(int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var hotel = await _repo.Get(id); //GetById(id);
            if (hotel == null)
                return NotFound($"Hotel with id {id} not found.");

            _repo.Delete(hotel);

            try
            {
                await _repo.SaveAll();
            }catch(Exception ex)
            {
                throw ex;
            }
            
            return Ok();
        }


        [HttpGet("SearchHotel")]
        // Search: api/Hotel/name="s"&city="a"
        public async Task<IActionResult> SearchHotel([FromQuery] HotelSearchParams hotelSearchParams)
        {
            var hotels = await _repo.Search(hotelSearchParams);
            return Ok(hotels);
        }
        

        
    }
}