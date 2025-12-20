using System.ComponentModel.DataAnnotations;
using ApiSistemaEstoque.ApiSistemaEstoque.Domain.Enums;

namespace ApiSistemaEstoque.ApiSistemaEstoque.Domain.Entities;

public class TipoMovimentacao
{
  [Key]
  public int Codigo { get; set; }
  public TipoBaseMovimentacao Tipo { get; set; }
  public string Descricao { get; set; }
  public string UsuarioCadastro { get; private set; }
  public DateTime CreatedAt { get; private set; }
  public DateTime updated_at { get; private set; }
  public Status Inativo { get; private set; }

  public TipoMovimentacao(TipoBaseMovimentacao tipo, string descricao, string usuarioCadastro)
  {
    Tipo = tipo;
    Descricao = descricao;
    UsuarioCadastro = usuarioCadastro;
    CreatedAt = DateTime.UtcNow;
  }

  public void SetTipo(TipoBaseMovimentacao tipo)
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

  public void SetInativar()
  {
    Inativo = Status.Inativo;
  }
}

