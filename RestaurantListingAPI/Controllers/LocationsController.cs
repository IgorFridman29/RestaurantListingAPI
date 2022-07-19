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
            _logger.LogDebug("[LocationsController:GetLocations] Started");
            var locations = await _unitOfWork.Locations.GetAll();
            var mappedLocations = _mapper.Map<IList<LocationDTO>>(locations);
            _logger.LogDebug("[LocationsController:GetLocations] Finished");
            return Ok(mappedLocations);
        }

        [HttpGet("GetAllOrdered")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetSortedLocations()
        {
            _logger.LogDebug("[LocationsController:GetLocations] Started");
            var locations = await _unitOfWork.Locations.GetAll(orderBy: q => q.OrderByDescending(loc => loc.Id));
            var mappedLocations = _mapper.Map<IList<LocationDTO>>(locations);
            _logger.LogDebug("[LocationsController:GetLocations] Finished");
            return Ok(mappedLocations);
        }

        [HttpGet("GetAllInclude")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetLocationsInclude()
        {
            _logger.LogDebug("[LocationsController:GetLocations] Started");
            var locations = await _unitOfWork.Locations.GetAll(include: q => q.Include(loc => loc.Restaurant));
            var mappedLocations = _mapper.Map<IList<LocationDTO>>(locations);
            _logger.LogDebug("[LocationsController:GetLocations] Finished");
            return Ok(mappedLocations);
        }

        [HttpGet("GetById/{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> GetLocationById(int id)
        {
            _logger.LogDebug("[LocationsController:GetLocations] Started");
            var locations = await _unitOfWork.Locations.Get(
                expression: q => q.Id == id,
                include: q => q.Include(loc => loc.Restaurant));

            var mappedLocations = _mapper.Map<LocationDTO>(locations);
            _logger.LogDebug("[LocationsController:GetLocations] Finished");
            return Ok(mappedLocations);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]

        public async Task<IActionResult> CreateLocation([FromBody] CreateLocationDTO locationDTO)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogError($"Invalid POST attempt in {nameof(CreateLocation)}");
                return BadRequest(ModelState);
            }

            var location = _mapper.Map<Location>(locationDTO);
            await _unitOfWork.Locations.Insert(location);
            await _unitOfWork.Save();

            return CreatedAtRoute("GetLocation", new { id = location.Id }, location);

        }


        // [Authorize]
        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateLocation(int id, [FromBody] UpdateLocationDTO locationDTO)
        {
            if (!ModelState.IsValid || id <= 0)
            {
                _logger.LogError($"Invalid UPDATE attempt in {nameof(UpdateLocation)}");
                return BadRequest(ModelState);
            }

            var location = await _unitOfWork.Locations.Get(q => q.Id == id);
            if (location == null)
            {
                _logger.LogError($"Invalid UPDATE attempt in {nameof(UpdateLocation)}");
                return BadRequest("Submitted data is invalid");
            }

            _mapper.Map(locationDTO, location);
            _unitOfWork.Locations.Update(location);
            await _unitOfWork.Save();

            return NoContent();

        }


        //[Authorize]
        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteLocation(int id)
        {
            if (id <= 0)
            {
                _logger.LogError($"Invalid DELETE attempt in {nameof(DeleteLocation)}");
                return BadRequest();
            }

            var location = await _unitOfWork.Locations.Get(q => q.Id == id);
            if (location == null)
            {
                _logger.LogError($"Invalid DELETE attempt in {nameof(DeleteLocation)}");
                return BadRequest("Submitted data is invalid");
            }

            await _unitOfWork.Locations.Delete(id);
            await _unitOfWork.Save();

            return StatusCode(StatusCodes.Status202Accepted, location);

        }
    }
}