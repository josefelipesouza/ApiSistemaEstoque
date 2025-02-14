using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ApiSistemaEstoque.ApiSistemaEstoque.Domain.Enums;

namespace ApiSistemaEstoque.ApiSistemaEstoque.Domain.Entities;

public class Usuario
{
    [Key]
    public int Codigo { get; private set; }
    public string Nome { get; private set; }
    public int CodigoSetor { get; private set;}
    public int UsuarioCadastro { get; private set; }
    public DateTime CreatedAt  { get; private set; }
    public DateTime updated_at {get; private set;}
    public IEnumerable<Status> Inativo { get; private set; }

    [ForeignKey(nameof(CodigoSetor))]
    public Setor Setor { get; private set; }

    public Usuario(string nome, int codigoSetor, int usuarioCadastro )
    {
        Nome = nome;
        UsuarioCadastro = usuarioCadastro;
        CodigoSetor = codigoSetor;
        CreatedAt =  DateTime.UtcNow;
        Inativo = new List<Status> { Status.Ativo };
    }




}