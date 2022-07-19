using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RestaurantListingAPI.Data;
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

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]

        public async Task<IActionResult> CreateRestaurant([FromBody] CreateRestaurantDTO restaurantDTO)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogError($"Invalid POST attempt in {nameof(CreateRestaurant)}");
                return BadRequest(ModelState);
            }

            var restaurant = _mapper.Map<Restaurant>(restaurantDTO);
            await _unitOfWork.Restaurants.Insert(restaurant);
            await _unitOfWork.Save();

            return CreatedAtRoute("GetRestaurant", new { id = restaurant.Id }, restaurant);

        }

        // [Authorize]
        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateRestaurant(int id, [FromBody] UpdateRestaurantDTO restaurantDTO)
        {
            if (!ModelState.IsValid || id <= 0)
            {
                _logger.LogError($"Invalid UPDATE attempt in {nameof(UpdateRestaurant)}");
                return BadRequest(ModelState);
            }

            var restaurant = await _unitOfWork.Restaurants.Get(q => q.Id == id);
            if (restaurant == null)
            {
                _logger.LogError($"Invalid UPDATE attempt in {nameof(UpdateRestaurant)}");
                return BadRequest("Submitted data is invalid");
            }

            _mapper.Map(restaurantDTO, restaurant);
            _unitOfWork.Restaurants.Update(restaurant);
            await _unitOfWork.Save();

            return NoContent();

        }


        //[Authorize]
        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteRestaurant(int id)
        {
            if (id <= 0)
            {
                _logger.LogError($"Invalid DELETE attempt in {nameof(DeleteRestaurant)}");
                return BadRequest();
            }

            var restaurant = await _unitOfWork.Restaurants.Get(q => q.Id == id);
            if (restaurant == null)
            {
                _logger.LogError($"Invalid DELETE attempt in {nameof(DeleteRestaurant)}");
                return BadRequest("Submitted data is invalid");
            }

            await _unitOfWork.Restaurants.Delete(id);
            await _unitOfWork.Save();

            return StatusCode(StatusCodes.Status202Accepted, restaurant);

        }
    }
}
