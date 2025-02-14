namespace ApiSistemaEstoque.ApiSistemaEstoque.Application.Contracts.Response;

public record PermissoesResponseModel(
    IEnumerable<TipoValorPermissaoResponseModel> PerfisDeAcesso
);

