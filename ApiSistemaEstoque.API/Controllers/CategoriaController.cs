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
        [Authorize(Roles = "Administrador")]
        [ProducesResponseType(200, Type = typeof(bool))]
        public async Task<IActionResult> CadastrarCategoriaAsync([FromBody] CadastrarCategoriaRequest request, CancellationToken cancellationToken)
        {
            var resultado = await _mediator.Send(request, cancellationToken);

            return resultado.Match(
                response => Ok(new { Success = true, Message = "Categoria cadastrada com sucesso." }),
                erros => Problem(erros));
        }

        /// <summary>
        /// Formata os erros para retorno em uma resposta HTTP.
        /// </summary>
        /// <param name="erros">Lista de erros ocorridos.</param>
        /// <returns>Retorna um objeto do tipo ObjectResult contendo os erros.</returns>
        private ObjectResult Problem(List<Error> erros)
        {
            throw new NotImplementedException();
        }
    }

