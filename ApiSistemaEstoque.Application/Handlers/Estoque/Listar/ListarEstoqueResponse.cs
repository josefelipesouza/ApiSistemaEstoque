using ApiSistemaEstoque.ApiSistemaEstoque.Domain.Enums;

namespace ApiSistemaEstoque.ApiSistemaEstoque.Application.Handlers.Estoque.Listar;

public record ListarEstoqueResponse(
    int Codigo,
    string Descricao,
    string Localizacao,
    int Responsavel,
    int Superior,
    DateTime CreatedAt,
    DateTime updated_at,
    IEnumerable<Status> Inativo  
);