using CaseMngmt.Server.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
//using ContactManager.Models;

namespace CaseMngmt.Server.Data
{
    public class ApplicationDbContext: IdentityDbContext
    {
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
            : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
        public DbSet<Contact> Contact { get; set; }
    }
}


//using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore;
//using ContactManager.Models;

//namespace ContactManager.Data;

//public class ApplicationDbContext : IdentityDbContext
//{
//#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
//    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
//#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
//        : base(options)
//    {
//    }
//    public DbSet<Contact> Contact { get; set; }
//}