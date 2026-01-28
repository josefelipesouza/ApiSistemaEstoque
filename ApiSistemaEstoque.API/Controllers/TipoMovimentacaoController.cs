using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using ErrorOr;
using ApiSistemaEstoque.ApiSistemaEstoque.Application.Handlers.TipoMovimentacao.Cadastrar;
using ApiSistemaEstoque.ApiSistemaEstoque.Application.Handlers.TipoMovimentacao.Listar;
using ApiSistemaEstoque.ApiSistemaEstoque.Application.Handlers.TipoMovimentacao.BuscarPorCodigo;
using ApiSistemaEstoque.ApiSistemaEstoque.Application.Handlers.TipoMovimentacao.Inativar;

namespace ApiSistemaEstoque.API.Controllers;

[Authorize]
[Route("api/tipos-movimentacao")]
[ApiController]
public class TipoMovimentacaoController : ControllerBase
{
    private readonly IMediator _mediator;

    public TipoMovimentacaoController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Cadastra um novo tipo de movimentação no sistema.
    /// </summary>
    /// <param name="request">Dados do tipo de movimentação a ser cadastrado.</param>
    /// <param name="cancellationToken">Token de cancelamento.</param>
    /// <returns>Retorna o tipo de movimentação criado ou erros de validação.</returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CadastrarTipoMovimentacaoAsync(
        [FromBody] CadastrarTipoMovimentacaoRequest request,
        CancellationToken cancellationToken)
    {
        var resultado = await _mediator.Send(request, cancellationToken);

        return resultado.Match(
            response => Ok(new
            {
                Success = true,
                Message = "Tipo de movimentação cadastrado com sucesso.",
                Data = response
            }),
            errors => Problem(errors)
        );
    }

    /// <summary>
    /// Lista todos os tipos de movimentação do sistema.
    /// </summary>
    /// <param name="cancellationToken">Token de cancelamento.</param>
    /// <returns>Retorna uma lista de tipos de movimentação ou erro.</returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> ListarTiposMovimentacaoAsync(CancellationToken cancellationToken)
    {
        var request = new ListarTipoMovimentacaoRequest();

        var resultado = await _mediator.Send(request, cancellationToken);

        return resultado.Match(
            response => Ok(new
            {
                Success = true,
                Message = "Tipos de movimentação listados com sucesso.",
                Data = response
            }),
            errors => Problem(errors)
        );
    }

    /// <summary>
    /// Busca um tipo de movimentação pelo seu código.
    /// </summary>
    /// <param name="codigo">Código do tipo de movimentação.</param>
    /// <param name="cancellationToken">Token de cancelamento.</param>
    /// <returns>Retorna os dados do tipo de movimentação ou erro.</returns>
    [HttpGet("{codigo}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> BuscarPorCodigoAsync(
        [FromRoute] int codigo,
        CancellationToken cancellationToken)
    {
        var request = new BuscarPorCodigoTipoMovimentacaoRequest(codigo);

        var resultado = await _mediator.Send(request, cancellationToken);

        return resultado.Match(
            response => Ok(new
            {
                Success = true,
                Message = "Tipo de movimentação encontrado com sucesso.",
                Data = response
            }),
            errors => Problem(errors)
        );
    }

    /// <summary>
    /// Inativa um tipo de movimentação pelo código.
    /// </summary>
    /// <param name="codigo">Código do tipo de movimentação.</param>
    /// <param name="cancellationToken">Token de cancelamento.</param>
    /// <returns>Retorna sucesso ou erro.</returns>
    [HttpPatch("{codigo}/inativar")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> InativarTipoMovimentacaoAsync(
        [FromRoute] int codigo,
        CancellationToken cancellationToken)
    {
        var request = new InativarTipoMovimentacaoRequest(codigo);

        var resultado = await _mediator.Send(request, cancellationToken);

        return resultado.Match(
            _ => Ok(new
            {
                Success = true,
                Message = "Tipo de movimentação inativado com sucesso."
            }),
            errors => Problem(errors)
        );
    }

    private ObjectResult Problem(List<Error> erros)
    {
        var primeiroErro = erros[0];

        var statusCode = primeiroErro.Type switch
        {
            ErrorType.Validation => StatusCodes.Status400BadRequest,
            ErrorType.NotFound => StatusCodes.Status404NotFound,
            ErrorType.Conflict => StatusCodes.Status409Conflict,
            ErrorType.Unauthorized => StatusCodes.Status401Unauthorized,
            ErrorType.Forbidden => StatusCodes.Status403Forbidden,
            _ => StatusCodes.Status500InternalServerError
        };

        return Problem(
            statusCode: statusCode,
            title: primeiroErro.Description,
            detail: string.Join(", ", erros.Select(e => e.Description)),
            type: primeiroErro.Code);
    }
}
