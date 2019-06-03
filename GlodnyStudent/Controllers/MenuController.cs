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
        private readonly IMapper _mapper;

        public MenuController(IMenuItemRepository menuItemRepository,IMapper mapper)
        {
            _menuItemRepository = menuItemRepository;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<ActionResult<MenuViewModel>> CreateMenuItem (MenuItemViewModel menuItem)
        {
            try
            {
                var result = await _menuItemRepository.Create(_mapper.Map<MenuItemViewModel, MenuItem>(menuItem));
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
                var result = _menuItemRepository.FindById(id);
                if (result == null)
                    return NotFound();

                await _menuItemRepository.Delete(id);

                return Ok();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Database Failure!");
            }
        }
    }
}