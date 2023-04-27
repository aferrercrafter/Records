using BallastLane.Application.DTO;
using BallastLane.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BallastLane.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthenticationController : ControllerBase
{
    private readonly IUserService _userService;

    public AuthenticationController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost("login")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AuthenticatedUserModel))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(string))]
    public async Task<IActionResult> Login([FromBody] LoginRequestModel request)
    {
        var result = await _userService.AuthenticateAsync(request);

        if (result == null)
        {
            return Unauthorized("Invalid credentials");
        }

        return Ok(result);
    }
}
