using System.ComponentModel.DataAnnotations;

namespace ApiSistemaEstoque.ApiSistemaEstoque.Domain.Entities;

    public class HistoricoValorItem
    {
      [Key]
      public int? Codigo { get; set; }
      public int? CodigoProduto { get; set; }
      public DateTime? DataInicial {  get; set; }
      public DateTime? DataFinal { get; set; }
      public float? Valor {  get; set; }
    
      public HistoricoValorItem(){}
      // Construtor com parâmetros
      public HistoricoValorItem(int codigoProduto, DateTime dataIncial, DateTime dataFinal, float valor)
        {
            CodigoProduto = codigoProduto;
            DataInicial = dataIncial;
            DataFinal = dataFinal;
            Valor = valor;

        }
    }

