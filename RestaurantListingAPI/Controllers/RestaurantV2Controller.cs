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
    [ApiVersion("2.0")]
    [Route("api/[controller]")]
    [ApiController]
    public class RestaurantsVController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<RestaurantsVController> _logger;
        private readonly IMapper _mapper;

        public RestaurantsVController(IUnitOfWork unitOfWork, ILogger<RestaurantsVController> logger, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet("GetAll")]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetRestaurants()
        {
            try
            {
                var restaurants = await _unitOfWork.Restaurants.GetAll();
                var results = _mapper.Map<IList<RestaurantDTO>>(restaurants);
                return Ok(results);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"ERROR IN METHOD {nameof(GetRestaurants)}");
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }
        }
    }
}
