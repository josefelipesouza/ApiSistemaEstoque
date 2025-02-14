
using ApiSistemaEstoque.ApiSistemaEstoque.Application.Contracts.Response;
using ApiSistemaEstoque.ApiSistemaEstoque.Application.Interfaces.Services;
using ApiSistemaEstoque.ApiSistemaEstoque.Domain.Enums;
using ApiSistemaEstoque.ApiSistemaEstoque.Domain.Extensions;

namespace ApiSistemaEstoque.ApiSistemaEstoque.Application.services;


public class PermissaoService : IPermissaoService
{
    public PermissoesResponseModel FormataPermissoesAplicacao()
    {
        var perfisDeAcesso = Enum.GetValues(typeof(PerfilDeAcessoEstoque))
            .Cast<PerfilDeAcessoEstoque>()
            .Select(s => new TipoValorPermissaoResponseModel(s.ToString(), s.ToString().SeparaStringPascalCase()))
            .ToList();

        return new PermissoesResponseModel(perfisDeAcesso);
    }
    
    public PermissoesResponseModel FormataPermissoesAplicacao(IEnumerable<PerfilDeAcessoEstoque> perfis)
    {
        var perfisDeAcesso = perfis
            .Select(s => new TipoValorPermissaoResponseModel(s.ToString(), s.ToString().SeparaStringPascalCase()))
            .ToList();

        return new PermissoesResponseModel(perfisDeAcesso);
    }
}