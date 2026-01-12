using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;

namespace ApiSistemaEstoque.ApiSistemaEstoque.Authentication.Context
{
    public class AuthContextFactory : IDesignTimeDbContextFactory<AuthContext>
    {
        public AuthContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<AuthContext>();

            // ======================================================
            // 1) Montar o caminho do banco igual ao Program.cs
            // ======================================================
            var databaseFolder = Path.Combine(AppContext.BaseDirectory, "Banco");
            Directory.CreateDirectory(databaseFolder);

            var databasePath = Path.Combine(databaseFolder, "estoque.db");

            var connectionString = $"Data Source={databasePath}";

            optionsBuilder.UseSqlite(connectionString);


           // optionsBuilder.LogTo(Console.WriteLine).EnableSensitiveDataLogging();


            return new AuthContext(optionsBuilder.Options);
        }
    }
}
