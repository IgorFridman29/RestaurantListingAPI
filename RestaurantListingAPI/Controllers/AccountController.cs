using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RestaurantListingAPI.Data;
using RestaurantListingAPI.DTO;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantListingAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        private readonly UserManager<ApiUser> _userManager;

        public AccountController(ILogger<AccountController> logger, IMapper mapper, UserManager<ApiUser> userManager)
        {
            _logger = logger;
            _mapper = mapper;
            _userManager = userManager;
        }


        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserDTO registerUserDTO)
        {
            try
            {
                _logger.LogDebug("[AccountController:Register] Start");
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                ApiUser user = _mapper.Map<ApiUser>(registerUserDTO);
                
                user.UserName = registerUserDTO.Email;
                user.PhoneNumber = registerUserDTO.MobileNumber;

                IdentityResult resultCreate = await _userManager.CreateAsync(user, registerUserDTO.Password);

                IdentityResult resultRoles = await _userManager.AddToRolesAsync(user, registerUserDTO.Roles);

                if (!resultCreate.Succeeded || !resultRoles.Succeeded)
                {
                    foreach (var error in resultCreate.Errors.Concat(resultRoles.Errors))
                    {
                        ModelState.AddModelError(error.Code, error.Description);
                    }
                    return BadRequest(ModelState);
                }

                _logger.LogDebug("[AccountController:Register] Finished Succesfully");
                return StatusCode(StatusCodes.Status201Created, "❤ User Created with love");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"ERROR IN METHOD {nameof(Register)}");
                return BadRequest(ex);
            }
            
        }
    }
}
