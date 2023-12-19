using CaseMngmt.Models.ApplicationRoles;
using CaseMngmt.Models.ApplicationUsers;
using CaseMngmt.Models.CaseKeywords;
using CaseMngmt.Models.Cases;
using CaseMngmt.Models.Companies;
using CaseMngmt.Models.CompanyTemplates;
using CaseMngmt.Models.Customers;
using CaseMngmt.Models.Keywords;
using CaseMngmt.Models.Metadatas;
using CaseMngmt.Models.TemplateKeywords;
using CaseMngmt.Models.Templates;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Type = CaseMngmt.Models.Types.Type;
//using ContactManager.Models;

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
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Case>()
                .HasMany(e => e.Keywords)
                .WithMany(e => e.Cases)
                .UsingEntity<CaseKeyword>(
                    l => l.HasOne<Keyword>().WithMany().HasForeignKey(e => e.KeywordId),
                    r => r.HasOne<Case>().WithMany().HasForeignKey(e => e.CaseId))
                .HasKey(e => e.Id);

            modelBuilder.Entity<Template>()
               .HasMany(e => e.Keywords)
               .WithMany(e => e.Templates)
               .UsingEntity<TemplateKeyword>(
                   l => l.HasOne<Keyword>().WithMany().HasForeignKey(e => e.KeywordId),
                   r => r.HasOne<Template>().WithMany().HasForeignKey(e => e.TemplateId),
                   z => z.HasOne<ApplicationRole>().WithMany().HasForeignKey(e => e.RoleId))
               .HasKey(e => e.Id);

            //modelBuilder.Entity<Keyword>()
            //    .HasMany(e => e.Templates)
            //    .WithMany(e => e.Keywords)
            //    .UsingEntity<TemplateKeyword>(
            //        l => l.HasOne<Template>().WithMany().HasForeignKey(e => e.TemplateId),
            //        r => r.HasOne<Keyword>().WithMany().HasForeignKey(e => e.KeywordId))
            //    .HasKey(m => new { m.TemplateId , m.KeywordId });

            //modelBuilder.Entity<TemplateKeyword>()
            //    .Property(e => e.RoleId)
            //    .HasColumnName("RoleId");

            modelBuilder.Entity<CompanyTemplate>()
                .HasKey(e => new { e.CompanyId, e.TemplateId });

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Company> Company { get; set; }
        public DbSet<Customer> Customer { get; set; }
        public DbSet<Type> Type { get; set; }
        public DbSet<Metadata> Metadata { get; set; }
        public DbSet<Case> Case { get; set; }
        public DbSet<Template> Template { get; set; }
        public DbSet<Keyword> Keyword { get; set; }
        public DbSet<TemplateKeyword> TemplateKeyword { get; set; }
        public DbSet<CaseKeyword> CaseKeyword { get; set; }
        public DbSet<CompanyTemplate> CompanyTemplate { get; set; }
    }
}
