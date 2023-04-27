using BallastLane.Application.DTO;
using BallastLane.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BallastLane.API.Controllers;

[AllowAnonymous]
[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;

    public UsersController(IUserService userService)
    {
        _userService = userService;
    }

    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterUserModel userModel)
    {
        try
        {
            var user = await _userService.RegisterUserAsync(userModel);
            return Ok(new { Message = "User registration successful." });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { ex.Message });
        }
    }

}