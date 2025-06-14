// Api/Controllers/AuthController.cs
using ApiSistemaEstoque.Application.Handlers.Usuario.Auth.Logar;
using ApiSistemaEstoque.Application.Handlers.Usuario.Auth.Registrar;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ApiSistemaEstoque.API.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuthController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("registrar")]
    public async Task<IActionResult> RegistrarAsync([FromBody] RegistrarUsuarioRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var response = await _mediator.Send(request, cancellationToken);
            return Ok(response);
        }
        catch (ApplicationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPost("login")]
    public async Task<IActionResult> LoginAsync([FromBody] LoginUsuarioRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var response = await _mediator.Send(request, cancellationToken);
            return Ok(response);
        }
        catch (ApplicationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

}
