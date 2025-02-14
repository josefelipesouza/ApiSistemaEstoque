using System.ComponentModel.DataAnnotations;
using ApiSistemaEstoque.ApiSistemaEstoque.Domain.Enums;

namespace ApiSistemaEstoque.ApiSistemaEstoque.Domain.Entities;

public class TipoMovimentacao
{
  [Key]
  public int? Codigo { get; set; }
  public IEnumerable<TipoBaseMovimentacao> Tipo { get; set; }
  public string? Descricao { get; set; }
  public int UsuarioCadastro { get; private set; }
  public DateTime CreatedAt { get; private set; }
  public DateTime updated_at { get; private set; }

  public TipoMovimentacao(IEnumerable<TipoBaseMovimentacao> tipo, string descricao, int usuarioCadastro)
  {
    Tipo = tipo;
    Descricao = descricao;
    UsuarioCadastro = usuarioCadastro;
    CreatedAt = DateTime.UtcNow;
  }

  public void SetTipo(IEnumerable<TipoBaseMovimentacao> tipo)
    {
        Tipo = tipo;
    }

    public void SetDescricao(string descricao)
    {
        Descricao = descricao;
    }

    public void SetDataAlteracao(DateTime dataAlteracao)
    {
        updated_at = dataAlteracao;
    }
}

