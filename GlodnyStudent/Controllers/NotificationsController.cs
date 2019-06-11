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

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Notification[]>> GetAll()
        {            /**
            *<summary>  
            *Metoda GetAll działa tylko jeśli użytkownik jest administratorem. Pobiera wszystkie zgłoszone restrauracje i komentarze
            *</summary> 
            *
            *<returns>
            *W przypadku powodzenia: tablica powiadomień zgłoszonych restauracji i komentarzy.
            *W przypadku błędu połącznia z bazą danych: Database Failure!\n
            *</returns>
            */
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

        [HttpPost("[action]")]
        [Authorize]
        public async Task<ActionResult<Notification>> CreateReportRestaurant([FromBody] long restaurantId)
        {
            /**
            *<summary>  
            *Metoda CreateReportRestaurant pozwala dowolnemu zalogowanemu użytkownikowi zgłosić restraurację. 
            *</summary> 
            *
            *<param name="id">  id restauracji
            *</param>
            *<returns>
            *W przypadku powodzenia: "Restrauacja [nazwa] została zgłoszona przez jednego z użytkowników.\n
            *Restauracja wymaga weryfikacji przez administratora."\n
            *W przypadku błędu zgłaszania: Nie udało się wysłać zgłoszenia\n
            *W przypadku błędu połącznia z bazą danych: Database Failure!\n
            *</returns>
            */
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

        [HttpPost("[action]")]
        [Authorize]
        public async Task<ActionResult<Notification>> CreateReportReview([FromBody] long reviewId)
        {
            /**
            *<summary>  
            *Metoda CreateReportReview pozwala dowolnemu zalogowanemu użytkownikowi zgłosić opinię. 
            *</summary> 
            *
            *<param name="id">  id opinii
            *</param>
            *<returns>
            *W przypadku powodzenia: "Komentarz użytkownika  [nazwa użytkownika] " o treści: [treść] została zgłoszony przez jednego z użytkowników."\n
            *Komentarz wymaga weryfikacji przez administratora.\n
            *Restauracja wymaga weryfikacji przez administratora."\n
            *W przypadku błędu zgłaszania: Nie udało się wysłać zgłoszenia\n
            *W przypadku błędu połącznia z bazą danych: Database Failure!\n
            *</returns>
            */
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

        [HttpDelete]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Notification>> DeleteNotification([FromQuery] long id)
        {
            /**
            *<summary>  
            *Metoda DeleteNotification pozwala administratorowi usunąć powiadomienie.
            *</summary> 
            *
            *<param name="id">  id powiadomienia
            *</param>
            *<returns>
            *Restauracja wymaga weryfikacji przez administratora."\n
            *W przypadku błędu podczas usuwania: Nie ma komentarza o takim ID\n
            *W przypadku błędu połącznia z bazą danych: Database Failure!\n
            *</returns>
            */
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