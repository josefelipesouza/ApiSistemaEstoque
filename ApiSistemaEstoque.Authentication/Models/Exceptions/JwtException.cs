namespace ApiSistemaEstoque.ApiSistemaEstoque.Authentication.Models.exceptions;

public class JwtException : Exception
{
    public JwtException()
    {
    }

    public JwtException(string mensagem) : base(mensagem)
    {
    }

    public JwtException(string mensagem, Exception exception) : base(mensagem, exception)
    {
    }
}