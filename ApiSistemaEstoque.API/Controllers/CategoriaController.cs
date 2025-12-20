using System.Security.Claims;
using ApiSistemaEstoque.ApiSistemaEstoque.Application.Handlers.Categoria.BuscarPorCodigo;
using ApiSistemaEstoque.ApiSistemaEstoque.Application.Handlers.Categoria.Cadastrar;
using ApiSistemaEstoque.ApiSistemaEstoque.Application.Handlers.Categoria.Editar;
using ApiSistemaEstoque.ApiSistemaEstoque.Application.Handlers.Categoria.Inativar;
using ApiSistemaEstoque.ApiSistemaEstoque.Application.Handlers.Categoria.Listar;
using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApiSistemaEstoque.ApiSistemaEstoque.API.Controllers;

/// <summary>
/// Controlador responsável pelas operações relacionadas à entidade Categoria.
/// </summary>
[Authorize] // ✅ Toda a controller requer autenticação
[Route("api/categorias")]
[ApiController]
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
    /// Altera uma categoria existente.
    /// </summary>
    /// <param name="codigo">Código da categoria a editar.</param>
    /// <param name="request">Dados a alterar.</param>
    // PUT api/categorias/{codigo}
    [HttpPut("{codigo:int}")]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(401)]
    [ProducesResponseType(404)]
    [ProducesResponseType(409)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> EditarCategoriaAsync(
        int codigo,
        [FromBody] EditarCategoriaRequest request,
        CancellationToken cancellationToken)
    {
        
        var resultado = await _mediator.Send(request, cancellationToken);

        return resultado.Match(
            response => Ok(new
            {
                Success = true,
                Message = "Categoria atualizada com sucesso.",
                Data = response
            }),
            errors => Problem(errors));
    }

    /// <summary>
    /// Inativa (marca como inativo) uma categoria existente.
    /// </summary>
    /// <param name="codigo">Código da categoria que será inativada.</param>
    /// <param name="cancellationToken"></param>
    [HttpPatch("{codigo:int}/inativar")]
    [ProducesResponseType(200, Type = typeof(bool))]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> InativarCategoriaAsync(
        int codigo,
        CancellationToken cancellationToken)
    {
        // construímos o request simples contendo só o código
        var request = new InativarCategoriaRequest(codigo);

        var resultado = await _mediator.Send(request, cancellationToken);

        return resultado.Match(
            ok => Ok(new { Success = true, Message = "Categoria inativada com sucesso." }),
            errors => Problem(errors)
        );
    }

    /// <summary>
    /// Lista todas as categorias cadastradas.
    /// </summary>
    /// <param name="cancellationToken">Token de cancelamento da operação.</param>
    /// <returns>Lista de categorias.</returns>
    [HttpGet]
    [ProducesResponseType(200)]
    [ProducesResponseType(401)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> ListarCategoriasAsync(CancellationToken cancellationToken)
    {
        var request = new ListarCategoriaRequest();

        var resultado = await _mediator.Send(request, cancellationToken);

        return resultado.Match(
            lista => Ok(new
            {
                Success = true,
                Message = "Categorias listadas com sucesso.",
                Data = lista
            }),
            errors => Problem(errors)
        );
    }

    /// <summary>
    /// Busca uma categoria pelo código.
    /// </summary>
    /// <param name="codigo">Código da categoria.</param>
    /// <param name="cancellationToken">Token de cancelamento.</param>
    /// <returns>Retorna os dados da categoria ou erro.</returns>
    [HttpGet("{codigo:int}")]
    [ProducesResponseType(200)]
    [ProducesResponseType(404)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> BuscarCategoriaPorCodigoAsync(
        int codigo,
        CancellationToken cancellationToken)
    {
        var request = new BuscarPorCodigoCategoriaRequest { Codigo = codigo };

        var resultado = await _mediator.Send(request, cancellationToken);

        return resultado.Match(
            response => Ok(new
            {
                Success = true,
                Message = "Categoria encontrada com sucesso.",
                Data = response
            }),
            errors => Problem(errors)
        );
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
