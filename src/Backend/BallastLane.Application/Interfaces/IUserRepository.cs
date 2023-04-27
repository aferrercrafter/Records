using BallastLane.Domain.Entities;

namespace BallastLane.Application.Interfaces;

public interface IUserRepository
{
    Task<User> CreateUserAsync(User user);
    Task<User> GetUserByUsernameAsync(string username);
    Task<User> AuthenticateAsync(string email, string password);
}
