using ErrorOr;
using MediatR;
using ApiSistemaEstoque.ApiSistemaEstoque.Application.Interfaces.Repositories;
using System.Security.Claims;
using Microsoft.Extensions.DependencyInjection; // Para IServiceCollection
using Microsoft.AspNetCore.Http;                // Para IHttpContextAccessor
using Microsoft.AspNetCore.Identity;


namespace ApiSistemaEstoque.ApiSistemaEstoque.Application.Handlers.Unidade.Editar;

public class EditarUnidadeHandler : BaseHandler, IRequestHandler<EditarUnidadeRequest, ErrorOr<EditarUnidadeResponse>>
{
    private readonly IUnidadeRepository _unidadeRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public EditarUnidadeHandler(
        IMediator mediator,
        IUnidadeRepository unidadeRepository,
        IHttpContextAccessor httpContextAccessor) : base(mediator)
    {
        _unidadeRepository = unidadeRepository;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<ErrorOr<EditarUnidadeResponse>> Handle(EditarUnidadeRequest request, CancellationToken cancellationToken)
    {
        var validator = new EditarUnidadeRequest.EditarUnidadeRequestValidator();
        var validationResult = validator.Validate(request);
        
        if (!validationResult.IsValid)
            return validationResult.Errors
                .Select(e => Error.Validation(e.PropertyName, e.ErrorMessage))
                .ToList();
                
        var usuarioCadastro = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrWhiteSpace(usuarioCadastro))
            return Errors.Application.UsuarioErrors.UsuarioNaoAutenticado;        

        var unidadeExistente = await _unidadeRepository.BuscarPorCodigoAsync(request.Codigo, cancellationToken);

        if (unidadeExistente is null)
            return Errors.Application.UnidadeErrors.UnidadeNaoEncontrada;

        unidadeExistente.SetDescricao(request.Descricao);
        unidadeExistente.SetDataAlteracao(DateTime.UtcNow);
        unidadeExistente.SetUsuarioCadastro(usuarioCadastro);

        _unidadeRepository.Atualizar(unidadeExistente);
        
        await _unidadeRepository.UnitOfWork.CommitAsync(cancellationToken);

        return new EditarUnidadeResponse(
            unidadeExistente.Codigo,
            unidadeExistente.Descricao,
            unidadeExistente.UsuarioCadastro,
            unidadeExistente.CreatedAt,
            unidadeExistente.updated_at,
            unidadeExistente.Inativo
        );
    }
}
