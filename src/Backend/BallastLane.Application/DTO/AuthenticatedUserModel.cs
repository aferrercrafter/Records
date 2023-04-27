namespace BallastLane.Application.DTO;

public class AuthenticatedUserModel
{
    public int Id { get; set; }
    public string Email { get; set; }
    public string Token { get; set; }
}
