using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using GlodnyStudent.Data.Abstract;
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
        private readonly IRestaurantAddressRepository _restaurantAddressRepository;
        private readonly IMapper _mapper;

        public RestaurantsController(IRestaurantRepository restaurantRepository,ICuisineRepository cuisineRepository ,IReviewRepository reviewRepository,IUserRepository userRepository,IRestaurantAddressRepository restaurantAddressRepository,IMapper mapper)
        {
            _restaurantRepository = restaurantRepository;
            _cuisineRepository = cuisineRepository;
            _reviewRepository = reviewRepository;
            _userRepository = userRepository;
            _restaurantAddressRepository = restaurantAddressRepository;
            _mapper = mapper;
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
                //review.UserId = users.First().Id;
                review.RestaurantId = id;

                var addedReview = await _reviewRepository.Create(review);

                return _mapper.Map<Review, ReviewViewModel>(addedReview);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Database Failure!");
            }
        }

        [HttpPost("{id:long}/[Action]")]
        public async Task<ActionResult<string>> UpdateName([FromBody]string name, long id)
        {
            try
            {
                var result = await _restaurantRepository.FindById(id);
                if (result == null)
                    return NotFound();

                result.Name = name;

                result = await _restaurantRepository.Update(result);

                return result.Name;
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Database Failure!");
            }
        }

        [HttpPost("{id:long}/[Action]")]
        public async Task<ActionResult<AddressViewModel>> UpdateAddress([FromBody]AddressViewModel addressViewModel, long id)
        {
            try
            {
                var restaurant = await _restaurantRepository.FindById(id);
                if (restaurant == null)
                    return NotFound();
                var restaurantAddressToUpdate = await _restaurantAddressRepository.FindById(restaurant.Address.Id);
                if (restaurantAddressToUpdate == null)
                    return NotFound();

                restaurantAddressToUpdate.District = addressViewModel.District;
                restaurantAddressToUpdate.LocalNumber = addressViewModel.LocalNumber;
                restaurantAddressToUpdate.Street = addressViewModel.Street;
                restaurantAddressToUpdate.StreetNumber = addressViewModel.StreetNumber;

                var result = await _restaurantAddressRepository.Update(restaurantAddressToUpdate);

                if (result == null)
                    return StatusCode(StatusCodes.Status500InternalServerError, "Database Failure!");

                return _mapper.Map<RestaurantAddress,AddressViewModel>(result);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Database Failure!");
            }
        }
    }
}