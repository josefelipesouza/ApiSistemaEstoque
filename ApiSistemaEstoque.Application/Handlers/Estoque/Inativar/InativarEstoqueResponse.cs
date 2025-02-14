namespace ApiSistemaEstoque.ApiSistemaEstoque.Application.Handlers.Estoque.Inativar;
public class InativarEstoqueResponse(bool success)
{
    public bool Success { get; set; } = success;
}
