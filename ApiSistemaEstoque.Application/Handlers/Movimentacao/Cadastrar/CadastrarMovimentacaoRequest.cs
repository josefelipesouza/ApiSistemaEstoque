using MediatR;
using ErrorOr;
using FluentValidation;

namespace ApiSistemaEstoque.ApiSistemaEstoque.Application.Handlers.Movimentacao.Cadastrar;

public record CadastrarMovimentacaoRequest(

    int CodigoTipoMovimentacao, 
    int CodigoEstoqueSolicitante, 
    int CodigoUsuarioEstoqueSolicitante, 
    int? CodigoEstoqueSolicitado, 
    List<ItemMovimentacao> Itens 
) : IRequest<ErrorOr<CadastrarMovimentacaoResponse>>;
    public record ItemMovimentacao(
    
        int CodigoItem,
        int Quantidade
    ) : IRequest<ErrorOr<CadastrarMovimentacaoResponse>>;

    public class CadastrarMovimentacaoRequestValidator : AbstractValidator<CadastrarMovimentacaoRequest>
    {
        public CadastrarMovimentacaoRequestValidator()
        {
            RuleFor(x => x.CodigoTipoMovimentacao)
                .GreaterThan(0).WithMessage("O código do tipo de movimentação é obrigatório.");

            RuleFor(x => x.CodigoEstoqueSolicitante)
                .GreaterThan(0).WithMessage("O código do estoque solicitante é obrigatório.");

            RuleFor(x => x.CodigoUsuarioEstoqueSolicitante)
                .GreaterThan(0).WithMessage("O código do usuário solicitante é obrigatório.");

            RuleFor(x => x.CodigoEstoqueSolicitado)
                .GreaterThan(0).When(x => x.CodigoEstoqueSolicitado.HasValue)
                .WithMessage("O código do estoque solicitado deve ser maior que zero.");

            RuleForEach(x => x.Itens)
                .ChildRules(items =>
                {
                    items.RuleFor(i => i.CodigoItem)
                        .GreaterThan(0).WithMessage("O código do item é obrigatório.");
                    
                    items.RuleFor(i => i.Quantidade)
                        .GreaterThan(0).WithMessage("A quantidade deve ser maior que zero.");
                });
        }
    }

