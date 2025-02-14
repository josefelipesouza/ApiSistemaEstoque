namespace ApiSistemaEstoque.ApiSistemaEstoque.Authentication.Models;


public class AppSettings
{
    public string Segredo { get; set; } = string.Empty;
    public int ExpiracaoEmHoras { get; set; }
    public string Emissor { get; set; } = string.Empty;
    public string ValidoEm { get; set; } = string.Empty;
}