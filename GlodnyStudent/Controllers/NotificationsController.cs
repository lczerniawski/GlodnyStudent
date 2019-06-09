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
        {
            try
            {
                var result = await _notificationRepository.FindAll();
                if (result == null)
                    return NotFound("Brak powiadomień");

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
            try
            {
                var restaurant = await _restaurantRepository.FindById(restaurantId);
                if (restaurant == null)
                    return NotFound("Nie ma restauracji o takim ID");

                Notification newNotification = new Notification
                {
                    Content = "Restauracja " + restaurant.Name + " została zgłoszona przez jednego z użytkowników." +
                              Environment.NewLine + "Restauracja wymaga weryfikacji przez administratora.",
                    RestaurantId = restaurant.Id
                };

                var result = await _notificationRepository.Create(newNotification);
                if (result == null)
                    return BadRequest("Nie udało się wysłać zgłoszenia");

                return result;
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
            try
            {
                var review = await _reviewRepository.FindById(reviewId);
                if (review == null)
                    return NotFound("Nie ma komentarza o takim ID");

                Notification newNotification = new Notification
                {
                    Content = "Komentarz użytkownika " + review.User.Username + " o treści: " + review.Description +" została zgłoszony przez jednego z użytkowników." +
                              Environment.NewLine + "Komentarz wymaga weryfikacji przez administratora.",
                    RestaurantId = review.RestaurantId
                };

                var result = await _notificationRepository.Create(newNotification);
                if (result == null)
                    return BadRequest("Nie udało się wysłać zgłoszenia");

                return result;
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
            try
            {
                var notification = await _notificationRepository.FindById(id);
                if (notification == null)
                    return NotFound("Nie ma komentarza o takim ID");

                await _notificationRepository.Delete(id);
                
                return notification;
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Database Failure!");
            }
        }
    }
}