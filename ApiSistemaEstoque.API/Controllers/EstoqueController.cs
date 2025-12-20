using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using ApiSistemaEstoque.ApiSistemaEstoque.Application.Handlers.Estoque.Cadastrar;
using ErrorOr;
using ApiSistemaEstoque.ApiSistemaEstoque.Application.Handlers.Estoque.Editar;
using ApiSistemaEstoque.ApiSistemaEstoque.Application.Handlers.Estoque.Listar;
using ApiSistemaEstoque.ApiSistemaEstoque.Application.Handlers.Estoque.Inativar;
using ApiSistemaEstoque.ApiSistemaEstoque.Application.Handlers.Estoque.BuscarPorCodigo;

namespace ApiSistemaEstoque.API.Controllers;

[Authorize]
[Route("api/estoques")]
[ApiController]
public class EstoqueController : ControllerBase
{
    private readonly IMediator _mediator;

    public EstoqueController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Cadastra um novo estoque no sistema.
    /// </summary>
    /// <param name="request">Dados do estoque a ser cadastrado.</param>
    /// <param name="cancellationToken">Token de cancelamento.</param>
    /// <returns>Retorna o estoque criado ou erro.</returns>
    [HttpPost]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(401)]
    [ProducesResponseType(403)]
    [ProducesResponseType(409)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> CadastrarEstoqueAsync(
        [FromBody] CadastrarEstoqueRequest request,
        CancellationToken cancellationToken)
    {

        // O handler já recupera o usuário via IHttpContextAccessor, então só envia o requestnn
        var resultado = await _mediator.Send(request, cancellationToken);

        return resultado.Match(
            response => Ok(new
            {
                Success = true,
                Message = "Estoque cadastrado com sucesso.",
                Data = response
            }),
            errors => Problem(errors));
    }

    /// <summary>
    /// Edita um estoque existente.
    /// </summary>
    /// <param name="codigo">Código do estoque a ser editado.</param>
    /// <param name="request">Novos dados do estoque.</param>
    /// <param name="cancellationToken">Token de cancelamento da requisição.</param>
    /// <returns>Retorna o estoque atualizado ou erro.</returns>
    [HttpPut("{codigo:int}")]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(401)]
    [ProducesResponseType(403)]
    [ProducesResponseType(404)]
    [ProducesResponseType(409)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> EditarEstoqueAsync(
        int codigo,
        [FromBody] EditarEstoqueRequest request,
        CancellationToken cancellationToken)
    {
 
        var resultado = await _mediator.Send(request, cancellationToken);

        return resultado.Match(
            response => Ok(new
            {
                Success = true,
                Message = "Estoque atualizado com sucesso.",
                Data = response
            }),
            errors => Problem(errors));
    }

    /// <summary>
    /// Lista todos os estoques cadastrados.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(200, Type = typeof(IEnumerable<ListarEstoqueResponse>))]
    [ProducesResponseType(500)]
    public async Task<IActionResult> ListarEstoquesAsync(CancellationToken cancellationToken)
    {
        var resultado = await _mediator.Send(new ListarEstoqueRequest(), cancellationToken);

        return resultado.Match(
            response => Ok(new
            {
                Success = true,
                Message = "Estoques listados com sucesso.",
                Data = response
            }),
            errors => Problem(errors)
        );
    }

    /// <summary>
    /// Inativa (marca como inativo) um estoque existente.
    /// </summary>
    /// <param name="codigo">Código do estoque que será inativado.</param>
    /// <param name="cancellationToken">Token de cancelamento.</param>
    /// <returns>Resultado da operação.</returns>
    [HttpPatch("{codigo:int}/inativar")]
    [ProducesResponseType(200, Type = typeof(bool))]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> InativarEstoqueAsync(
        int codigo,
        CancellationToken cancellationToken)
    {
        var request = new InativarEstoqueRequest(codigo);

        var resultado = await _mediator.Send(request, cancellationToken);

        return resultado.Match(
            ok => Ok(new
            {
                Success = true,
                Message = "Estoque inativado com sucesso."
            }),
            errors => Problem(errors)
        );
    }

      /// <summary>
    /// Retorna os detalhes de um estoque pelo seu código.
    /// </summary>
    /// <param name="codigo">Código do estoque a buscar.</param>
    /// <param name="cancellationToken">Token de cancelamento.</param>
    [HttpGet("{codigo:int}")]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> BuscarPorCodigoAsync(
        int codigo,
        CancellationToken cancellationToken)
    {
        var request = new BuscarPorCodigoEstoqueRequest { Codigo = codigo };

        var resultado = await _mediator.Send(request, cancellationToken);

        return resultado.Match(
            response => Ok(new
            {
                Success = true,
                Message = "Estoque encontrado com sucesso.",
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
