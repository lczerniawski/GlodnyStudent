using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using GeoAPI.Geometries;
using GlodnyStudent.Data.Abstract;
using GlodnyStudent.Models;
using GlodnyStudent.Models.Domain;
using GlodnyStudent.Models.Repositories;
using GlodnyStudent.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.VisualStudio.Web.CodeGeneration.EntityFrameworkCore;
using NetTopologySuite.Geometries;

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
        private readonly IImageRepository _imageRepository;
        private readonly IMenuItemRepository _menuItemRepository;
        private readonly IMapper _mapper;

        public RestaurantsController(IRestaurantRepository restaurantRepository,ICuisineRepository cuisineRepository ,IReviewRepository reviewRepository,IUserRepository userRepository,IRestaurantAddressRepository restaurantAddressRepository,IImageRepository imageRepository,IMenuItemRepository menuItemRepository,IMapper mapper)
        {
            _restaurantRepository = restaurantRepository;
            _cuisineRepository = cuisineRepository;
            _reviewRepository = reviewRepository;
            _userRepository = userRepository;
            _restaurantAddressRepository = restaurantAddressRepository;
            _imageRepository = imageRepository;
            _menuItemRepository = menuItemRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// Metoda RestaurantDetails zwraca dane restauracji na podstawie id w obiekcie DTO RestaurantDetailsViewModel
        /// Odwołanie do API następuje po adresie "nazwahosta/api/Restaurants/[id restauracji]" metodą Get
        /// </summary>
        /// <param name="id">Id restauracji</param>
        /// <returns>
        /// W przypadku blędu bazy danych: Database Failure!\n
        /// W przypadku powodzenia: Status Code 200 wiadomosc, oraz obiekt resutaracji pod postacia DTO RestaurantDetailsViewModel
        /// W przypadki nie poprawnego ID: Status Code 404 oraz wiadomość "Nie ma restauracji o takim ID"
        /// </returns>
        [HttpGet("{id:long}")]
        public async Task<ActionResult<RestaurantDetailsViewModel>> RestaurantDetails(long id)
        {
            try
            {
                var result = await _restaurantRepository.FindById(id);
                if (result == null)
                    return NotFound(new { status = StatusCodes.Status404NotFound, message = "Nie ma restauracji o takim ID" });

                var restaurant = _mapper.Map<Restaurant, RestaurantDetailsViewModel>(result);

                return Ok(new { status = StatusCodes.Status200OK, message = "Ok", restaurant });

            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Database Failure!");
            }
        }

        /// <summary>
        /// Metoda AddReview pozwala na dodanie komentarza na temat restauracji. Za pomocą repozytorium znajdowana jest restauracja, tworzony jest obiekt klasy Review zawierający, date, IDrestauracji, w której ma się znaleźć oraz sam tekst komentarza.
        /// Odwołanie do API następuje po adresie "nazwahosta/api/Restaurants/[id restauracji]/AddReview" metodą PUT należy w body umiescić obiekt DTO Review oraz  header Authorization z tokenem wygenerowanym poprzez logowanie
        /// </summary>
        /// <param name="review"> Obiekt klasy Review reprezentujący komenatrz</param>
        /// <param name="id">Id komentarza</param>
        /// <returns>
        /// W przypadku błędu bazy danych: Database Failure! \n
        /// W przypadku błędu podczas dodawaniu opinii:Status code 400 oraz wiadomość "Błąd przy dodawaniu opinii"\n
        /// W przypadku powodzenia: Status Code 200 oraz obiekt opini reprezentowany przez klase ReviewViewModel\n
        /// </returns>
        [HttpPut("{id:int}/[action]")]
        [Authorize]
        public async Task<IActionResult> AddReview([FromBody]Review review, int id)
        {
            try
            {
                var user = await _userRepository.FindById(review.UserId);
                if (user == null)
                    return BadRequest(new { status = StatusCodes.Status400BadRequest, message = "Nie udało się dodać opini o restauracji" });


                var restaurant = await _restaurantRepository.FindById(id);

                if (restaurant == null)
                    return BadRequest(new { status = StatusCodes.Status400BadRequest, message = "Nie udało się dodać opini o restauracji" });

                review.AddTime = DateTime.Now.Day + "." + DateTime.Now.Month + "." + DateTime.Now.Year + " " + DateTime.Now.Hour + ":" + DateTime.Now.Minute;
                review.RestaurantId = id;
                review.UserId = user.Id;

                var addedReview = await _reviewRepository.Create(review);
                if (addedReview == null)
                    return BadRequest(new { status = StatusCodes.Status400BadRequest, message = "Nie udało się dodać opini o restauracji" });

                restaurant.ReviewsCount++;
                var result = await _restaurantRepository.Update(restaurant);
                if (result == null)
                    return BadRequest(new { status = StatusCodes.Status400BadRequest, message = "Nie udało się dodać opini o restauracji" });


                var newReview = _mapper.Map<Review, ReviewViewModel>(addedReview);

                return Ok(new { status = StatusCodes.Status200OK, newReview });
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Database Failure!");
            }
        }

        /// <summary>
        /// Metoda UpdateName pozwala na zmianę nazwy restauracji.
        /// Odwołanie do API następuje po adresie "nazwahosta/api/Restaurants/[id restauracji]/UpdateName" metodą PUT należy w body umieścić nową nazwę restauracji oraz header Authorization z tokenem wygenerowanym poprzez logowanie
        /// </summary>
        /// <param name="name">Nowa nazwa restauracji</param>
        /// <param name="id">Id restauracji</param>
        /// <returns>
        /// W przypadku błędu bazy danych: Database Failure! \n
        /// W przypadku błednego id restauracji: Status Code 404 oraz wiadomość "Nie udało się znaleźć resturacji o takim ID"
        /// W przypadku powodzenia: Status Code 200 oraz obiekt opini reprezentowany przez klase ReviewViewModel\n
        /// </returns>
        [HttpPost("{id:long}/[Action]")]
        [Authorize]
        public async Task<IActionResult> UpdateName([FromBody]string name, long id)
        {
            try
            {
                var result = await _restaurantRepository.FindById(id);
                if (result == null)
                    return NotFound(new { status = StatusCodes.Status404NotFound, message = "Nie udało się znaleźć resturacji o takim ID" });


                result.Name = name;

                result = await _restaurantRepository.Update(result);

                return Ok(new { status = StatusCodes.Status200OK, message = "Poprawnie zaktualizowano nazwę", name = result.Name });

            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Database Failure!");
            }
        }

        /// <summary>
        /// Metoda UpdateAddress pozwala na zmiane adresu restauracji. Za pomocą repozytorium znajdowana jest restauracja i zmieniany jest jej adres.
        /// Odwołanie do API następuje po adresie "nazwahosta/api/Restaurants/[id restauracji]/UpdateAddress" metodą POST należy w body umieścić obiekt DTO klasy AddressViewModel oraz header Authorization z tokenem wygenerowanym poprzez logowanie
        /// </summary>
        /// <param name="addressViewModel">Obiekt DTO klasy AddressViewModel zawierający wymagane pola</param>
        /// <param name="id">Id restauracji</param>
        /// <returns>
        /// W przypadku błędu bazy danych: Database Failure! \n
        /// W przypadku błędu podczas zmieniana adresu:Status code 400 oraz wiadomość "Nie udało się zaktualizować adresu restauracji"\n
        /// W przypadku błednego id restauracji: Status Code 404 oraz wiadomość "Nie udało się znaleźć resturacji o takim ID"
        /// W przypadku powodzenia: Status Code 200 oraz obiekt klasy AddressViewModel reprezentujący zaktualizowane dane\n
        /// </returns>
        [HttpPost("{id:long}/[Action]")]
        [Authorize]
        public async Task<ActionResult<AddressViewModel>> UpdateAddress([FromBody]AddressViewModel addressViewModel, long id)
        {
            try
            {
                var restaurant = await _restaurantRepository.FindById(id);
                if (restaurant == null)
                    return NotFound(new { status = StatusCodes.Status404NotFound, message = "Nie udało się znaleźć resturacji o takim ID" });

                var restaurantAddressToUpdate = await _restaurantAddressRepository.FindById(restaurant.Address.Id);
                if (restaurantAddressToUpdate == null)
                    return NotFound(new { status = StatusCodes.Status404NotFound, message = "Nie udało się znaleźć adresu o takim ID" });

                restaurantAddressToUpdate.District = addressViewModel.District;
                restaurantAddressToUpdate.LocalNumber = addressViewModel.LocalNumber;
                restaurantAddressToUpdate.StreetName = addressViewModel.StreetName;
                restaurantAddressToUpdate.StreetNumber = addressViewModel.StreetNumber;
                restaurantAddressToUpdate.Location =  new Point(addressViewModel.LocationX,addressViewModel.LocationY){SRID = 4326};

                var result = await _restaurantAddressRepository.Update(restaurantAddressToUpdate);

                if (result == null)
                    return StatusCode(StatusCodes.Status500InternalServerError, new { status = StatusCodes.Status500InternalServerError, message = "Nie udało się zaktualizować adresu restauracji" });

                var newAddress = _mapper.Map<RestaurantAddress, AddressViewModel>(result);

                return Ok(new { status = StatusCodes.Status200OK, message = "Poprawnie zaktualizowano adres", newAddress });

            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Database Failure!");
            }
        }

        /// <summary>
        /// Metoda AllCuisines pozwala pobranie wszystkich dostępnych rodzajów kuchni.
        /// Odwołanie do API następuje po adresie "nazwahosta/api/Restaurants/AllCuisines" metodą GET
        /// </summary>
        /// <returns>
        /// W przypadku powodzenia: Tablicę stringow z nazwami typów kuchni
        /// W przypadku błędu bazy danych: Database Failure! \n
        /// </returns>
        [HttpGet("[action]")]
        public  async Task<ActionResult<string[]>> AllCuisines()
        {
            try
            {
                HashSet<string> cuisines = new HashSet<string>();

                foreach (string cuisine in Enum.GetNames(typeof(CuisineTypes.Cuisines)))
                {
                    cuisines.Add(cuisine);
                }

                return cuisines.ToArray();

            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Database Failure!");
            }
        }


        /// <summary>
        /// Metoda CreateRestaurant pozwala na utworzenie nowej restauracji. Za pomocą repozytorium tworzona jest nowa restauracja o podanych w formularzu danych.
        /// Odwołanie do API następuje po adresie "nazwahosta/api/Restaurants/" metodą PUT należy w body umieścić obiekt DTO klasy AddRestaurantViewModel oraz header Authorization z tokenem wygenerowanym poprzez logowanie
        /// </summary>
        /// <param name="addRestaurantViewModel">Obiekt klasy AddRestaurantViewModel zawierający dane do wstawienia</param>
        /// <returns>
        /// W przypadku błędu bazy danych: Database Failure! \n
        /// W przypadku błędu przy dodawaniu: Status code 400 oraz wiadomość "Nie udało się dodać restauracji"\n
        /// W przypadku powodzenia: Status code 200 oraz ID nowo dodanej resturacji
        /// </returns>
        [HttpPut]
        [Authorize]
        public async Task<IActionResult> CreateRestaurant(AddRestaurantViewModel addRestaurantViewModel)
        {
            try
            {
                var user = await _userRepository.FindUserByUsername(addRestaurantViewModel.Username);
                if (user == null)
                    return BadRequest(new{status = StatusCodes.Status400BadRequest, message = "Nie udało się dodać restauracji"});

                
                Restaurant newRestaurant = new Restaurant
                {
                    Name = addRestaurantViewModel.RestaurantName,
                    OwnerId = user.Id,
                    GotOwner = addRestaurantViewModel.GotOwner
                };

                var addedRestaurant = await _restaurantRepository.Create(newRestaurant);

                if (addedRestaurant == null)
                    return BadRequest(new{status = StatusCodes.Status400BadRequest, message = "Nie udało się dodać restauracji"});

                var newCuisine = new Cuisine
                {
                    Name = addRestaurantViewModel.Cuisine,
                    RestaurantId = addedRestaurant.Id
                };

                if (await _cuisineRepository.Create(newCuisine) == null)
                    return BadRequest(new{status = StatusCodes.Status400BadRequest, message = "Nie udało się dodać restauracji"});


                var newAddress = new RestaurantAddress
                {
                    StreetName = addRestaurantViewModel.Address.StreetName,
                    District = addRestaurantViewModel.Address.District,
                    LocalNumber = addRestaurantViewModel.Address.LocalNumber,
                    StreetNumber = addRestaurantViewModel.Address.StreetNumber,
                    RestaurantId = addedRestaurant.Id,
                    Location = new Point(addRestaurantViewModel.Lng,addRestaurantViewModel.Lat){SRID = 4326}
                };

                var result = await _restaurantAddressRepository.Create(newAddress);
                if(result == null)
                    return BadRequest(new{status = StatusCodes.Status400BadRequest, message = "Nie udało się dodać restauracji"});

                return Ok(new {status = StatusCodes.Status200OK, message = "Dodano nową restauracje",id = newRestaurant.Id});
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new{status = StatusCodes.Status500InternalServerError, message = "Database Failure!"});
            }
        }

        /// <summary>
        /// Metoda DeleteRestaurant pozwala na usunięcie istniejącej restauracji przez administratora
        /// Odwołanie do API następuje po adresie "nazwahosta/api/Restaurants/" metodą DELETE należy umieścić header Authorization z tokenem wygenerowanym poprzez logowanie
        /// </summary>
        /// <param name="id">Id restauracji</param>
        /// <returns>
        /// W przypadku błędnego ID: Status code 404 i wiadomość "Restauracja o takim ID nie istnieje"
        /// W przypadku błedu podczas usuwania: Status Code 400 i wiadomość "Błąd podczas usuwania restauracji"
        /// W przypadku powodzenia: Status Code 200 oraz wiadomość "Usuwanie restauracji powiodło się"
        /// W przypadku błędy bazy danych: Wiadomość "Database Failure!"
        /// </returns>
        [HttpDelete]
        [Route("{id:long}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteRestaurant(long id)
        {
            try
            {
                var restaurant = await _restaurantRepository.FindById(id);
                if(restaurant == null)
                    return NotFound(new{status = StatusCodes.Status404NotFound, message = "Restauracja o takim ID nie istnieje"});

                var restaurantCuisine = await _cuisineRepository.FindById(restaurant.Cuisine.Id);
                if(restaurantCuisine == null)
                    return BadRequest(new{status = StatusCodes.Status400BadRequest, message = "Błąd podczas usuwania restauracji"});

                await _cuisineRepository.Delete(restaurantCuisine.Id);

                var restaurantAddress = await _restaurantAddressRepository.FindById(restaurant.Address.Id);
                if(restaurantAddress == null)
                    return BadRequest(new{status = StatusCodes.Status400BadRequest, message = "Błąd podczas usuwania restauracji"});

                await _restaurantAddressRepository.Delete(restaurantAddress.Id);

                var restaurantGallery = await _imageRepository.FindAllByRestaurantId(restaurant.Id);
                if(restaurantGallery == null)
                    return BadRequest(new{status = StatusCodes.Status400BadRequest, message = "Błąd podczas usuwania restauracji"});

                foreach (var image in restaurantGallery)
                {
                    await _imageRepository.Delete(image.Id);
                }

                var restaurantReviews = await _reviewRepository.FindAllByRestaurantId(restaurant.Id);
                if(restaurantReviews == null)
                    return BadRequest(new{status = StatusCodes.Status400BadRequest, message = "Błąd podczas usuwania restauracji"});

                foreach (var review in restaurantReviews)
                {
                    await _reviewRepository.Delete(review.Id);
                }

                var restaurantMenu = await _menuItemRepository.FindAllByRestaurantId(restaurant.Id);
                if(restaurantMenu == null)
                    return BadRequest(new{status = StatusCodes.Status400BadRequest, message = "Błąd podczas usuwania restauracji"});

                foreach (var menuItem in restaurantMenu)
                {
                    await _menuItemRepository.Delete(menuItem.Id);
                }

                await _restaurantRepository.Delete(id);

                return Ok(new{status = StatusCodes.Status200OK, message = "Usuwanie restauracji powiodło się"});
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new{status = StatusCodes.Status500InternalServerError, message = "Database Failure!"});
            }
        }
    }
}