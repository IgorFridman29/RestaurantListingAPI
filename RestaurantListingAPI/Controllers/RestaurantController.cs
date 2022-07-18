using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RestaurantListingAPI.DTO;
using RestaurantListingAPI.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantListingAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RestaurantsController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public RestaurantsController(ILogger<RestaurantsController> logger, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }


        [HttpGet("GetAll")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetRestaurants()
        {
            try
            {
                _logger.LogDebug("[RestaurantsController:GetRestaurants] Started");
                var restaurants = await _unitOfWork.Restaurants.GetAll(include: q => q.Include(rest => rest.Location).Include(rest => rest.Dishes));
                var mappedRestaurants = _mapper.Map<IList<RestaurantDTO>>(restaurants);
                _logger.LogDebug("[RestaurantsController:GetRestaurants] Finished");
                return Ok(mappedRestaurants);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"ERROR IN METHOD {nameof(GetRestaurants)}");
                return BadRequest(ex);
            }
        }

        [HttpGet("GetById/{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> GetRestaurantsById(int id)
        {
            try
            {
                _logger.LogDebug("[RestaurantsController:GetRestaurantsById] Started");
                var restaurants = await _unitOfWork.Restaurants.Get(
                    expression: q => q.Id == id,
                    include: q => q.Include(rest => rest.Location).Include(rest => rest.Dishes));

                var mappedRestaurants = _mapper.Map<RestaurantDTO>(restaurants);
                _logger.LogDebug("[RestaurantsController:GetRestaurantsById] Finished");
                return Ok(mappedRestaurants);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"ERROR IN METHOD {nameof(GetRestaurantsById)}");
                return BadRequest(ex);
            }
        }
    }
}
