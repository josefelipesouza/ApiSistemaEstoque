using ErrorOr;
using FluentValidation;
using MediatR;

namespace ApiSistemaEstoque.ApiSistemaEstoque.Application.Handlers;


public abstract class BaseHandler
{
    protected readonly IMediator Mediator;

    protected BaseHandler(IMediator mediator)
    {
        Mediator = mediator;
    }

    protected List<Error> Validar<TEntity, TValidator>(TEntity entity, TValidator validator)
        where TEntity : class
        where TValidator : AbstractValidator<TEntity>
    {
        var resultadoValidacao = validator.Validate(entity);

        return resultadoValidacao.Errors
            .Select(erro => Error.Validation(code: erro.ErrorCode, description: erro.ErrorMessage,
                new Dictionary<string, object>()
                {
                    { "propertyName", erro.PropertyName }
                }))
            .ToList();
    }
}