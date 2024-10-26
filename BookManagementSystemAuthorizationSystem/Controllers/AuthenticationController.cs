using AutoMapper;
using BookManagementSystemAuthorizationSystem.Contracts;
using Entities;
using Entities.DataTransferObjects;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BookManagementSystemAuthorizationSystem.Controllers;

[Route("api/authentication")]
[ApiController]
public class AuthenticationController : Controller
{
    private readonly IMapper _mapper;
    private readonly UserManager<User> _userManager;
    private readonly IAuthenticationManager _authManager;
    public AuthenticationController (IMapper mapper, UserManager<User> userManager, IAuthenticationManager authManager)
    {
        _mapper = mapper;
        _userManager = userManager;
        _authManager = authManager;
    }

    [HttpGet("registerPage")]
    public IActionResult RegisterPage()
    {
        return View("~/Views/Auth/RegisterPage.cshtml");
    }
    
    [HttpGet("loginPage")]
    public IActionResult LoginPage()
    {
        return View("~/Views/Auth/LoginPage.cshtml");
    }
    
    [HttpPost("register")]
    public async Task<IActionResult> RegisterUser([FromBody] UserForRegistrationDto userForRegistration)
    {
        var user = _mapper.Map<User>(userForRegistration);
        var result = await _userManager.CreateAsync(user, userForRegistration.Password);
        if (!result.Succeeded)
        {
            foreach (var error in result.Errors)
            {
                ModelState.TryAddModelError(error.Code, error.Description);
            }
            return BadRequest(ModelState);
        }
        var userRoleAsString = userForRegistration.Role.ToString();
        await _userManager.AddToRolesAsync(user, new List<string> { userRoleAsString });
        return StatusCode(201);
    }
    
    [HttpPost("login")]
    public async Task<IActionResult> Authenticate([FromBody] UserForAuthenticationDto user)
    {
        if (!await _authManager.ValidateUser(user))
        {
            return Unauthorized();
        }
        return Ok();
    }
}
