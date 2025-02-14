using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ApiSistemaEstoque.ApiSistemaEstoque.Domain.Enums;

namespace ApiSistemaEstoque.ApiSistemaEstoque.Domain.Entities;

public class Movimentacao
{
    [Key]
    public int Codigo { get; set; }
    public StatusMovimentacao Status { get; set; }
    public ICollection<ItemMovimentacao> ItensMovimentacao { get; set; } = new List<ItemMovimentacao>();
    public int CodigoTipoMovimentacao { get; set; }
    public int CodigoEstoqueSolicitante { get; set; }
    public int CodigoUsuarioEstoqueSolicitante { get; set; }
    public int CodigoEstoqueSolicitado { get; set; }
    public int CodigoUsuarioEstoqueSolicitado { get; set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime updated_at { get; private set; }

    [ForeignKey(nameof(CodigoTipoMovimentacao))]
    public TipoMovimentacao TipoMovimentacao { get; private set; } = default!;

    public Movimentacao(int codigoTipoMovimentacao, int codigoEstoqueSolicitante, int codigoUsuarioEstoqueSolicitante, int codigoEstoqueSolicitado)
    {
        Status = StatusMovimentacao.Aguardando;
        CodigoTipoMovimentacao = codigoTipoMovimentacao;
        CodigoEstoqueSolicitante = codigoEstoqueSolicitante;
        CodigoUsuarioEstoqueSolicitante = codigoUsuarioEstoqueSolicitante;
        CodigoEstoqueSolicitado = codigoEstoqueSolicitado;
        CodigoUsuarioEstoqueSolicitado = 0;
        CreatedAt = DateTime.UtcNow;
    }

    // Construtor padrão necessário para o EF
    public Movimentacao() { }

    public void SetStatus(StatusMovimentacao status)
    {
        Status = status;
    }

    public void SetCodigoTipoMovimentacao(int codigoTipoMovimentacao)
    {
        CodigoTipoMovimentacao = codigoTipoMovimentacao;
    }

    public void SetCodigoEstoqueSolicitante(int codigoEstoqueSolicitante)
    {
        CodigoEstoqueSolicitante = codigoEstoqueSolicitante;
    }

    public void SetCodigoEstoqueSolicitado(int codigoEstoqueSolicitado)
    {
        CodigoEstoqueSolicitado = codigoEstoqueSolicitado;
    }

    public void SetDataAlteracao(DateTime dataAlteracao)
    {
        updated_at = dataAlteracao;
    }

}

