using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RestaurantListingAPI.DTO;
using RestaurantListingAPI.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RestaurantListingAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DishController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public DishController(ILogger<DishController> logger, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet("GetAllInclude")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetDishesInclude()
        {
            try
            {
                _logger.LogDebug("[DishController:GetDishesInclude] Started");
                var dishes = await _unitOfWork.Dishes.GetAll(include: q => q.Include(loc => loc.Restaurant));
                var mappedDishes = _mapper.Map<IList<DishDTO>>(dishes);
                _logger.LogDebug("[DishController:GetDishesInclude] Finished");
                return Ok(mappedDishes);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"ERROR IN METHOD {nameof(GetDishesInclude)}");
                return BadRequest(ex);
            }
        }

        [HttpGet("GetById/{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> GetDishById(int id)
        {
            try
            {
                _logger.LogDebug("[DishController:GetDishById] Started");
                var dishes = await _unitOfWork.Dishes.Get(
                    expression: q => q.Id == id,
                    include: q => q.Include(loc => loc.Restaurant));

                var mappedDishes = _mapper.Map<DishDTO>(dishes);
                _logger.LogDebug("[DishController:GetDishById] Finished");
                return Ok(mappedDishes);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"ERROR IN METHOD {nameof(GetDishById)}");
                return BadRequest(ex);
            }
        }
    }
}
