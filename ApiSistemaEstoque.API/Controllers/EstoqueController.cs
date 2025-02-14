using Microsoft.AspNetCore.Mvc;
using MediatR;

namespace ApiSistemaEstoque.API.Controllers;

public class EstoqueController : Controller
{
    private readonly IMediator _mediator;
    public EstoqueController(IMediator mediator) 
    {
        _mediator = mediator;
    }
    
}