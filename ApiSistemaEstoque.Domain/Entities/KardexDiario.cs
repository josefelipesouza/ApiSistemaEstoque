using System.ComponentModel.DataAnnotations;

namespace ApiSistemaEstoque.ApiSistemaEstoque.Domain.Entities;

    public class KardexDiario
    {
      [Key]
      public int? Codigo { get; set; }
      public DateTime? Data {  get; set; }
      public int? CodigoMovimentacao { get; set; }
      public int? CodigoItem {  get; set; }
      public int? Quantidade { get; set; }
      public int? CodigoEstoqueOrigem { get; set; }
      public int? QuantidadeEstoqueOrigemInicial { get; set; }
      public int? QuantidadeEstoqueOrigemFinal { get; set; }
      public int? CodigoEstoqueDestino { get; set; }
      public int? QuantidadeEstoqueDestinoInicial { get; set; }
      public int? QuantidadeEstoqueDestinoFinal { get; set; }

      public KardexDiario(){}

        public KardexDiario(DateTime data, int codigoMovimentacao, int codigoItem, int quantidade, int codigoEstoqueOrigem,
            int quantidadeEstoqueOrigemInicial, int quantidadeEstoqueOrigemFinal, int codigoEstoqueDestino, int quantidadeEstoqueDestinoInicial, int quantidadeEstoqueDestinoFinal)
        {
           CodigoMovimentacao = codigoMovimentacao;
           CodigoItem = codigoItem;
           Quantidade = quantidade;
           CodigoEstoqueOrigem = codigoEstoqueOrigem;
           QuantidadeEstoqueOrigemInicial = quantidadeEstoqueOrigemInicial;
           QuantidadeEstoqueOrigemFinal = quantidadeEstoqueOrigemFinal;
           CodigoEstoqueDestino = codigoEstoqueDestino;
           QuantidadeEstoqueDestinoInicial = quantidadeEstoqueDestinoInicial;
           QuantidadeEstoqueDestinoFinal = quantidadeEstoqueDestinoFinal;

        }
    }

