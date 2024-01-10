using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace WebApplication1.Models
{
    public class AppDbContext : IdentityDbContext<AppUser,AppRole,string>
    {
        public DbSet<Haber> habers { get; set; }
        public DbSet<Kategori> kategori { get; set; }

        public DbSet<Iletisim> ıletisims { get; set; }
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }
    }
}
