using ApiSistemaEstoque.ApiSistemaEstoque.Domain.Enums;

namespace ApiSistemaEstoque.ApiSistemaEstoque.Application.Handlers.TipoMovimentacao.Listar;

public record ListarTipoMovimentacaoResponse(

    int Codigo,
    string Descricao,
    TipoBaseMovimentacao Tipo,
    string UsuarioCadastro,
    DateTime CreatedAt,
    DateTime updated_at,
    Status Inativo 
);