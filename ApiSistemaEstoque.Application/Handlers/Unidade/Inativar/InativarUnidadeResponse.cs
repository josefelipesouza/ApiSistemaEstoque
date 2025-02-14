namespace ApiSistemaEstoque.ApiSistemaEstoque.Application.Handlers.Unidade.Inativar;

public class InativarUnidadeResponse
{
    public bool Success { get; set; }

    public InativarUnidadeResponse(bool success)
    {
        Success = success;
    }
}
