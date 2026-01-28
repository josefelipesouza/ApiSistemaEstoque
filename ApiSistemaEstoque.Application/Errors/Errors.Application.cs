
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

        public static Error UsuarioNaoVinculadoAEstoque => Error.NotFound(
            code: "Usuario.NaoVinculadoAEstoque",
            description: "O usuário não está vinculado a nenhum estoque."
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

        public static Error ItemEstoqueQuantidadeInsuficiente => Error.NotFound(
            code: "ItemEstoque.QuantidadeInsuficiente",
            description: "A quantidade do item não é suficiente para realizar a movimentação neste estoque."
        );

        public static Error ItemEstoqueQuantidadeInvalida => Error.NotFound(
            code: "ItemEstoque.QuantidadeInvalida",
            description: "A quantidade informada é inválida."
        );
    
    
    }

    public static class MovimentacaoErrors
    {
        public static Error MovimentacaoNaoEncontrada => Error.NotFound(
            code: "Movimentacao.NaoEncontrada",
            description: "A Movimentação não foi encontrada."
        );

        public static Error MovimentacaoNaoPodeSerAlterada => Error.NotFound(
            code: "Movimentacao.NaoPodeSerAlterada",
            description: "A Movimentação não pode ser alterada."
        );

        public static Error TransicaoStatusInvalida => Error.NotFound(
            code: "Movimentacao.TransicaoStatusInvalida",
            description: "A transição de status da movimentação é inválida."
        );

        public static Error TransacaoInvalida => Error.NotFound(
            code: "Movimentacao.TransacaoInvalida",
            description: "A transação de movimentação é inválida."
        );

        public static Error UsuarioNaoPertenceAoEstoqueSolicitado => Error.NotFound(
            code: "Movimentacao.UsuarioNaoPertenceAoEstoqueSolicitado",
            description: "O usuário não pertence ao estoque solicitado."
        );

        public static Error UsuarioNaoPertenceAoEstoqueSolicitante => Error.NotFound(
            code: "Movimentacao.UsuarioNaoPertenceAoEstoqueSolicitante",
            description: "O usuário não pertence ao estoque solicitante."
        );


    }
}