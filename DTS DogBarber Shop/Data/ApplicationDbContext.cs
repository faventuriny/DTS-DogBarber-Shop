using DTS_DogBarber_Shop.Data.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


namespace DTS_DogBarber_Shop.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public DbSet<AppointmentIdentity> Queue { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    }
}
