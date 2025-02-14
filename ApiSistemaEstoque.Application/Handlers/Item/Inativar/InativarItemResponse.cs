namespace ApiSistemaEstoque.ApiSistemaEstoque.Application.Handlers.Item.Inativar;

public class InativarItemResponse
{
    public bool Success { get; set; }

    public InativarItemResponse(bool success)
    {
        Success = success;
    }
}
