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
        public async Task<ActionResult<MenuViewModel>> CreateMenuItem(MenuItemViewModel menuItem)
        {
            /**
            *  <summary>  
            *Metoda CreateMenuItem tworzy obiekt, który zotanie dodany do menu 
            *</summary> 
            * 
            *<param name="menuItem">  Obiekt klasy MenuItemViewModel jest to widok obiektu, który ma zostać dodany do menu.-
            *</param>

            *<returns>
            *W przypadku błędu bazy danych: Database Failure!                   \n
            *W przypadu braku istnienia danej restauracji: Podana restauracja nie istnieje
            *W przypadku błędu podczas dodawania pozycji do menu: Dodawanie nie powiodło się
            *W przypadku powodzenia: przesyła nową mapę menu.
            * 
            *</returns>
            */
            try
            {
                var restaurant = await _restaurantRepository.FindById(menuItem.RestaurantId);
                if (restaurant == null)
                    return NotFound(new { status = StatusCodes.Status404NotFound, message = "Nie udało się znaleźć restauracji o takim ID" });

                var result = await _menuItemRepository.Create(_mapper.Map<MenuItemViewModel, MenuItem>(menuItem));

                if (result == null)
                    return BadRequest(new { status = StatusCodes.Status400BadRequest, message = "Dodawanie nie powiodło się" });


                await UpdateHihgestPrice(restaurant);

                var newMenuItem = _mapper.Map<MenuItem, MenuViewModel>(result);

                return Ok(new { status = StatusCodes.Status200OK, message = "Dodawanie powiodło się", newMenuItem });

            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Database Failure!");
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteMenuItem(long id)
        {      
            /** *  <summary>  
            *Metoda DeleteMenuItem usuwa obiekt z menu
            *</summary> 
            * 
            *<param name="id">  id obiektu w menu
            *</param>

            *<returns>
            *W przypadku błędu bazy danych: Database Failure!                   \n
            *</returns>
            */
            try
            {
                var result = await _menuItemRepository.FindById(id);
                if (result == null)
                    return NotFound(new { status = StatusCodes.Status404NotFound, message = "Nie udało się znaleźć restauracji o takim ID" });


                var restaurant = await _restaurantRepository.FindById(result.RestaurantId);

                await _menuItemRepository.Delete(id);

                await UpdateHihgestPrice(restaurant);

                return Ok(new { status = StatusCodes.Status200OK, message = "Poprawnie usunięto danie z menu" });

            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Database Failure!");
            }
        }

        private async Task UpdateHihgestPrice(Restaurant restaurant)
        {

            /** *  <summary>  
            *Metoda UpdateHihgestPrice ustawia nową najwyższą cenę potrawy w danej restauracji 
            *</summary> 
            * 
            *<param name="restaurant">  Obiekt klasy Restaurant. 
            *</param>

            */

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