using ApiSistemaEstoque.ApiSistemaEstoque.Domain.Enums;

namespace ApiSistemaEstoque.ApiSistemaEstoque.Application.Handlers.TipoMovimentacao.Cadastrar;

public record CadastrarTipoMovimentacaoResponse(
    int Codigo,
    TipoBaseMovimentacao Tipo,
    string Descricao,
    string UsuarioCadastro,
    DateTime CreatedAt,
    DateTime updated_at,
    Status Inativo
);
