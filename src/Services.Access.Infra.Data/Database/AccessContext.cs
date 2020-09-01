using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Services.Access.Domain.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Access.Infra.Data.Database
{
    public class AccessContext : DbContext
    {
        public AccessContext(DbContextOptions<AccessContext> options) : base(options)
        {
        }

        public DbSet<Lock> Locks { get; set; }
        public DbSet<Site> Sites { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Lock>(e =>
            {
                e.HasKey(p => p.Id);
                e.Property(p => p.Title);
                e.Property(p => p.Description);
                e.Ignore(p => p.State);
                
                e.HasOne(p => p.Site)
                .WithMany()
                .HasForeignKey(p => p.SiteId)
                .HasPrincipalKey(s => s.Id);

                e.Property(p => p.Created).IsRequired();
                e.Property(p => p.Modified).IsRequired();
                
                e.ToTable("tbl_Locks");
            });

            modelBuilder.Entity<Site>(e => 
            {
                e.HasKey(p => p.Id);
                e.Property(p => p.Title);
                e.Property(p => p.Description);
                e.Ignore(p => p.State);

                e.HasMany(p => p.Locks)
                .WithOne(l => l.Site)
                .OnDelete(DeleteBehavior.ClientSetNull);

                e.Property(p => p.Created).IsRequired();
                e.Property(p => p.Modified).IsRequired();

                e.ToTable("tbl_Sites");
            });

            modelBuilder.Entity<Permission>(e => 
            {
                e.HasKey(p => p.Id);
                e.Property(p => p.PermissionSubject)
                .HasConversion<int>();

                e.Property(p => p.PermissionLevel)
                .HasConversion<int>();

                e.Property(p => p.UserId)
                .IsRequired();

                e.Property(p => p.ObjectId)
                .IsRequired();

                e.Property(p => p.Created).IsRequired();
                e.Property(p => p.Modified).IsRequired();

                e.ToTable("tbl_Permissions");
            });
        }


        private void ConfigureBaseProperties<T>(EntityTypeBuilder<T> e) where T : BaseDomainModel
        {
            e.HasKey(p => p.Id);

            e.Property(p => p.Created)
           .ValueGeneratedOnAdd()
           .HasDefaultValueSql("GETDATE()");

            e.Property(p => p.Modified)
            .ValueGeneratedOnAddOrUpdate()
            .HasDefaultValueSql("GETDATE()");
        }
    }
}
