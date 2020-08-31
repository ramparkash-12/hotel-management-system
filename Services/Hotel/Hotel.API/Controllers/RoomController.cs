using System;
using System.Threading.Tasks;
using Hotel.API.Helpers;
using Hotel.API.Model;
using Hotel.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace Hotel.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RoomController : ControllerBase
    {
       private readonly IRoomRespository _repo;

        public RoomController(IRoomRespository repo)
        {
            _repo = repo;
        } 

        // Get All: api/Room/RoomsList
        [HttpGet("RoomsList")]
        public async Task<IActionResult> RoomsList()
        {
            var rooms = await _repo.GetAll();
            return Ok(rooms);
        }

        // Get: api/room/id
        [HttpGet("{id}")]
        public async Task<IActionResult> Room(int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var room = await _repo.Get(id);
            
            if (room == null)
                return NotFound();

            return Ok(room);
        }

        [HttpGet]
        // Search: api/room/RoomtypeId=1&HotelId=2"
        public async Task<IActionResult> SearchRoom([FromQuery] RoomSearchParams roomSearchParams)
        {
            var rooms = await _repo.Search(roomSearchParams);
            return Ok(rooms);
        }

        // Save: api/Room/model
        [HttpPost]
        public async Task<IActionResult> PostRoom([FromBody]Room model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _repo.Insert(model);
            await _repo.SaveAll();

            return Ok();
        }

        // Update: api/room/model
        [HttpPut]
        public async Task<IActionResult> PutRoom([FromBody]Room model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var room = await _repo.Get(model.Id);
            if (room == null)
                return NotFound($"Room with id {model.Id} not found.");

            _repo.Update(model);

            try
            {
                await _repo.SaveAll();
            }catch(Exception ex)
            {
                throw ex;
            }
            
            return Ok();
            //return CreatedAtAction(nameof(Room), new { id = model.Id }, null);
        }

        // Delete: api/Room/id
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRoom(int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var room = await _repo.Get(id); //GetById(id);
            if (room == null)
                return NotFound($"Room with id {id} not found.");

            _repo.Delete(room);

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