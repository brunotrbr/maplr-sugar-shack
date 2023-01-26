using maplr_api.Models;
using Microsoft.EntityFrameworkCore;

namespace maplr_api.Context
{
    public class MaplrContext : DbContext
    {
        public MaplrContext(DbContextOptions<MaplrContext> options) : base(options)
        {
        }

        protected override void OnConfiguring (DbContextOptionsBuilder optionsBuilder)
        {
            
        }
        public DbSet<MapleSyrup> MapleSyrup { get; set; }
        public DbSet<Orders> Orders { get; set; }
        public DbSet<Carts> Carts { get; set; }
    }
}
