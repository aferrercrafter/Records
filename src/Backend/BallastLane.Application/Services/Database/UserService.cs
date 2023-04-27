using BallastLane.Application.DTO;
using BallastLane.Application.Interfaces;
using BallastLane.Domain.Entities;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BallastLane.Application.Services.Database;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IConfiguration _configuration;
    private readonly IValidator<User> _validator;

    public UserService(IUserRepository repository, IConfiguration configuration, IValidator<User> validator)
    {
        _userRepository = repository;
        _configuration = configuration;
        _validator = validator;
    }

    public async Task<User> RegisterUserAsync(RegisterUserModel userModel)
    {
        if (await _userRepository.GetUserByUsernameAsync(userModel.Username) != null)
            throw new ArgumentException("Username already exists.");

        byte[] passwordHash, passwordSalt;
        CreatePasswordHash(userModel.Password, out passwordHash, out passwordSalt);

        var user = new User
        {
            Username = userModel.Username,
            Email = userModel.Email,
            PasswordHash = passwordHash,
            PasswordSalt = passwordSalt
        };

        var validationResult = _validator.Validate(user);

        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }

        return await _userRepository.CreateUserAsync(user);
    }

    public async Task<AuthenticatedUserModel> AuthenticateAsync(LoginRequestModel request)
    {
        var user = await _userRepository.AuthenticateAsync(request.Email, request.Password);

        if (user == null)
        {
            return null;
        }

        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_configuration["JwtKey"]);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[] { new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()) }),
            Expires = DateTime.UtcNow.AddDays(7),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);

        return new AuthenticatedUserModel
        {
            Id = user.Id,
            Email = user.Email,
            Token = tokenHandler.WriteToken(token)
        };
    }

    private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
    {
        using (var hmac = new System.Security.Cryptography.HMACSHA512())
        {
            passwordSalt = hmac.Key;
            passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
        }
    }

    // Other methods
}
