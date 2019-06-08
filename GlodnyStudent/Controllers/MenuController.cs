using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using GlodnyStudent.Models.Domain;
using GlodnyStudent.Models.Repositories;
using GlodnyStudent.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GlodnyStudent.Controllers
{
    [Route("api/Restaurants/[controller]")]
    [Authorize]
    [ApiController]
    public class MenuController : ControllerBase
    {
        private readonly IMenuItemRepository _menuItemRepository;
        private readonly IRestaurantRepository _restaurantRepository;
        private readonly IMapper _mapper;

        public MenuController(IMenuItemRepository menuItemRepository,IRestaurantRepository restaurantRepository,IMapper mapper)
        {
            _menuItemRepository = menuItemRepository;
            _restaurantRepository = restaurantRepository;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<ActionResult<MenuViewModel>> CreateMenuItem (MenuItemViewModel menuItem)
        {
            try
            {
                var restaurant = await _restaurantRepository.FindById(menuItem.RestaurantId);
                if (restaurant == null)
                    return BadRequest("Podana restauracja nie istnieje");

                var result = await _menuItemRepository.Create(_mapper.Map<MenuItemViewModel, MenuItem>(menuItem));
                if (result == null)
                    return BadRequest("Dodawanie nie powiodło się");

                await UpdateHihgestPrice(restaurant);

                return _mapper.Map<MenuItem, MenuViewModel>(result);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Database Failure!");
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteMenuItem(long id)
        {
            try
            {
                var result = await _menuItemRepository.FindById(id);
                if (result == null)
                    return NotFound();

                var restaurant = await _restaurantRepository.FindById(result.RestaurantId);

                await _menuItemRepository.Delete(id);

                await UpdateHihgestPrice(restaurant);

                return Ok();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Database Failure!");
            }
        }

        private async Task UpdateHihgestPrice(Restaurant restaurant)
        {
            decimal maxPrice = restaurant.HighestPrice;
            foreach (var menuItemLoop in await _menuItemRepository.FindAllByRestaurantId(restaurant.Id))
            {
                if (menuItemLoop.Price > maxPrice)
                    maxPrice = menuItemLoop.Price;
            }

            restaurant.HighestPrice = maxPrice;
        }
    }
}