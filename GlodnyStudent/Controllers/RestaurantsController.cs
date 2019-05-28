using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using GlodnyStudent.Models;
using GlodnyStudent.Models.Domain;
using GlodnyStudent.Models.Repositories;
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
        private readonly ICuisineRepository _cuisineRepository;
        private readonly IReviewRepository _reviewRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public RestaurantsController(IRestaurantRepository restaurantRepository,ICuisineRepository cuisineRepository ,IReviewRepository reviewRepository,IUserRepository userRepository,IMapper mapper)
        {
            _restaurantRepository = restaurantRepository;
            _cuisineRepository = cuisineRepository;
            _reviewRepository = reviewRepository;
            _userRepository = userRepository;
            _mapper = mapper;
        }

        [HttpGet("{address}")]
        public async Task<IActionResult> Get(string address)
        {
            try
            {
                var result = await _restaurantRepository.GetRestaurantsByStreet(address);
                if (result == null)
                    return NotFound();

                return Ok(_mapper.Map<RestaurantListViewModel[]>(result));
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Database Failure!");
            }
        }
        
        [HttpGet]
        public  async Task<IActionResult> Filters(string address,int distance,int highestPrice,string cuisine)
        {
            try
            {
                var result = await _restaurantRepository.GetRestaurantsByStreet(address);
                var filteredResult = from r in result
                    where r.HighestPrice <= highestPrice && r.Cuisine.Name == cuisine
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
        public  async Task<ActionResult<Cuisine[]>> Cuisines()
        {
            try
            {
                return await _cuisineRepository.FindAll();

            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Database Failure!");

            }
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<RestaurantDetailsViewModel>> RestaurantDetails(int id)
        {
            try
            {
                var result = await _restaurantRepository.FindById(id);
                if (result == null)
                    return NotFound();

                return _mapper.Map<Restaurant, RestaurantDetailsViewModel>(result);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Database Failure!");
            }
        }

        [HttpPost("{id:int}/[action]")]
        public async Task<ActionResult<RatingViewModel>> UpVote(int id)
        {
            try
            {
                var result = await _restaurantRepository.FindById(id);
                result.Score++;

                return new RatingViewModel{Rating = result.Score};
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Database Failure!");
            }
        }

        [HttpPost("{id:int}/[action]")]
        public async Task<ActionResult<RatingViewModel>> DownVote(int id)
        {
            try
            {
                var result = await _restaurantRepository.FindById(id);
                result.Score--;

                return new RatingViewModel{Rating = result.Score};
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Database Failure!");
            }
        }

        [HttpPut("{id:int}/[action]")]
        public  async Task<ActionResult<ReviewViewModel>> AddReview([FromBody]Review review,int id)
        {
            try
            {
                var users = await _userRepository.FindAll();
                var restaurant = await _restaurantRepository.FindById(id);

                if (restaurant == null)
                    return BadRequest();

                review.AddTime = DateTime.Now;
                review.UserId = users.First().Id;
                review.RestaurantId = id;

                var addedReview = await _reviewRepository.Create(review);

                return _mapper.Map<Review, ReviewViewModel>(addedReview);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Database Failure!");
            }
        }
    }
}