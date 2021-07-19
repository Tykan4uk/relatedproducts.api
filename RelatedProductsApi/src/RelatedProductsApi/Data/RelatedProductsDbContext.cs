using Microsoft.EntityFrameworkCore;
using RelatedProductsApi.Data.Entities;

namespace RelatedProductsApi.Data
{
    public class RelatedProductsDbContext : DbContext
    {
        public RelatedProductsDbContext(DbContextOptions<RelatedProductsDbContext> options)
            : base(options)
        {
        }

        public DbSet<RelatedProductEntity> RelatedProducts { get; set; } = null!;
    }
}
