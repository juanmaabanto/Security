using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using N5.Challenge.Services.Security.Domain.Entities;
using N5.Challenge.Services.Security.Domain.SeedWork;

namespace N5.Challenge.Services.Security.Infrastructure
{
    public class SecurityContext : DbContext, IUnitOfWork
    {
        #region DbSets

        public DbSet<Permission>? Permissions { get; set; }
        public DbSet<PermissionType>? PermissionTypes { get; set; }

        #endregion

        #region Constructors

        public SecurityContext(DbContextOptions<SecurityContext> options) : base(options) { }

        #endregion

        #region Methods

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<Permission>(
                    eb =>
                    {
                        eb.ToTable("Permissions");
                        eb.HasKey(v => v.Id);
                        eb.Property(v => v.EmployeeForename).HasColumnName("EmployeeForename");
                        eb.Property(v => v.EmployeeSurname).HasColumnName("EmployeeSurname");
                        eb.Property(v => v.PermissionDate).HasColumnName("PermissionDate");
                        eb.Property(v => v.PermissionTypeId).HasColumnName("PermissionType");
                    })
                .Entity<PermissionType>(
                    eb =>
                    {
                        eb.ToTable("dbo.PermissionTypes");
                        eb.HasKey(v => v.Id);
                        eb.Property(v => v.Description).HasColumnName("Description");
                    }
                );
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            return await base.SaveChangesAsync();
        }

        #endregion
    }
}