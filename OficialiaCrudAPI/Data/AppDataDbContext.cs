using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OficialiaCrudAPI.Models;

namespace OficialiaCrudAPI.Data
{
    public class AppDataDbContext : IdentityDbContext<IdentityUser>
    {
        public AppDataDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Usuario> Students { get; set; }
    }
}
