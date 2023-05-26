using Frete.Models;
using Microsoft.EntityFrameworkCore;

namespace Frete.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public virtual DbSet<FreteModel> Cotacoes { get; set; }
    }
}