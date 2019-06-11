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
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GlodnyStudent.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
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
        public async Task<IActionResult> UploadFile(IFormFile file, long id)
        {
            try
            {
                if (file == null || file.Length == 0)
                    return BadRequest(new { status = StatusCodes.Status400BadRequest, message = "Plik który chcesz wysłać jest pusty" });

                var restaurant = await _restaurantRepository.FindById(id);

                if (restaurant == null)
                    return NotFound(new { status = StatusCodes.Status404NotFound, message = "Nie ma restauracji o takim ID" });


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
                    filePath = result.FilePath,
                    status = StatusCodes.Status200OK
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