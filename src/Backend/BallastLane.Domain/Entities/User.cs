namespace BallastLane.Domain.Entities;

public class User
{
    public int Id { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public byte[] PasswordHash { get; set; }
    public byte[] PasswordSalt { get; set; }

    public void Validate()
    {
        if (string.IsNullOrEmpty(Email))
        {
            throw new Exception("User email cannot be empty.");
        }

        if (!IsValidEmail(Email))
        {
            throw new Exception("Invalid email address.");
        }
    }

    private bool IsValidEmail(string email)
    {
        try
        {
            var addr = new System.Net.Mail.MailAddress(email);
            return addr.Address == email;
        }
        catch
        {
            return false;
        }
    }
}
