using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Monitoring.Db.IdentityModels;
using Monitoring.Db.Models;

namespace Monitoring.Db
{
    public class IdentityMonitoringDbContext : IdentityDbContext<ApplicationUser>
    {
        public IdentityMonitoringDbContext(DbContextOptions<IdentityMonitoringDbContext> options)
            : base(options)
        {
        }

        public DbSet<MessageEntity> MessageEntity { get; set; }
        public DbSet<Form> Forms { get; set; }
        public DbSet<Field> Fields { get; set; }
        public DbSet<FormField> FormFields { get; set; }
        public DbSet<FieldValue> FieldValues { get; set; }
        public DbSet<Puls> puls { get; set; }
        public DbSet<Board> boards { get; set; }
        public DbSet<PulseType> pulseTypes { get; set; }
        public DbSet<Pulsnature> pulsenatures { get; set; }
        public DbSet<Machine> machines { get; set; }
        public DbSet<ClientPuls> clientPuls { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Seed Roles
            string adminRoleId = Guid.NewGuid().ToString();
            string userRoleId = Guid.NewGuid().ToString();

            modelBuilder.Entity<IdentityRole>().HasData(
                new IdentityRole
                {
                    Id = adminRoleId,
                    Name = "Admin",
                    NormalizedName = "ADMIN"
                },
                new IdentityRole
                {
                    Id = userRoleId,
                    Name = "User",
                    NormalizedName = "USER"
                }
            );

            // Create a user
            string adminUserId = Guid.NewGuid().ToString();
            var hasher = new PasswordHasher<ApplicationUser>();
            modelBuilder.Entity<ApplicationUser>().HasData(
                new ApplicationUser
                {
                    Id = adminUserId,
                    UserName = "admin@admin.com",
                    NormalizedUserName = "ADMIN@ADMIN.COM",
                    Email = "admin@admin.com",
                    NormalizedEmail = "ADMIN@ADMIN.COM",
                    EmailConfirmed = true,
                    PasswordHash = hasher.HashPassword(null, "Admin@123"),
                    SecurityStamp = string.Empty
                }
            );

            // Assign roles to users
            modelBuilder.Entity<IdentityUserRole<string>>().HasData(
                new IdentityUserRole<string>
                {
                    RoleId = adminRoleId,
                    UserId = adminUserId
                }
            );

            // Additional configurations for other entities
            modelBuilder.Entity<MessageEntity>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
                entity.Property(e => e.Content).IsRequired();
                entity.Property(e => e.Status).IsRequired();
                entity.Property(e => e.MessageType).IsRequired();
                entity.Property(e => e.Timestamp).IsRequired();
            });

            modelBuilder.Entity<Form>(entity =>
            {
                entity.HasKey(f => f.FormId);
                entity.Property(f => f.Title).IsRequired();
                entity.Property(f => f.Description);
                entity.Property(f => f.CreatedDate).IsRequired();

                entity.HasMany(f => f.FormFields)
                      .WithOne(ff => ff.Form)
                      .HasForeignKey(ff => ff.FormId);
            });

            modelBuilder.Entity<Field>(entity =>
            {
                entity.HasKey(f => f.FieldId);
                entity.Property(f => f.Name).IsRequired();
                entity.Property(f => f.Type).IsRequired();
                entity.Property(f => f.Extension);
            });

            modelBuilder.Entity<FormField>(entity =>
            {
                entity.HasKey(ff => ff.FormFieldId);

                entity.HasOne(ff => ff.Form)
                      .WithMany(f => f.FormFields)
                      .HasForeignKey(ff => ff.FormId);

                entity.HasOne(ff => ff.Field)
                      .WithMany(f => f.FormFields)
                      .HasForeignKey(ff => ff.FieldId);

                entity.HasMany(ff => ff.FieldValues)
                      .WithOne(fv => fv.FormField)
                      .HasForeignKey(fv => fv.FormFieldId);
            });

            modelBuilder.Entity<FieldValue>(entity =>
            {
                entity.HasKey(fv => fv.FieldValueId);
                entity.Property(fv => fv.Value).IsRequired();
                entity.Property(fv => fv.SubmittedDate).IsRequired();

                entity.HasOne(fv => fv.FormField)
                      .WithMany(ff => ff.FieldValues)
                      .HasForeignKey(fv => fv.FormFieldId);
            });

            modelBuilder.Entity<Board>(entity =>
            {
                entity.HasKey(b => b.Id);
                entity.HasMany(b => b.Pulses)
               .WithOne(p => p.Board)
               .HasForeignKey(p => p.BoardId);
                entity.HasMany(b => b.ClientPulses)
               .WithOne(p => p.Board)
               .HasForeignKey(p => p.BoardId);
                entity.HasMany(b => b.machines)
                .WithOne(m => m.Board).HasForeignKey(m => m.BoardId);

            });

            modelBuilder.Entity<ClientPuls>(entity =>
            {
                entity.HasKey(p => p.Id);
                entity.HasOne(p => p.puls)
                   .WithMany()
                 .HasForeignKey(cd => cd.PulsId)
               .OnDelete(DeleteBehavior.Restrict); // Use Restrict to avoid cascading deletes

                entity.HasOne(p => p.Board)
                .WithMany(b => b.ClientPulses)
                .HasForeignKey(b => b.BoardId);

            });



            // Configure Puls entity
            modelBuilder.Entity<Puls>(entity =>
        {
            entity.HasKey(p => p.Id);

            entity.HasOne(p => p.Board)
                  .WithMany(b => b.Pulses)
                  .HasForeignKey(p => p.BoardId);

            entity.HasOne(p => p.PulseType)
                  .WithMany()
                  .HasForeignKey(p => p.PulseTypeId)
                  .OnDelete(DeleteBehavior.Restrict); // Use Restrict to avoid cascading deletes

            entity.HasOne(p => p.Nature)
                  .WithMany()
                  .HasForeignKey(p => p.PulsenatureId)
                  .OnDelete(DeleteBehavior.Restrict); // Use Restrict to avoid cascading deletes
        });

            modelBuilder.Entity<PulseType>().HasData(
            new PulseType { Id = 1, Name = "دستی", Description = "" },
            new PulseType { Id = 2, Name = "سخت افزاری", Description = "" }
        );

            modelBuilder.Entity<Pulsnature>().HasData(
           new Pulsnature { Id = 1, Name = "وضعیت", Description = "" },
           new Pulsnature { Id = 2, Name = "تعداد", Description = "" }
       );

            modelBuilder.Entity<Machine>(entity =>
            {
                entity.HasKey(f => f.Id);
            });


        }
    }
}
