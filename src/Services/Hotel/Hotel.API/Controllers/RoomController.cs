using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Hotel.API.Helpers;
using Hotel.API.Model;
using Hotel.API.Model.Dtos;
using Hotel.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace Hotel.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class RoomController : ControllerBase
    {
       private readonly IRoomRespository _repo;
       private readonly IMapper _mapper;

        public RoomController(IRoomRespository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        } 

        // Get All: api/Room/RoomsList
        [HttpGet("RoomsList")]
        public async Task<IActionResult> RoomsList()
        {
            var rooms = await _repo.GetAll();
            var roomsToReturn = _mapper.Map<IEnumerable<RoomForListDto>>(rooms);
            return Ok(roomsToReturn);
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
            
            var roomToReturn = _mapper.Map<RoomForDetailsDto>(room);

            return Ok(roomToReturn);
        }

         [HttpGet("SearchRoom")]
        // Search: api/room/RoomtypeId=1&HotelId=2"
        public async Task<IActionResult> SearchRoom([FromQuery] RoomSearchParams roomSearchParams)
        {
            var rooms = await _repo.Search(roomSearchParams);
            var roomsToReturn = _mapper.Map<IEnumerable<RoomForListDto>>(rooms);
            return Ok(roomsToReturn);
        }

        // Save: api/Room/model
        [HttpPost("Save")]
        public async Task<IActionResult> PostRoom([FromBody]Room model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _repo.Insert(model);
            await _repo.SaveAll();

            return NoContent();
        }

        // Update: api/room/model
        [HttpPut("Update")]
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