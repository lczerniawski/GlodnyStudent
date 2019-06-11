using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GlodnyStudent.Models.Domain;
using GlodnyStudent.Models.Repositories;
using GlodnyStudent.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GlodnyStudent.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationsController : ControllerBase
    {
        private readonly INotificationRepository _notificationRepository;
        private readonly IRestaurantRepository _restaurantRepository;
        private readonly IReviewRepository _reviewRepository;

        public NotificationsController(INotificationRepository notificationRepository,IRestaurantRepository restaurantRepository,IReviewRepository reviewRepository)
        {
            _notificationRepository = notificationRepository;
            _restaurantRepository = restaurantRepository;
            _reviewRepository = reviewRepository;
        }
        
        /// <summary>
        /// Metoda GetAll działa tylko jeśli użytkownik jest administratorem. Pobiera wszystkie zgłoszenia dotyczące restrauracji i komentarzy
        /// Odwołanie do API następuje po adresie "nazwahosta/api/Notifications" metodą Get w żądaniu należy umieścić header Authorization z tokenem wygenerowanym poprzez logowanie
        /// </summary>
        /// <returns>
        /// W przypadku powodzenia: tablica powiadomień zgłoszonych restauracji i komentarzy.
        /// W przypadku błędu połącznia z bazą danych: Database Failure!\n
        /// </returns>
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Notification[]>> GetAll()
        {            
            try
            {
                var result = await _notificationRepository.FindAll();
                if (result == null)
                    return new Notification[]{};

                return result.ToArray();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Database Failure!");
            }
        }

        /// <summary>
        /// Metoda CreateReportRestaurant pozwala dowolnemu zalogowanemu użytkownikowi zgłosić restraurację w związku z występującymi w niej nieprawidłowościami
        /// Odwołanie do API następuje po adresie "nazwahosta/api/Notifications/CreateReportRestaurant" metodą Post w żądaniu należy umieścić ID restauracji której ma dotyczyć zgłoszenie
        /// </summary>
        /// <param name="restaurantId">Id restauracji</param>
        /// <returns>
        /// W przypadku powodzenia: Status Code 200 oraz wiadomość o wysłąniu zgłoszenia.
        /// W przypadku błędu zgłaszania: Status Code 400 oraz wiadomość "Nie udało się wysłać zgłoszenia"\n
        /// W przypadku nie znalezienia restuaracji której zgłoszenie dotyczy: Status Code 404 oraz wiadomość "Nie ma restauracji o takim ID"\n
        /// W przypadku błędu połączenia z bazą danych: Database Failure!\n
        /// </returns>
        [HttpPost("[action]")]
        [Authorize]
        public async Task<ActionResult<Notification>> CreateReportRestaurant([FromBody] long restaurantId)
        {
            try
            {
                var restaurant = await _restaurantRepository.FindById(restaurantId);
                if (restaurant == null)
                    return NotFound(new { status = StatusCodes.Status404NotFound, message = "Nie ma restauracji o takim ID" });

                Notification newNotification = new Notification
                {
                    Content = "Restauracja " + restaurant.Name + " została zgłoszona przez jednego z użytkowników." +
                              Environment.NewLine + "Restauracja wymaga weryfikacji przez administratora.",
                    RestaurantId = restaurant.Id
                };

                var result = await _notificationRepository.Create(newNotification);
                if (result == null)
                    return BadRequest(new { status = StatusCodes.Status400BadRequest, message = "Nie udało się wysłać zgłoszenia" });

                return Ok(new { status = StatusCodes.Status200OK, message = "Wysłano zgłoszenie" });
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Database Failure!");
            }
        }


        /// <summary>
        /// Metoda CreateReportRestaurant pozwala dowolnemu zalogowanemu użytkownikowi zgłosić opinię w związku z występującymi w niej nieprawidłowościami
        /// Odwołanie do API następuje po adresie "nazwahosta/api/Notifications/CreateReportRestaurant" metodą Post w żądaniu należy umieścić ID restauracji której ma dotyczyć zgłoszenie
        /// </summary>
        /// <param name="restaurantId">Id opinii</param>
        /// <returns>
        /// W przypadku powodzenia: Status Code 200 oraz wiadomość o wysłąniu zgłoszenia.
        /// W przypadku błędu zgłaszania: Status Code 400 oraz wiadomość "Nie udało się wysłać zgłoszenia"\n
        /// W przypadku nie znalezienia restuaracji której zgłoszenie dotyczy: Status Code 404 oraz wiadomość "Nie ma restauracji o takim ID"\n
        /// W przypadku błędu połączenia z bazą danych: Database Failure!\n
        /// </returns>
        [HttpPost("[action]")]
        [Authorize]
        public async Task<ActionResult<Notification>> CreateReportReview([FromBody] long reviewId)
        {
            try
            {
                var review = await _reviewRepository.FindById(reviewId);
                if (review == null)
                    return NotFound(new { status = StatusCodes.Status404NotFound, message = "Nie ma komentarza o takim ID" });


                Notification newNotification = new Notification
                {
                    Content = "Komentarz użytkownika " + review.User.Username + " o treści: " + review.Description + " została zgłoszony przez jednego z użytkowników." +
                              Environment.NewLine + "Komentarz wymaga weryfikacji przez administratora.",
                    RestaurantId = review.RestaurantId
                };

                var notification = await _notificationRepository.Create(newNotification);
                if (notification == null)
                    return BadRequest(new { status = StatusCodes.Status400BadRequest, message = "Nie udało się wysłać zgłoszenia" });


                return Ok(new { status = StatusCodes.Status200OK, message = "Wysłano zgłoszenie" });

            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Database Failure!");
            }
        }

        /// <summary>
        /// Metoda DeleteNotification pozwala administratorowi usunąć powiadomienie.
        /// Odwołanie do API następuje po adresie "nazwahosta/api/Notifications/" metodą Delete w QueryStringu żądania należy umieścić ID oraz header Authorization z tokenem wygenerowanym poprzez logowanie
        /// </summary>
        /// <param name="id">Id powiadomienia</param>
        /// <returns>
        /// W przypadku nie znalezienia komentarza o podanym ID: Status code 404 oraz wiadomość "Nie ma komentarza o takim ID"\n
        /// W przypadku błędu połącznia z bazą danych: Database Failure!\n
        /// </returns>
        [HttpDelete]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Notification>> DeleteNotification([FromQuery] long id)
        {
            try
            {
                var notification = await _notificationRepository.FindById(id);
                if (notification == null)
                    return NotFound(new{status = StatusCodes.Status404NotFound, message = "Nie ma komentarza o takim ID"});


                await _notificationRepository.Delete(id);
                
                return Ok(new{status = StatusCodes.Status200OK,notification});
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Database Failure!");
            }
        }
    }
}