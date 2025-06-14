using ApiSistemaEstoque.ApiSistemaEstoque.Application.Handlers.Categoria.Cadastrar;
using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApiSistemaEstoque.ApiSistemaEstoque.API.Controllers;

/// <summary>
/// Controlador responsável pelas operações relacionadas à entidade Categoria.
/// </summary>
[Route("api/categorias")]
public class CategoriaController : ControllerBase
{
    private readonly IMediator _mediator;

    /// <summary>
    /// Construtor do CategoriaController.
    /// </summary>
    /// <param name="mediator">Objeto responsável por mediar as requisições.</param>
    public CategoriaController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Cadastra uma nova categoria.
    /// </summary>
    /// <param name="request">Dados da categoria a ser cadastrada.</param>
    /// <param name="cancellationToken">Token de cancelamento da requisição.</param>
    /// <returns>Retorna o status do cadastro da categoria.</returns>
    [HttpPost]
    [ProducesResponseType(200, Type = typeof(bool))]
    [ProducesResponseType(400)]
    [ProducesResponseType(401)]
    [ProducesResponseType(403)]
    [ProducesResponseType(409)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> CadastrarCategoriaAsync(
    [FromBody] CadastrarCategoriaRequest request,
    CancellationToken cancellationToken)
    {
        var resultado = await _mediator.Send(request, cancellationToken);

        return resultado.Match(
            response => Ok(new
            {
                Success = true,
                Message = "Categoria cadastrada com sucesso.",
                Data = response
            }),
            errors => Problem(errors));
    }

    /// <summary>
    /// Formata os erros para retorno em uma resposta HTTP.
    /// </summary>
    /// <param name="erros">Lista de erros ocorridos.</param>
    /// <returns>Retorna um objeto do tipo ObjectResult contendo os erros.</returns>
    private ObjectResult Problem(List<Error> erros)
    {
        var firstError = erros[0];

        var statusCode = firstError.Type switch
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
            title: firstError.Description,
            detail: string.Join(", ", erros.Select(e => e.Description)),
            type: firstError.Code);
    }
}

