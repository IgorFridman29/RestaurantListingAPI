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
                _logger.LogError(ex, $"ERROR IN METHOD {nameof(GetLocations)}");
                return BadRequest(ex);
            }
        }

        [HttpGet("GetAllInclude")]
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
                _logger.LogError(ex, $"ERROR IN METHOD {nameof(GetLocations)}");
                return BadRequest(ex);
            }
        }
    }
}
