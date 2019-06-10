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
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.VisualStudio.Web.CodeGeneration.EntityFrameworkCore;

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
            /**
            *  <summary>  
            *Metoda RestaurantDetails zwraca dane restauracji na podstawie id.
            *</summary> 
            * 
            *<param name="id"> identyfikator restauracji
            * </param>
            * 
            *<returns>
            *W przypadku blędu bazy danych: Database Failure!\n
            *W przypadku powodzenia: mapa Restauracji i widok jej danych.\n
            *</returns>
            */
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
            /**
            *  <summary>  
            *Metoda UpVote pozwala na pozytywne zagłosowanie na restauracje/komentarz. Za pomocą repozytorium znajdowana jest restauracja oraz zmieniana jest jej ocena
            *</summary> 
            * 
            *<param name="id"> identyfikator restauracji/komentarza
            * </param>
            * 
            *<returns>
            *W przypadku blędu bazy danych: Database Failure!\n
            *W przypadku powodzenia: nowy widok oceny\n
            *</returns>
            */
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
            /**
            *  <summary>  
            *Metoda DownVote pozwala na negatywne zagłosowanie na restauracje/komentarz. Za pomocą repozytorium znajdowana jest restauracja oraz zmieniana jest jej ocena
            *</summary> 
            * 
            *<param name="id"> identyfikator restauracji/komentarza
            * </param>
            * 
            *<returns>
            *W przypadku blędu bazy danych: Database Failure!\n
            *W przypadku powodzenia: nowy widok oceny\n
            *</returns>
            */
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
            /**
            *  <summary>  
            *Metoda AddReview pozwala na dodanie komentarza na temat restauracji. Za pomocą repozytorium znajdowana jest restauracja, tworzony jest obiekt klasy Review zawierający, date, IDrestauracji, w której się znajduje oraz sam tekst komentarza.
            *</summary> 
            * 
            *<param name="id"> identyfikator komentarza
            * </param>
            *<param name="review"> Obiekt klasy Review reprezentujący komentarza
            *</param>
            * 
            *<returns>
            *W przypadku błędu bazy danych: Database Failure! \n
            *W przypadku błędu podczas dodawaniu opinii: Błąd przy dodawaniu opinii\n
            *W przypadku powodzenia: Mapa z opinią oraz jej widokiem\n
            *</returns>
            */

            try
            {
                var user = await _userRepository.FindById(review.UserId);
                if (user == null)
                    return BadRequest();

                var restaurant = await _restaurantRepository.FindById(id);

                if (restaurant == null)
                    return BadRequest();

                review.AddTime = DateTime.Now.Day + "." + DateTime.Now.Month + "." + DateTime.Now.Year + " " + DateTime.Now.Hour + ":" + DateTime.Now.Minute;
                review.RestaurantId = id;
                review.UserId = user.Id;

                var addedReview = await _reviewRepository.Create(review);
                if (addedReview == null)
                    return BadRequest("Błąd przy dodawaniu opinii");

                restaurant.ReviewsCount++;
                var result = await _restaurantRepository.Update(restaurant);
                if(result == null)
                    return BadRequest("Błąd przy dodawaniu opinii");

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
            /**
            *  <summary>  
            *Metoda UpdateName pozwala na zmiane nazwy restauracji. Za pomocą repozytorium znajdowana jest restauracja
            *</summary> 
            * 
            *<param name="id"> identyfikator restauracji
            * </param>
            *<param name="name"> string z nową nazwą restauracji
            *</param>
            * 
            *<returns>
            *W przypadku błędu bazy danych: Database Failure! \n
            *W przypadku powodzenia: zmieniona nazwa restauracji 
            *</returns>
            */
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
            /**
            *  <summary>  
            *Metoda UpdateAddress pozwala na zmiane adresu restauracji. Za pomocą repozytorium znajdowana jest restauracja i zmieniany jest jej adres.
            *</summary> 
            * 
            *<param name="id"> identyfikator restauracji
            * </param>
            *<param name="addressViewModel"> Obiekt klasy AddressViewModel zawierający widok edycji adresu.
            *</param>
            * 
            *<returns>
            *W przypadku błędu bazy danych: Database Failure! \n
            *W przypadku powodzenia: mapa restauracji oraz widok nowych danych.
            *</returns>
            */
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
                restaurantAddressToUpdate.StreetName = addressViewModel.StreetName;
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

        [HttpGet("[action]")]
        public  async Task<ActionResult<string[]>> AllCuisines()
        {
            /**
            *  <summary>  
            *Metoda AllCuisines pozwala pobranie wszystkich dostępnych rodzajów jedzenia w bazie danych.
            *</summary> 
            * 
            *<returns>
            *W przypadku błędu bazy danych: Database Failure! \n
            *W przypadku powodzenia: tablica z rodzajami jedzenia.\n
            *</returns>
            */
            try
            {
                HashSet<string> cuisines = new HashSet<string>();

                var cuisinesFromDb = await _cuisineRepository.FindAll();

                foreach (var cuisine in cuisinesFromDb)
                {
                    cuisines.Add(cuisine.Name);
                }

                return cuisines.ToArray();

            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Database Failure!");
            }
        }

        [HttpPut]
        public async Task<ActionResult<long>> CreateRestaurant(AddRestaurantViewModel addRestaurantViewModel)
        {
            /**
            *  <summary>  
            *Metoda CreateRestaurant pozwala na utworzenie nowej restauracji. Za pomocą repozytorium tworzona jest nowa restauracja o podanych w formularzu danych.
            *</summary> 
            * 
            *<param name="addRestaurantViewModel"> Obiekt klasy AddRestaurantViewModel zawierający widok rejestracji nowej restauracji.
            *</param>
            * 
            *<returns>
            *W przypadku błędu bazy danych: Database Failure! \n
            *W przypadku błędu przy dodawaniu: Nie udało się dodać restauracji"\n
            *W przypadku powodzenia: mapa restauracji oraz widok nowych danych.
            *</returns>
            */
            try
            {
                var user = await _userRepository.FindUserByUsername(addRestaurantViewModel.Username);
                if (user == null)
                    return BadRequest("Nie udało się dodać restauracji");
                
                Restaurant newRestaurant = new Restaurant
                {
                    Name = addRestaurantViewModel.RestaurantName,
                    OwnerId = user.Id
                };

                var addedRestaurant = await _restaurantRepository.Create(newRestaurant);

                if (addedRestaurant == null)
                    return BadRequest("Nie udało się dodać restauracji");

                var newCuisine = new Cuisine
                {
                    Name = addRestaurantViewModel.Cuisine,
                    RestaurantId = addedRestaurant.Id
                };

                if (await _cuisineRepository.Create(newCuisine) == null)
                    return BadRequest("Nie udało się dodać restauracji");

                var newAddress = new RestaurantAddress
                {
                    StreetName = addRestaurantViewModel.Address.StreetName,
                    District = addRestaurantViewModel.Address.District,
                    LocalNumber = addRestaurantViewModel.Address.LocalNumber,
                    StreetNumber = addRestaurantViewModel.Address.StreetNumber,
                    RestaurantId = addedRestaurant.Id
                };

                if (await _restaurantAddressRepository.Create(newAddress) == null)
                    return BadRequest(false);

                return addedRestaurant.Id;
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Database Failure!");
            }
        }
    }
}