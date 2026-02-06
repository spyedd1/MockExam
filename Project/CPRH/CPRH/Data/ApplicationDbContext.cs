using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using CPRH.Models;

namespace CPRH.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<CPRH.Models.Accessibility> Accessibility { get; set; } = default!;
        public DbSet<CPRH.Models.Booking> Booking { get; set; } = default!;
        public DbSet<CPRH.Models.Room> Room { get; set; } = default!;
        public DbSet<CPRH.Models.Staff> Staff { get; set; } = default!;
    }
}
