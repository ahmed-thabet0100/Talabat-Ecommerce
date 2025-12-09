using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Talabat.APIs.Dtos;
using Talabat.APIs.Errors;
using Talabat.APIs.Extensions;
using Talabat.Core.Entities.Identity;
using Talabat.Core.Repo.Contarct;
using Talabat.Repo.Identity;

namespace Talabat.APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Account : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IAuthService _authService;
        private readonly AppIdentityDbContext _identityDbContext;
        private readonly IMapper _mapper;

        public Account(UserManager<AppUser> userManager ,
            SignInManager<AppUser> signInManager ,
            IAuthService authService,
            AppIdentityDbContext IdentityDbContext,
            IMapper mapper)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _authService = authService;
            _identityDbContext = IdentityDbContext;
            _mapper = mapper;
        }

        [HttpPost("LogIn")]
        public async Task<ActionResult<UserDto>> LogIn(LogInDto model)
        { 
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user == null)
                    return Unauthorized(new ApiRespone(410));

            var check = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);
            if (!check.Succeeded)
                return Unauthorized(new ApiRespone(401));
            return Ok(new UserDto()
            {
                DisplayName = user.DisplayName,
                email = user.Email,
                Token = await _authService.CreateToken(user, _userManager)
            }); ;
        }


        [HttpPost("Register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto model)
        {
            if (IfEmailExist(model.Email).Result)
                return BadRequest(new ApiValidationErrors() { Errors = new List<string> { "this email already exist" } });;
            var user = new AppUser()
            {
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
                DisplayName = model.DisplayName,
                UserName = model.Email.Split('@')[0]
            };
            var result = await _userManager.CreateAsync(user ,model.Password);
            if (result.Succeeded is false) return BadRequest(new ApiRespone(400));

            return Ok(new UserDto()
            {
                DisplayName = user.DisplayName,
                email = user.Email,
                Token = await _authService.CreateToken(user, _userManager)
            });

        }


        [Authorize]
        [HttpGet]
        public async Task<ActionResult<UserDto>> getCurrentUser()
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var user = await _userManager.FindByEmailAsync(email);

            return Ok(new UserDto()
            {
                email = email,
                DisplayName = user.DisplayName,
                Token = await _authService.CreateToken(user, _userManager)
            });
        }


        [Authorize]
        [HttpPost("createAddress")]
        public async Task<ActionResult<AddressDTo>> CreateAddress(AddressDTo addressDto)
        {
            var address = _mapper.Map<AddressDTo, Address>(addressDto);
            var user = await _userManager.FindByEmailIncludeAddress(User);
            address.AppUserId = user.Id;
            user.Address = address;
            await _userManager.UpdateAsync(user);
            return Ok(addressDto);
        }


        [Authorize]
        [HttpGet("Address")]
        public async Task<ActionResult<AddressDTo>> GetAddress()
        {
            var user = await _userManager.FindByEmailIncludeAddress(User);
            return Ok(_mapper.Map<AddressDTo>(user.Address));
        }
        [HttpGet("emailexists")]
        public async Task<bool> IfEmailExist(string email)
        {
            return await _userManager.FindByEmailAsync(email) is not null;
        }
    }
}
