﻿using System;
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
    public class SearchController : ControllerBase
    {
        private readonly IRestaurantRepository _restaurantRepository;
        private readonly IMapper _mapper;

        public SearchController(IRestaurantRepository restaurantRepository,IMapper mapper)
        {
            _restaurantRepository = restaurantRepository;
            _mapper = mapper;
        }

        [HttpGet("{address}")]
        public async Task<ActionResult<RestaurantListViewModel[]>> Get(string address)
        {
            /**
            *<summary>  
            *Metoda Get na podstawie podanego adresu tworzy widok z listą restrauracji pod danym adresem.
            *</summary> 
            * 
            *<param name="address">String zawierający adres danej restauracji
            *</param>

            * 
            *<returns>
            *W przypadku powodzenia: widok z listą restauracji.\n
            *W przypadku błędu połącznia z bazą danych: Database Failure!\n
            *</returns>
            */
            try
            {
                var result = await _restaurantRepository.GetRestaurantsByStreet(address);

                if (result == null)
                    return new RestaurantListViewModel[] { };

                return Ok(_mapper.Map<RestaurantListViewModel[]>(result));
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Database Failure!");
            }
        }

        [HttpGet]
        public async Task<ActionResult<RestaurantListViewModel[]>> Filters(string address, int distance, int highestPrice, string cuisine)
        {
            /**
            *<summary>  
            *Metoda Filters pozwala użytkownikowi na filtrowania za pomocą wybranych przez niego parametrów w menu aplikacji. 
            *</summary> 
            * 
            *<param name="address">String zawierający adres danej restauracji
            *</param>
            *<param name="cuisine">String zawierający nazwę danego rodzaju jedzenia
            *</param>
            *<param name="highestPrice">liczba zawierająca maksymalną cenę szukanej potrawy
            *</param>
            *<param name="distance">Liczba zawierająca dystans liczony od podanego adresu
            *</param>     

            * 
            *<returns>
            *W przypadku powodzenia: widok listy z restauracjami w pobliżu podanej ulicy.  \n
            *W przypadku błędu połącznia z bazą danych: Database Failure!\n
            *</returns>
            */
            try
            {
                IOrderedEnumerable<Restaurant> filteredResult;

                var result = await _restaurantRepository.GetRestaurantsByStreet(address);

                if (cuisine.Equals("Wszystkie"))
                {
                    filteredResult = from r in result
                                     where r.HighestPrice <= highestPrice
                                     orderby r.HighestPrice
                                     select r;
                }
                else
                {
                    filteredResult = from r in result
                                     where r.HighestPrice <= highestPrice && r.Cuisine.Name == cuisine
                                     orderby r.HighestPrice
                                     select r;
                }


                if (filteredResult.Any())
                    return Ok(_mapper.Map<RestaurantListViewModel[]>(filteredResult));
                else
                    return new RestaurantListViewModel[] { };
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Database Failure!");

            }
        }

        [HttpGet("[action]/{street}")]
        public  async Task<ActionResult<CuisineViewModel>> Cuisines(string street)
        {
            /**
            *  <summary>  
            *Metoda Cuisines na podstawie nazwy przekazanej ulicy poszukuje restauracje w pobliżu. 
            *</summary> 
            *<param name="street">
            *String zawierający nazwę ulicy 
            *</param>
            *<returns>
            *W przypadku powodzenia: lista z restauracjami w pobliżu podanej ulicy. \n
            *W przypadku błędu połącznia z bazą danych: Database Failure!\n
            * 
            *</returns>
            */
            try
            {
                HashSet<string> cuisines = new HashSet<string>();
                decimal highestPriceOfRestaurants = 0M;
                cuisines.Add("Wszystkie");

                var restaurantsResult = await _restaurantRepository.GetRestaurantsByStreet(street);

                foreach (var restaurant in restaurantsResult)
                {
                    if (highestPriceOfRestaurants < restaurant.HighestPrice)
                        highestPriceOfRestaurants = restaurant.HighestPrice;

                    cuisines.Add(restaurant.Cuisine.Name);
                }

                if (highestPriceOfRestaurants == 0)
                    highestPriceOfRestaurants = 100M;

                var result = new CuisineViewModel
                {
                    CuisinesList = cuisines.ToList(),
                    HighestPrice = Math.Ceiling(highestPriceOfRestaurants)
                };

                return result;

            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Database Failure!");
            }
        }
    }
}