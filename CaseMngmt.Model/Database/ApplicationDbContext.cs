using CaseMngmt.Models.ApplicationRoles;
using CaseMngmt.Models.ApplicationUsers;
using CaseMngmt.Models.CaseKeywords;
using CaseMngmt.Models.Cases;
using CaseMngmt.Models.Companies;
using CaseMngmt.Models.CompanyTemplates;
using CaseMngmt.Models.Customers;
using CaseMngmt.Models.KeywordRoles;
using CaseMngmt.Models.Keywords;
using CaseMngmt.Models.Templates;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Type = CaseMngmt.Models.Types.Type;

namespace CaseMngmt.Models.Database
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>
    {
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
            : base(options)
        {
        }


        public virtual DbSet<ApplicationRole> ApplicationRole { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Case>()
                .HasMany(e => e.Keywords)
                .WithMany(e => e.Cases)
                .UsingEntity<CaseKeyword>(
                    l => l.HasOne<Keyword>().WithMany().HasForeignKey(e => e.KeywordId),
                    r => r.HasOne<Case>().WithMany().HasForeignKey(e => e.CaseId))
                .HasKey(e => e.Id);

            modelBuilder.Entity<CompanyTemplate>()
                .HasKey(e => new { e.CompanyId, e.TemplateId });
            modelBuilder.Entity<KeywordRole>()
                .HasKey(e => new { e.KeywordId, e.RoleId });
            modelBuilder.Entity<KeywordRole>()
                .HasOne(e => e.ApplicationRole)
                .WithMany(e => e.KeywordRole)
                .HasForeignKey(e => new { e.RoleId });
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Company> Company { get; set; }
        public DbSet<Customer> Customer { get; set; }
        public DbSet<Type> Type { get; set; }
        public DbSet<Case> Case { get; set; }
        public DbSet<Template> Template { get; set; }
        public DbSet<Keyword> Keyword { get; set; }
        public DbSet<KeywordRole> KeywordRole { get; set; }
        public DbSet<CaseKeyword> CaseKeyword { get; set; }
        public DbSet<CompanyTemplate> CompanyTemplate { get; set; }
    }
}
