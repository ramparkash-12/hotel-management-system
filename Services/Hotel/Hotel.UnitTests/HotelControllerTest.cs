using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Hotel.API.Controllers;
using Hotel.API.Services;
using Microsoft.AspNetCore.Mvc;
using Xunit;
using FakeItEasy;
using FluentAssertions;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using System;
using Hotel.API.Helpers;
using Hotel.API.Model;

namespace Hotel.UnitTests
{
    public class HotelControllerTest
    {
        private readonly HotelController _testee;
        private readonly API.Model.Hotel _createHotelModel;
        private readonly API.Model.Hotel _updateHotelModel;
        private readonly API.Helpers.HotelSearchParams _hotelSearchParams;
        private readonly PagedList<API.Model.Hotel> _listHotels;

        private readonly int _id = 48;
        private readonly IHotelRepository _repo;
        private readonly IGenericRepository<Images> _imagesRepo;
        private readonly IImageService _imageService;
       
        public HotelControllerTest()
        {
            _repo = A.Fake<IHotelRepository>();
            _imagesRepo = A.Fake<IGenericRepository<Images>>();
            _imageService = A.Fake<IImageService>();

            _testee = new HotelController(_repo, _imageService, _imagesRepo);

            _createHotelModel = new API.Model.Hotel
           {
               Id = _id,
               Name = "Test Hotel",
               City = "London",
               Description = "Sample text goes here",
               Address = "123 Ealing broadway, Tw1 8H5",
               Country = "UK"
           };

           _updateHotelModel = new API.Model.Hotel
           {
               Id = _id,
               Name = "Test Hotel",
               City = "London",
               Description = "Sample text goes here",
               Address = "123 Ealing broadway, Tw1 8H5",
               Country = "UK"
           };

           var hotels = new List<API.Model.Hotel>()
           {
               new API.Model.Hotel
               {
                    Id = 5,
                    Name = "Hotel Mayfair",
                    City = "London",
                    Description = "Sample text goes here",
                    Address = "580 Ealing broadway, Tw1 8H5",
                    Country = "UK"
               },
               new API.Model.Hotel
               {
                    Id = 6,
                    Name = "Travel Lodge",
                    City = "London",
                    Description = "Sample text goes here",
                    Address = "709 Ealing broadway, Tw1 8H5",
                    Country = "UK"
               }
           };

           _listHotels = new PagedList<API.Model.Hotel>(hotels, 2, 1, 2);


           _hotelSearchParams = new API.Helpers.HotelSearchParams
           {
               City = "London",
               PageSize = 2,
               PageNumber = 2
           };


           A.CallTo(() => _repo.Insert(A<API.Model.Hotel>._));
           A.CallTo(() => _repo.Update(A<API.Model.Hotel>._));

        }

        [Fact]
        public async void Hotel_ShouldReturnListOfHotels()
        {
            //Act
            var result = await _testee.HotelsList(_hotelSearchParams);
            
            //Assert
            //(result.Result as StatusCodeResult)?.StatusCode.Should().Be((int)HttpStatusCode.OK);
            result.Value.Should().BeOfType<List<API.Model.Hotel>>();
        }

        /*
        [Fact]
        public async void Post_ShouldReturnHotel()
        {   
            //Act
            var result = await _testee.PostHotel();
            
            //Assert
            (result.Result as StatusCodeResult)?.StatusCode.Should().Be((int)HttpStatusCode.OK);
            result.Value.Should().BeOfType<API.Model.Hotel>();
            result.Value.Id.Should().Be(_id);
        }
        */

        /*
        [Theory]
       [InlineData("PostHotel: Hotel is null")]
       public async void Post_WhenAnExceptionOccurs_ShouldReturnBadRequest(string exceptionMessage)
       {
           A.CallTo(() => _repo.Insert(A<API.Model.Hotel>._)).Throws(new Exception(exceptionMessage));

           var result = await _testee.PostHotel();

           (result.Result as StatusCodeResult)?.StatusCode.Should().Be((int) HttpStatusCode.BadRequest);
           (result.Result as BadRequestObjectResult)?.Value.Should().Be(exceptionMessage);
       }
       */

       [Fact]
       public async void Put_ShouldReturnHotel()
       {
           var result = await _testee.PutHotel(_updateHotelModel);

           (result.Result as StatusCodeResult)?.StatusCode.Should().Be((int) HttpStatusCode.OK);
           result.Value.Should().BeOfType<API.Model.Hotel>();
           result.Value.Id.Should().Be(_id);
       }

       [Theory]
       [InlineData("PutHotel: Hotel is null")]
       //[InlineData("No hotel with given id found")]
       public async void Put_WhenAnExceptionOccurs_ShouldReturnBadRequest(string exceptionMessage)
       {
           A.CallTo(() => _repo.Update(A<API.Model.Hotel>._)).Throws(new Exception(exceptionMessage));

           var result = await _testee.PutHotel(_updateHotelModel);

           (result.Result as StatusCodeResult)?.StatusCode.Should().Be((int) HttpStatusCode.BadRequest);
           (result.Result as BadRequestObjectResult)?.Value.Should().Be(exceptionMessage);
       }

        [Fact]
       public async void Delete_ShouldDeleteHotel()
       {
           var result = await _testee.DeleteHotel(_id);

           (result as StatusCodeResult)?.StatusCode.Should().Be((int) HttpStatusCode.OK);
       }

       [Theory]
       [InlineData("DeleteHotel: Hotel is null")]
       //[InlineData("No hotel with given id found")]
       public async void Delete_WhenAnExceptionOccurs_ShouldReturnBadRequest(string exceptionMessage)
       {
           A.CallTo(() => _repo.Delete(A<API.Model.Hotel>._)).Throws(new Exception(exceptionMessage));

           var result = await _testee.DeleteHotel(_id);

           (result as StatusCodeResult)?.StatusCode.Should().Be((int) HttpStatusCode.BadRequest);
           (result as BadRequestObjectResult)?.Value.Should().Be(exceptionMessage);
       }

        [Fact]
       public async void Search_ShouldReturnMatchedHotel()
       {

           A.CallTo(() => _repo.Search(A<HotelSearchParams>._)).Returns(_listHotels);
          
           var result = await _testee.SearchHotel(_hotelSearchParams);

           (result.Result as StatusCodeResult)?.StatusCode.Should().Be((int) HttpStatusCode.OK);

           var page = Assert.IsAssignableFrom<PagedList<API.Model.Hotel>>(result.Value);
           result.Value.Should().BeOfType<PagedList<API.Model.Hotel>>();

            page.PageSize.Should().Be(2);
            page.TotalCount.Should().Be(2);
            //Assert.Equal(2, page.PageSize); // page index....
            //Assert.Equal(2, page.TotalCount); // page items...
       }
       

    }
}