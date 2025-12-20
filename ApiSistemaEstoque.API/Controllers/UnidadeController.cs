using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using ErrorOr;
using ApiSistemaEstoque.ApiSistemaEstoque.Application.Handlers.Unidade.Cadastrar;
using ApiSistemaEstoque.ApiSistemaEstoque.Application.Handlers.Unidade.Editar;
using ApiSistemaEstoque.ApiSistemaEstoque.Application.Handlers.Unidade.BuscarPorCodigo;
using ApiSistemaEstoque.ApiSistemaEstoque.Application.Handlers.Unidade.Listar;
using ApiSistemaEstoque.ApiSistemaEstoque.Application.Handlers.Unidade.Inativar;

namespace ApiSistemaEstoque.API.Controllers;

[Authorize]
[Route("api/unidades")]
[ApiController]
public class UnidadeController : ControllerBase
{
    private readonly IMediator _mediator;

    public UnidadeController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Cadastra uma nova unidade no sistema.
    /// </summary>
    /// <param name="request">Dados da unidade a ser cadastrada.</param>
    /// <param name="cancellationToken">Token de cancelamento.</param>
    /// <returns>Retorna a unidade criada ou erro.</returns>
    [HttpPost]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(401)]
    [ProducesResponseType(403)]
    [ProducesResponseType(409)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> CadastrarUnidadeAsync(
        [FromBody] CadastrarUnidadeRequest request,
        CancellationToken cancellationToken)
    {

        var resultado = await _mediator.Send(request, cancellationToken);

        return resultado.Match(
            response => Ok(new
            {
                Success = true,
                Message = "Unidade cadastrada com sucesso.",
                Data = response
            }),
            errors => Problem(errors));
    }

    /// <summary>
    /// Edita uma unidade existente no sistema.
    /// </summary>
    /// <param name="codigo">Código da unidade a ser editada.</param>
    /// <param name="request">Dados atualizados da unidade.</param>
    /// <param name="cancellationToken">Token de cancelamento.</param>
    /// <returns>Retorna a unidade atualizada ou erro.</returns>
    [HttpPut("{codigo}")]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(401)]
    [ProducesResponseType(403)]
    [ProducesResponseType(404)]
    [ProducesResponseType(409)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> EditarUnidadeAsync(
        [FromRoute] int codigo,
        [FromBody] EditarUnidadeRequest request,
        CancellationToken cancellationToken)
    {
        // Garante que o código da URL será usado no request
        request.Codigo = codigo;

        var resultado = await _mediator.Send(request, cancellationToken);

        return resultado.Match(
            response => Ok(new
            {
                Success = true,
                Message = "Unidade atualizada com sucesso.",
                Data = response
            }),
            errors => Problem(errors)
        );
    }

    /// <summary>
    /// Busca uma unidade pelo seu código.
    /// </summary>
    /// <param name="codigo">Código da unidade.</param>
    /// <param name="cancellationToken">Token de cancelamento.</param>
    /// <returns>Retorna os dados da unidade ou erro caso não encontrada.</returns>
    [HttpGet("{codigo}")]
    [ProducesResponseType(typeof(BuscarPorCodigoUnidadeResponse), 200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(401)]
    [ProducesResponseType(403)]
    [ProducesResponseType(404)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> BuscarUnidadePorCodigoAsync(
        [FromRoute] int codigo,
        CancellationToken cancellationToken)
    {
        var request = new BuscarPorCodigoUnidadeRequest { Codigo = codigo };

        var resultado = await _mediator.Send(request, cancellationToken);

        return resultado.Match(
            response => Ok(new
            {
                Success = true,
                Message = "Unidade encontrada com sucesso.",
                Data = response
            }),
            errors => Problem(errors)
        );
    }

    /// <summary>
    /// Lista todas as unidades cadastradas.
    /// </summary>
    /// <param name="cancellationToken">Token de cancelamento.</param>
    /// <returns>Retorna uma lista de unidades ou erro.</returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> ListarUnidadesAsync(CancellationToken cancellationToken)
    {
        var resultado = await _mediator.Send(new ListarUnidadeRequest(), cancellationToken);

        return resultado.Match(
            response => Ok(new
            {
                Success = true,
                Message = "Unidades listadas com sucesso.",
                Data = response
            }),
            errors => Problem(errors)
        );
    }

    /// <summary>
    /// Inativa uma unidade existente.
    /// </summary>
    /// <param name="codigo">Código da unidade a ser inativada.</param>
    /// <param name="cancellationToken">Token de cancelamento.</param>
    /// <returns>Retorna sucesso ou erro.</returns>
    [HttpPatch("{codigo}/inativar")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> InativarUnidadeAsync(
        [FromRoute] int codigo,
        CancellationToken cancellationToken)
    {
        var request = new InativarUnidadeRequest(codigo);

        var resultado = await _mediator.Send(request, cancellationToken);

        return resultado.Match(
            _ => Ok(new
            {
                Success = true,
                Message = "Unidade inativada com sucesso."
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
