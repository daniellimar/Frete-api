using Frete.Models;
using Microsoft.EntityFrameworkCore;

namespace Frete.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public virtual DbSet<CotacaoModel> Cotacao { get; set; }
		public virtual DbSet<ShippingServiceModel> ShippingService { get; set; }
	}
}