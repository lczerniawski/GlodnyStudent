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

        /// <summary>
        /// Metoda CreateMenuItem tworzy nowa danie w menu resturacji i zapisuje je w bazie danych.
        /// Odwołanie do API następuje po adresie "nazwahosta/api/Restaurants/Menu" metodą POST w żądaniu należy zawżeć obiekt DTO MenuItemViewModel opisany odpowiednimi polami oraz header Authorization z tokenem wygenerowanym poprzez logowanie
        /// </summary>
        /// <param name="menuItem">Obiekt klasy MenuItemViewModel czyli DTO zawierające pola wymagane aby dodać nowe danie</param>
        /// <returns>
        /// W przypadku błędu bazy danych: Database Failure!                   \n
        /// W przypadu braku istnienia danej restauracji: Staus Code 404 oraz wiadomość "Nie udało się znaleźć restauracji o takim ID"
        /// W przypadku błędu podczas dodawania pozycji do menu: Staus Code 404 oraz wiadomość "Dodawanie nie powiodło się"
        /// W przypadku powodzenia: Status Code 200 oraz nowo dodany obiekt dania w celu wyswietlenia w aplikacji.
        /// </returns>
        [HttpPost]
        public async Task<ActionResult<MenuViewModel>> CreateMenuItem(MenuItemViewModel menuItem)
        {
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

        /// <summary>
        /// Metoda DeleteMenuItem usuwa obiekt z menu restauracji
        /// Odwołanie do API następuje po adresie "nazwahosta/api/Restaurants/Menu/[id dania]" metodą DELETE oraz header Authorization z tokenem wygenerowanym poprzez logowanie
        /// </summary>
        /// <param name="id">Id dania</param>
        /// <returns>
        /// W przypadku błędu bazy danych: Database Failure!                   \n
        /// W przypadu braku istnienia danej restauracji: Staus Code 404 oraz wiadomość "Nie udało się znaleźć restauracji o takim ID"
        /// W przypadku powodzenia: Status Code 200 oraz wiadomość "Poprawnie usunięto danie z menu".
        /// </returns>
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteMenuItem(long id)
        {
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

        /// <summary>
        /// Metoda która aktualizuję największą cene w resutaracji, pole które wykorzysystywane jest podczas stosowania filtrów w wyszukiwaniu 
        /// </summary>
        /// <param name="restaurant">Obiekt klasy Restaurant w którym chcemy zaktualizować cenę</param>
        /// <returns></returns>
        private async Task UpdateHihgestPrice(Restaurant restaurant)
        {
            decimal maxPrice = restaurant.HighestPrice;
            foreach (var menuItemLoop in await _menuItemRepository.FindAllByRestaurantId(restaurant.Id))
            {
                if (menuItemLoop.Price > maxPrice)
                    maxPrice = menuItemLoop.Price;
            }

            restaurant.HighestPrice = maxPrice;

            await _restaurantRepository.Update(restaurant);
        }
    }
}