using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using ErrorOr;
using ApiSistemaEstoque.ApiSistemaEstoque.Application.Handlers.Movimentacao.Cadastrar;
using ApiSistemaEstoque.ApiSistemaEstoque.Application.Handlers.Movimentacao.BuscarPorCodigo;

namespace ApiSistemaEstoque.API.Controllers;

[Authorize]
[Route("api/movimentacoes")]
[ApiController]
public class MovimentacaoController : ControllerBase
{
    private readonly IMediator _mediator;

    public MovimentacaoController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Cadastra uma nova movimentação no sistema.
    /// </summary>
    /// <param name="request">Dados da movimentação a ser cadastrada.</param>
    /// <param name="cancellationToken">Token de cancelamento.</param>
    /// <returns>Retorna a movimentação criada ou erros de validação.</returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CadastrarMovimentacaoAsync(
        [FromBody] CadastrarMovimentacaoRequest request,
        CancellationToken cancellationToken)
    {
        var resultado = await _mediator.Send(request, cancellationToken);

        return resultado.Match(
            response => Ok(new
            {
                Success = true,
                Message = "Movimentação cadastrada com sucesso.",
                Data = response
            }),
            errors => Problem(errors)
        );
    }

    /// <summary>
    /// Altera uma movimentação existente.
    /// </summary>
    /// <param name="codigo">Código da movimentação a editar.</param>
    /// <param name="request">Dados a alterar.</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// PUT api/movimentacoes/{codigo}
    [HttpPut("{codigo:int}")]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(401)]
    [ProducesResponseType(404)]
    [ProducesResponseType(409)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> EditarMovimentacaoAsync(
        int codigo,
        [FromBody] EditarMovimentacaoRequest request,
        CancellationToken cancellationToken)
    {
        // 🔐 Garante que o código da rota é usado
        request.Codigo = codigo;

        var resultado = await _mediator.Send(request, cancellationToken);

        return resultado.Match(
            response => Ok(new
            {
                Success = true,
                Message = "Movimentação atualizada com sucesso.",
                Data = response
            }),
            errors => Problem(errors)
        );
    }

    /// <summary>
    /// Busca uma movimentação pelo código.
    /// </summary>
    /// <param name="codigo">Código da movimentação a ser buscada.</param>
    /// <param name="cancellationToken">Token de cancelamento.</param>
    /// <returns>Retorna os dados da movimentação encontrada ou erro caso não exista.</returns>
    [HttpGet("{codigo:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> BuscarPorCodigoAsync([FromRoute] int codigo, CancellationToken cancellationToken)
    {
        var request = new BuscarPorCodigoMovimentacaoRequest { Codigo = codigo };

        var resultado = await _mediator.Send(request, cancellationToken);

        return resultado.Match(
            response => Ok(new
            {
                Success = true,
                Message = "Movimentação encontrada com sucesso.",
                Data = response
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
