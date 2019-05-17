using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using GlodnyStudent.Models;
using GlodnyStudent.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GlodnyStudent.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RestaurantsController : ControllerBase
    {
        private readonly IRestaurantRepository _restaurantRepository;
        private readonly IMapper _mapper;

        public RestaurantsController(IRestaurantRepository restaurantRepository, IMapper mapper)
        {
            _restaurantRepository = restaurantRepository;
            _mapper = mapper;
        }

        [HttpGet("{address}")]
        public IActionResult Get(string address)
        {
            try
            {
                var result = _restaurantRepository.GetRestaurantsByStreet(address);
                if (!result.Any())
                    return NotFound();

                return Ok(_mapper.Map<RestaurantListViewModel[]>(result));
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Database Failure!");
            }
        }
        
        [HttpGet]
        public IActionResult Filters(string address,int distance,int highestPrice,string cuisine)
        {
            try
            {
                var result = _restaurantRepository.GetRestaurantsByStreet(address);

                var filteredResult = from r in result
                    where r.HighestPrice <= highestPrice && r.CuisineType.Name == cuisine //TODO Dodac filtorwanie po adresie jak zostanie napisany algorytm
                    orderby r.HighestPrice
                    select r;

                if (filteredResult.Any())
                    return Ok(_mapper.Map<RestaurantListViewModel[]>(filteredResult));

                return BadRequest();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Database Failure!");

            }
        }
        
        [HttpGet("[action]")]
        public ActionResult<Cuisine[]> Cuisines()
        {
            try
            {
                return _restaurantRepository.GetAllCuisines().ToArray();

            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Database Failure!");

            }
        }

        [HttpGet("{id:int}")]
        public ActionResult<RestaurantDetailsViewModel> RestaurantDetails(int id)
        {
            try
            {
                var result = _restaurantRepository.GetRestaurantById(id);
                if (result == null)
                    return NotFound();

                return _mapper.Map<Restaurant, RestaurantDetailsViewModel>(result);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Database Failure!");

            }
        }
    }
}