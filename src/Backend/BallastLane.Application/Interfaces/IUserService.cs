using BallastLane.Application.DTO;
using BallastLane.Domain.Entities;

namespace BallastLane.Application.Interfaces;

public interface IUserService
{
    Task<User> RegisterUserAsync(RegisterUserModel userMode);
    Task<AuthenticatedUserModel> AuthenticateAsync(LoginRequestModel request);
}
