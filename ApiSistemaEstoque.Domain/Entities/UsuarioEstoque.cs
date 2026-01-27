using System.ComponentModel.DataAnnotations;

namespace ApiSistemaEstoque.ApiSistemaEstoque.Domain.Entities;

public class UsuarioEstoque
{
    [Key]
    public int Codigo { get; private set; }

    [Required]
    public string CodigoUsuario { get; private set; }

    [Required]
    public int CodigoEstoque { get; private set; }

    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    // EF
    protected UsuarioEstoque() { }

    public UsuarioEstoque(string codigoUsuario, int codigoEstoque)
    {
        CodigoUsuario = codigoUsuario;
        CodigoEstoque = codigoEstoque;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }
}
