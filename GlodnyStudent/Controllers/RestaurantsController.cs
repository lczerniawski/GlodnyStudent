using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GlodnyStudent.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GlodnyStudent.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RestaurantsController : ControllerBase
    {
        private readonly IRestaurantRepository _restaurantRepository;

        public RestaurantsController(IRestaurantRepository restaurantRepository)
        {
            _restaurantRepository = restaurantRepository;
        }

        [HttpGet("{address}")]
        public IActionResult Get(string address)
        {
            var result = _restaurantRepository.GetRestaurantsByStreet(address);
            if(!result.Any())
                return NotFound();

            return Ok(result);

        }

        [HttpGet]
        public IActionResult Filters(string address,int distance,int highestPrice,string cuisine)
        {
            var result = _restaurantRepository.GetRestaurantsByStreet(address);

            var filteredResult = from r in result
                where r.Distance <= distance && r.HighestPrice <= highestPrice && r.Cuisine == cuisine
                select r;

            if (filteredResult.Any())
                return Ok(filteredResult);

            return BadRequest();
        }
    }
}