using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using DWebProjFinal.Models;

namespace DWebProjFinal.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<DWebProjFinal.Models.Utentes> Utentes { get; set; } = default!;
        public DbSet<DWebProjFinal.Models.Paginas> Paginas { get; set; } = default!;
        public DbSet<DWebProjFinal.Models.Categorias> Categorias { get; set; } = default!;
        public DbSet<DWebProjFinal.Models.LoginUtilizador> LoginUtilizador { get; set; } = default!;
    }
}
