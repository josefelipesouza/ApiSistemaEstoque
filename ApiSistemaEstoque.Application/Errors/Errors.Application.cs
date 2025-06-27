
using ErrorOr;

namespace ApiSistemaEstoque.ApiSistemaEstoque.Application.Errors;


public static partial class Application
{

    public static class EstoqueErrors
    {
        public static Error EstoqueNaoEncontrado => Error.NotFound(
            code: "Estoque.NaoEncontrado",
            description: "O estoque não foi encontrado."
        );
    }

    public static class CategoriaErrors
    {
        public static Error CategoriaNaoEncontrada => Error.NotFound(
            code: "Categoria.NaoEncontrada",
            description: "A categoria não foi encontrada."
        );
    }

    public static class TipoMovimentacaoErrors
    {
        public static Error TipoMovimentacaoNaoEncontrada => Error.NotFound(
            code: "TipoMovimentacao.NaoEncontrada",
            description: "O tipo de movimentação não foi encontrada."
        );
    }

    public static class UnidadeErrors
    {
        public static Error UnidadeNaoEncontrada => Error.NotFound(
            code: "Unidade.NaoEncontrada",
            description: "A unidade não foi encontrada."
        );
    }

    public static class UsuarioErrors
    {
        public static Error UsuarioNaoAutenticado => Error.NotFound(
            code: "Usuario.NaoEncontrado",
            description: "O usuário não foi encontrado."
        );
    }

    public static class ItemErrors
    {
        public static Error ItemNaoEncontrado => Error.NotFound(
            code: "Item.NaoEncontrado",
            description: "O item não foi encontrado."
        );
    }

    public static class ItemEstoqueErrors
    {
        public static Error ItemEstoqueNaoEncontrado => Error.NotFound(
            code: "ItemEstoque.NaoEncontrado",
            description: "O item não foi encontrado neste estoque."
        );
    }

    public static class MovimentacaoErrors
    {
        public static Error MovimentacaoNaoEncontrada => Error.NotFound(
            code: "Movimentacao.NaoEncontrada",
            description: "A Movimentação não foi encontrada."
        );
    }
}