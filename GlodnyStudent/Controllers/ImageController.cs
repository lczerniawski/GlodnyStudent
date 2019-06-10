using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using GlodnyStudent.Models.Domain;
using GlodnyStudent.Models.Repositories;
using GlodnyStudent.Models.Repositories.Implementations;
using GlodnyStudent.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GlodnyStudent.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImageController : ControllerBase
    {
        private readonly IImageRepository _imageRepository;
        private readonly IRestaurantRepository _restaurantRepository;

        public ImageController(IImageRepository imageRepository,IRestaurantRepository restaurantRepository)
        {
            _imageRepository = imageRepository;
            _restaurantRepository = restaurantRepository;
        }

        /*
        public async Task<IActionResult> Upload(IFormFile model)
        {
            var file = model;

            if (file.Length > 0) {
                string path = Path.Combine(_hostingEnvironment.WebRootPath, "uploadFiles");
                using (var fs = new FileStream(Path.Combine(path, file.FileName), FileMode.Create))
                {
                    await file.CopyToAsync(fs);
                }

                //model.Source = $"/uploadFiles{file.FileName}";
                //model.Extension = Path.GetExtension(file.FileName).Substring(1);
            }
            return BadRequest();
        }
        */

        [HttpPost]
        [Route("{id:long}/Upload")]
        public async Task<IActionResult> UploadFile(IFormFile file,long id)
        {
            /**
            *  <summary>  
            *Metoda UploadFile zapisuje przesłany obrazek w pamięci aplikacji
            *</summary> 
            * 
            *<param name="file"> plik obrazu
            *</param>
            *<param name="id"> id obrazu
            *</param>
            *<returns>
            *W przypadku błędu bazy danych: Database Failure!                   \n
            *W przypadku powodzenia: Zapisuje obraz w bazie danych
            * 
            *</returns>
            */
            try
            {
                if (file == null || file.Length == 0)
                    return BadRequest();

                var restaurant = await _restaurantRepository.FindById(id);

                if (restaurant == null)
                    return NotFound();

                string path = Path.Combine(Directory.GetCurrentDirectory(), "UploadImages");
                using (var fs = new FileStream(Path.Combine(path, file.FileName), FileMode.Create))
                {
                    await file.CopyToAsync(fs);
                }

                string filePath = Url.Content($"/UploadImages/{file.FileName}");

                var result = await _imageRepository.Create(new Image
                {
                    RestaurantId = restaurant.Id,
                    FilePath = filePath
                });

                return Ok(new
                {
                    id = result.Id,
                    filePath = result.FilePath
                });
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Database Failure!");
            }
        }

        [HttpDelete]
        [Route("Delete/{id:long}")]
        public async Task<IActionResult> DeleteFile(long id)
        {
            /**
            *  <summary>  
            *Metoda DeleteFile usuwa  obaz o podanym ID.
            *</summary> 
            * 
            *<param name="id"> id obrazu
            *</param>
            *<returns>
            *W przypadku błędu bazy danych: Database Failure! \n
            *W przypadku powodzenia: Usuwa obraz z bazy danych.
            * 
            *</returns>
            */
            try
            {
                var file = await _imageRepository.FindById(id);
                if (file == null)
                    return NotFound();

                string path = Url.Content($"{Directory.GetCurrentDirectory()}{file.FilePath}");
                System.IO.File.Delete(path);

                await _imageRepository.Delete(id);

                return Ok();
            }

            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Database Failure!");
            }
        }
    }
}