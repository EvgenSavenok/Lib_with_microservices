using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BookManagementSystemAuthorizationSystem.Contracts;
using Entities;
using Entities.DataTransferObjects;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Repository;

public class AuthenticationManager : IAuthenticationManager
{
    private readonly UserManager<User> _userManager;
    private readonly IConfiguration _configuration;

    private User _user;

    public AuthenticationManager(UserManager<User> userManager, IConfiguration configuration)
    {
        _userManager = userManager;
        _configuration = configuration;
    }

    public async Task<bool> ValidateUser(UserForAuthenticationDto userForAuth)
    {
        _user = await _userManager.FindByNameAsync(userForAuth.UserName);
        return (_user != null && await _userManager.CheckPasswordAsync(_user,
            userForAuth.Password));
    }
}
