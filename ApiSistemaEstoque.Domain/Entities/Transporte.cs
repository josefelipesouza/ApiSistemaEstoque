using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiSistemaEstoque.ApiSistemaEstoque.Domain.Entities;

public class Transporte
{
    [Key]
    public int Codigo { get; private set; }

    public string PlacaVeiculo { get; private set; }

    public int CodigoMovimentacao { get; private set; }

    public int CodigoItem { get; private set; }

    public int Quantidade { get; private set; }

    public DateTime DataEntrada { get; private set; }

    public DateTime? DataSaida { get; private set; }

    //Relacionamentos

    [ForeignKey(nameof(CodigoMovimentacao))]
    public Movimentacao Movimentacao { get; private set; }

    [ForeignKey(nameof(CodigoItem))]
    public Item Item { get; private set; }

    //Construtor protegido para EF
    protected Transporte() { }

    //Construtor de domínio
    public Transporte(
        string placaVeiculo,
        int codigoMovimentacao,
        int codigoItem,
        int quantidade,
        DateTime dataEntrada)
    {
        PlacaVeiculo = placaVeiculo;
        CodigoMovimentacao = codigoMovimentacao;
        CodigoItem = codigoItem;
        Quantidade = quantidade;
        DataEntrada = DateTime.UtcNow;
    }

    // 🧠 Regras de negócio

    public void SetDataSaida(DateTime dataSaida)
    {
        DataSaida = dataSaida;
    }

    public void SetQuantidade(int quantidade)
    {
        Quantidade = quantidade;
    }

   
}
