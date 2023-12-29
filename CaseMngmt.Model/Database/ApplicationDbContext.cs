using CaseMngmt.Models.ApplicationRoles;
using CaseMngmt.Models.ApplicationUsers;
using CaseMngmt.Models.CaseKeywords;
using CaseMngmt.Models.Cases;
using CaseMngmt.Models.Companies;
using CaseMngmt.Models.CompanyTemplates;
using CaseMngmt.Models.Customers;
using CaseMngmt.Models.FileTypes;
using CaseMngmt.Models.KeywordRoles;
using CaseMngmt.Models.Keywords;
using CaseMngmt.Models.RoleFileTypes;
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
                  l => l.HasOne(e => e.Keyword).WithMany(e => e.CaseKeywords).HasForeignKey(e => e.KeywordId), //MapLeftKey
                  r => r.HasOne(e => e.Case).WithMany(e => e.CaseKeywords).HasForeignKey(e => e.CaseId)) //MapRightKey
              .HasKey(e => e.Id);

            modelBuilder.Entity<FileType>()
                .HasMany(e => e.Roles)
                .WithMany(e => e.FileTypes)
                .UsingEntity<RoleFileType>(
                    l => l.HasOne(e => e.ApplicationRole).WithMany(e => e.RoleFileTypes).HasForeignKey(e => e.RoleId), //MapLeftKey
                    r => r.HasOne(e => e.FileType).WithMany(e => e.RoleFileTypes).HasForeignKey(e => e.FileTypeId)) //MapRightKey
              .HasKey(e => new { e.RoleId, e.FileTypeId });

            modelBuilder.Entity<ApplicationRole>()
                .HasMany(e => e.Keywords)
                .WithMany(e => e.Roles)
                .UsingEntity<KeywordRole>(
                    l => l.HasOne(e => e.Keyword).WithMany(e => e.KeywordRoles).HasForeignKey(e => e.KeywordId), //MapLeftKey
                    r => r.HasOne(e => e.ApplicationRole).WithMany(e => e.KeywordRoles).HasForeignKey(e => e.RoleId)) //MapRightKey
              .HasKey(e => new { e.RoleId, e.KeywordId });

            modelBuilder.Entity<CompanyTemplate>()
                .HasKey(e => new { e.CompanyId, e.TemplateId });

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
        public DbSet<FileType> FileType { get; set; }
        public DbSet<RoleFileType> RoleFileType { get; set; }
    }
}
