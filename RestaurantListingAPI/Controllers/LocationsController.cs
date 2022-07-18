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
    public class LocationsController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public LocationsController(ILogger<LocationsController> logger, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet("GetAll")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetLocations()
        {
            try
            {
                _logger.LogDebug("[LocationsController:GetLocations] Started");
                var locations = await _unitOfWork.Locations.GetAll();
                var mappedLocations = _mapper.Map<IList<LocationDTO>>(locations);
                _logger.LogDebug("[LocationsController:GetLocations] Finished");
                return Ok(mappedLocations);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"ERROR IN METHOD {nameof(GetLocations)}");
                return BadRequest(ex);
            }
        }

        [HttpGet("GetAllOrdered")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetSortedLocations()
        {
            try
            {
                _logger.LogDebug("[LocationsController:GetLocations] Started");
                var locations = await _unitOfWork.Locations.GetAll(orderBy: q => q.OrderByDescending(loc => loc.Id));
                var mappedLocations = _mapper.Map<IList<LocationDTO>>(locations);
                _logger.LogDebug("[LocationsController:GetLocations] Finished");
                return Ok(mappedLocations);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"ERROR IN METHOD {nameof(GetSortedLocations)}");
                return BadRequest(ex);
            }
        }

        [HttpGet("GetAllInclude")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetLocationsInclude()
        {
            try
            {
                _logger.LogDebug("[LocationsController:GetLocations] Started");
                var locations = await _unitOfWork.Locations.GetAll(include: q => q.Include(loc => loc.Restaurant));
                var mappedLocations = _mapper.Map<IList<LocationDTO>>(locations);
                _logger.LogDebug("[LocationsController:GetLocations] Finished");
                return Ok(mappedLocations);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"ERROR IN METHOD {nameof(GetLocationsInclude)}");
                return BadRequest(ex);
            }
        }

        [HttpGet("GetById/{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> GetLocationById(int id)
        {
            try
            {
                _logger.LogDebug("[LocationsController:GetLocations] Started");
                var locations = await _unitOfWork.Locations.Get(
                    expression: q => q.Id == id,
                    include: q => q.Include(loc => loc.Restaurant));

                var mappedLocations = _mapper.Map<LocationDTO>(locations);
                _logger.LogDebug("[LocationsController:GetLocations] Finished");
                return Ok(mappedLocations);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"ERROR IN METHOD {nameof(GetLocationById)}");
                return BadRequest(ex);
            }
        }
    }
}
