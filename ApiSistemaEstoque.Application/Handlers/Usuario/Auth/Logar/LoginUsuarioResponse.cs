public class LoginUsuarioResponse
{
    public string UserId { get; }
    public string Email { get; }
    public string Token { get; } 

    public LoginUsuarioResponse(string userId, string email, string tokenString)
    {
        UserId = userId;
        Email = email;
        Token = tokenString;
    }
}
