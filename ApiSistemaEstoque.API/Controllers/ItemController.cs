using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using ErrorOr;
using ApiSistemaEstoque.ApiSistemaEstoque.Application.Handlers.Item.Cadastrar;
using ApiSistemaEstoque.ApiSistemaEstoque.Application.Handlers.Item.Editar;
using ApiSistemaEstoque.ApiSistemaEstoque.Application.Handlers.Item.BuscarPorCodigo;
using ApiSistemaEstoque.ApiSistemaEstoque.Application.Handlers.Item.Listar;
using ApiSistemaEstoque.ApiSistemaEstoque.Application.Handlers.Item.Inativar;

namespace ApiSistemaEstoque.API.Controllers;

[Authorize]
[Route("api/itens")]
[ApiController]
public class ItemController : ControllerBase
{
    private readonly IMediator _mediator;

    public ItemController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Cadastra um novo item no sistema.
    /// </summary>
    /// <param name="request">Dados do item a ser cadastrado.</param>
    /// <param name="cancellationToken">Token de cancelamento.</param>
    /// <returns>Retorna o item criado ou erros de validação.</returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CadastrarItemAsync(
        [FromBody] CadastrarItemRequest request,
        CancellationToken cancellationToken)
    {
        var resultado = await _mediator.Send(request, cancellationToken);

        return resultado.Match(
            response => Ok(new
            {
                Success = true,
                Message = "Item cadastrado com sucesso.",
                Data = response
            }),
            errors => Problem(errors)
        );
    }

    /// <summary>
    /// Edita um item existente no sistema.
    /// </summary>
    /// <param name="codigo">Código do item a ser editado.</param>
    /// <param name="request">Dados atualizados do item.</param>
    /// <param name="cancellationToken">Token de cancelamento.</param>
    /// <returns>Retorna o item atualizado ou erro.</returns>
    [HttpPut("{codigo}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> EditarItemAsync(
        [FromRoute] int codigo,
        [FromBody] EditarItemRequest request,
        CancellationToken cancellationToken)
    {
        // Garante que o código da rota sobrescreva o do corpo da requisição
        request.Codigo = codigo;

        var resultado = await _mediator.Send(request, cancellationToken);

        return resultado.Match(
            response => Ok(new
            {
                Success = true,
                Message = "Item atualizado com sucesso.",
                Data = response
            }),
            errors => Problem(errors)
        );
    }

    /// <summary>
    /// Busca um item pelo seu código.
    /// </summary>
    /// <param name="codigo">Código do item.</param>
    /// <param name="cancellationToken">Token de cancelamento.</param>
    /// <returns>Retorna os dados do item ou erro.</returns>
    [HttpGet("{codigo}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> BuscarItemPorCodigoAsync(
        [FromRoute] int codigo,
        CancellationToken cancellationToken)
    {
        var request = new BuscarPorCodigoItemRequest { Codigo = codigo };

        var resultado = await _mediator.Send(request, cancellationToken);

        return resultado.Match(
            response => Ok(new
            {
                Success = true,
                Message = "Item encontrado com sucesso.",
                Data = response
            }),
            errors => Problem(errors)
        );
    }

    /// <summary>
    /// Lista todos os itens do sistema.
    /// </summary>
    /// <param name="cancellationToken">Token de cancelamento.</param>
    /// <returns>Retorna uma lista de itens ou erro.</returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> ListarItensAsync(CancellationToken cancellationToken)
    {
        var request = new ListarItemRequest();

        var resultado = await _mediator.Send(request, cancellationToken);

        return resultado.Match(
            response => Ok(new
            {
                Success = true,
                Message = "Itens listados com sucesso.",
                Data = response
            }),
            errors => Problem(errors)
        );
    }

    /// <summary>
    /// Inativa um item existente no sistema.
    /// </summary>
    /// <param name="codigo">Código do item a ser inativado.</param>
    /// <param name="cancellationToken">Token de cancelamento.</param>
    /// <returns>Retorna sucesso ou erro.</returns>
    [HttpPatch("{codigo}/inativar")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> InativarItemAsync(
        [FromRoute] int codigo,
        CancellationToken cancellationToken)
    {
        var request = new InativarItemRequest(codigo);

        var resultado = await _mediator.Send(request, cancellationToken);

        return resultado.Match(
            _ => Ok(new
            {
                Success = true,
                Message = "Item inativado com sucesso."
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
