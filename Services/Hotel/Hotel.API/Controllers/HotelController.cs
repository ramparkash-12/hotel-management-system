using System;
using Microsoft.AspNetCore.Mvc;
using Hotel.API.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Hotel.API.Services;
using Hotel.API.Helpers;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Hotel.API.Model;
using System.IO;
using Microsoft.AspNetCore.Http;
using Hotel.API.Extensions;

namespace Hotel.API.Controllers
{
    [ApiController]
      [Route("api/v1/[controller]")]
    public class HotelController: ControllerBase
    {
        private readonly IHotelRepository _repo;
        private readonly IGenericRepository<Images> _imagesRepo;
        private readonly IImageService _imageService;

        public HotelController(IHotelRepository repo, IImageService imageService, IGenericRepository<Images> imagesRepo)
        {
            _repo = repo;
            _imageService = imageService;
            _imagesRepo = imagesRepo;
        }

   
        // Get All: api/Hotel/HotelsList
        //[Authorize]
        [HttpGet("HotelsList")]
        public async Task<ActionResult<List<Model.Hotel>>> HotelsList([FromQuery] HotelSearchParams searchParams)
        {
            var hotels = await _repo.GetAll(searchParams);
            
            Response.AddPagination(hotels.CurrentPage, hotels.PageSize, hotels.TotalCount,
            hotels.TotalPages);
            
            return hotels.ToList();
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
        //[Authorize]
        [HttpPost("Save")]
        public async Task<ActionResult<Model.Hotel>> PostHotel()
        {
            
            //** store request in variable 
            var _request = Request;
            var _formValues = _request.Form.ToDictionary(x => x.Key, x => x.Value.ToString());

            Model.Hotel model = MapFormDataToModel(_formValues);
        
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _repo.Insert(model);

            //** Save Image entries in Database and upload....
            var files = _request.Form.Files;
            if(files != null && files.Count > 0)
            {
                IEnumerable<Images> imagesList = MapImagesFormDataToModelAndSave(files, model);
                await _repo.SaveAll();
                try
                {
                    await _imageService.Upload(imagesList);
                }
                catch(Exception ex)
                {
                    _repo.Delete(model);
                    _imagesRepo.DeleteRange(imagesList);
                    await _repo.SaveAll();

                    return BadRequest(ex.Message);
                }
            } 
            else
            {
                await _repo.SaveAll();
            }

            return Ok();
            
        }

        private IEnumerable<Images> MapImagesFormDataToModelAndSave(IFormFileCollection files, Model.Hotel model)
        {
            List<Images> _imageList = new List<Images>();
            foreach (var file in files)
            {
                Images image = new Images();
                image.Name = file.FileName;
                image.Size = (Math.Round(((decimal)file.Length / (1024 * 1024)), 8));
                image.Extension = Path.GetExtension(file.FileName);
                image.Hotel = model;
                
                image.ImageType = (int) ImageType.HotelImage;
                image.Image = file;

                _imageList.Add(image);
                _imagesRepo.Insert(image);
            }

            return _imageList;
        }

        private Model.Hotel MapFormDataToModel(Dictionary<string,string> formValues)
        {
            Model.Hotel model = new Model.Hotel();

            model.Name = formValues["name"];
            model.Description = formValues["description"];
            model.Address = formValues["address"];
            model.Country = formValues["country"];
            model.City = formValues["city"];
            model.Status = Convert.ToBoolean(formValues["status"]);
            model.Stars = Convert.ToInt16(formValues["stars"]);
            model.IsFeatured = Convert.ToBoolean(formValues["isFeatured"]);
            model.FeaturedFrom = string.IsNullOrWhiteSpace(formValues["featuredFrom"]) && model.IsFeatured == false ? DateTime.MinValue : Extensions.Extensions.ConvertStringToDateTime(formValues["featuredFrom"]);
            model.FeaturedTo = string.IsNullOrWhiteSpace(formValues["featuredTo"]) && model.IsFeatured == false ? DateTime.MinValue :  Extensions.Extensions.ConvertStringToDateTime(formValues["featuredTo"]);

            return model;
        }

        // Update: api/Hotel/model
        [HttpPut("Update")]
        public async Task<ActionResult<Model.Hotel>> PutHotel([FromBody]Model.Hotel model)
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
            
            return model;
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
                if (hotel.Images != null && hotel.Images.Count > 0)
                {
                    await _imageService.Delete(hotel.Images);
                }
                await _repo.SaveAll();

            }catch(Exception ex)
            {
                throw ex;
            }
            
            return Ok();
        }


        [HttpGet("SearchHotel")]
        // Search: api/Hotel/name="s"&city="a"
        public async Task<ActionResult<PagedList<Model.Hotel>>> SearchHotel([FromQuery] HotelSearchParams hotelSearchParams)
        {
            var hotels = await _repo.Search(hotelSearchParams);
            return hotels;
        }
        

        
    }
}