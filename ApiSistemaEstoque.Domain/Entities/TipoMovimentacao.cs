using System.ComponentModel.DataAnnotations;
using ApiSistemaEstoque.ApiSistemaEstoque.Domain.Enums;

namespace ApiSistemaEstoque.ApiSistemaEstoque.Domain.Entities;

public class TipoMovimentacao
{
    [Key]
    public int Codigo { get; private set; }

    public TipoBaseMovimentacao Tipo { get; private set; }

    public string Descricao { get; private set; } = null!;

    public string UsuarioCadastro { get; private set; } = null!;

    public DateTime CreatedAt { get; private set; }

    public DateTime UpdatedAt { get; private set; }

    public Status Inativo { get; private set; }

    protected TipoMovimentacao() { }

    public TipoMovimentacao(
        TipoBaseMovimentacao tipo,
        string usuarioCadastro)
    {
        Tipo = tipo;
        Descricao = tipo.ToString();
        UsuarioCadastro = usuarioCadastro;

        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;

        Inativo = Status.Ativo;
    }

    public void SetTipo(TipoBaseMovimentacao tipo)
    {
        Tipo = tipo;
        Descricao = tipo.ToString();
        UpdatedAt = DateTime.UtcNow;
    }

    public void SetInativar()
    {
        Inativo = Status.Inativo;
        UpdatedAt = DateTime.UtcNow;
    }

    public void SetReativar()
    {
        Inativo = Status.Ativo;
        UpdatedAt = DateTime.UtcNow;
    }
}
