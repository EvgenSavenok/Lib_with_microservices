using Entities.DataTransferObjects;

namespace BookManagementSystemAuthorizationSystem.Contracts;

public interface IAuthenticationManager
{
    Task<bool> ValidateUser(UserForAuthenticationDto userForAuth);
}
