using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ErrorOr;
using ApiSistemaEstoque.ApiSistemaEstoque.Application.Handlers.ItemEstoque.Listar;
using ApiSistemaEstoque.ApiSistemaEstoque.Application.Handlers.ItemEstoque.BuscarPorCodigoEstoque;
using ApiSistemaEstoque.ApiSistemaEstoque.Application.Handlers.ItemEstoque.BuscarPorCodigoEstoqueItem;

namespace ApiSistemaEstoque.API.Controllers;

[Authorize]
[Route("api/itens-estoque")]
[ApiController]
public class ItemEstoqueController : ControllerBase
{
    private readonly IMediator _mediator;

    public ItemEstoqueController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Lista todos os itens de estoque.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(200)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> ListarAsync(CancellationToken cancellationToken)
    {
        var resultado = await _mediator.Send(new ListarItemEstoqueRequest(), cancellationToken);

        return resultado.Match(
            response => Ok(new
            {
                Success = true,
                Message = "Itens de estoque listados com sucesso.",
                Data = response
            }),
            errors => Problem(errors)
        );
    }

    /// <summary>
    /// Lista os itens de um estoque específico.
    /// </summary>
    [HttpGet("estoque/{codigoEstoque:int}")]
    [ProducesResponseType(200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> BuscarPorCodigoEstoqueAsync(
        int codigoEstoque,
        CancellationToken cancellationToken)
    {
        var request = new BuscarPorCodigoRequest(codigoEstoque);

        var resultado = await _mediator.Send(request, cancellationToken);

        return resultado.Match(
            response => Ok(new
            {
                Success = true,
                Message = "Itens do estoque encontrados com sucesso.",
                Data = response
            }),
            errors => Problem(errors)
        );
    }

    /// <summary>
    /// Retorna um item específico dentro de um estoque.
    /// </summary>
    [HttpGet("estoque/{codigoEstoque:int}/item/{codigoItem:int}")]
    [ProducesResponseType(200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> BuscarItemEstoqueAsync(
        int codigoEstoque,
        int codigoItem,
        CancellationToken cancellationToken)
    {
        var request = new BuscarPorCodigoEstoqueItemRequest
        {
            codigoEstoque = codigoEstoque,
            codigoItem = codigoItem
        };

        var resultado = await _mediator.Send(request, cancellationToken);

        return resultado.Match(
            response => Ok(new
            {
                Success = true,
                Message = "Item de estoque encontrado com sucesso.",
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
