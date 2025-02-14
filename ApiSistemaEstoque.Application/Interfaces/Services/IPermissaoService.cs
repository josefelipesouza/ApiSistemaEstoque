using ApiSistemaEstoque.ApiSistemaEstoque.Application.Contracts.Response;

namespace ApiSistemaEstoque.ApiSistemaEstoque.Application.Interfaces.Services; 


public interface IPermissaoService
{
    PermissoesResponseModel FormataPermissoesAplicacao();
}