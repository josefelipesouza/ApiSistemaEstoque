using ApiSistemaEstoque.ApiSistemaEstoque.Domain.Enums;

namespace ApiSistemaEstoque.ApiSistemaEstoque.Application.Handlers.Categoria.BuscarPorCodigo;

public record BuscarPorCodigoCategoriaResponse(

    int Codigo,
    string Descricao,
    int Superior,
    string UsuarioCadastro,
    DateTime CreatedAt,
    DateTime updated_at,
    Status Inativo 
);


