using AuthenticationApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationApi.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        // Your DbSets here
        DbSet<User> User { get; set; }
        DbSet<UserClaim> UserClaims { get; set; }
        DbSet<UserRole> UserRoles { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var userRoles = new List<UserRole>
            {
                new UserRole { Id = 1, Value = "Admin" },
                new UserRole { Id = 2, Value = "User" }
            };

            // Mock data for Users
            var tempCurrentDate = new DateTime(2024, 05, 19, 22, 42, 59, DateTimeKind.Local);
            var users = new List<User>
            {
                new User { Id = 1, UserName = "admin", Password = "$2a$11$e6ubhLs.8H261Pn0ye0w4OO9npSXdOictGBuUpKEKH4RxngN86oDu", Active = true, Email = "admin@example.com", UserRoleId = 1, CreatedDate = tempCurrentDate.AddHours(1), UpdatedDate = tempCurrentDate.AddHours(1) },
                new User { Id = 2, UserName = "user1", Password = "$2a$11$e6ubhLs.8H261Pn0ye0w4OO9npSXdOictGBuUpKEKH4RxngN86oDu", Active = true, Email = "user1@example.com", UserRoleId = 2, CreatedDate = tempCurrentDate.AddHours(2), UpdatedDate = tempCurrentDate.AddHours(2) },
                new User { Id = 3, UserName = "user2", Password = "$2a$11$e6ubhLs.8H261Pn0ye0w4OO9npSXdOictGBuUpKEKH4RxngN86oDu", Active = true, Email = "user2@example.com", UserRoleId = 2, CreatedDate = tempCurrentDate.AddHours(3), UpdatedDate = tempCurrentDate.AddHours(3) }
            };

            modelBuilder.Entity<UserRole>().HasData(userRoles);
            modelBuilder.Entity<User>().HasData(users);

            base.OnModelCreating(modelBuilder);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var entries = ChangeTracker
                .Entries()
                .Where(e => e.Entity is BaseEntity && (
                        e.State == EntityState.Added
                        || e.State == EntityState.Modified));

            foreach (var entityEntry in entries)
            {
                ((BaseEntity)entityEntry.Entity).UpdatedDate = DateTime.Now;

                if (entityEntry.State == EntityState.Added)
                {
                    var createdDate = ((BaseEntity)entityEntry.Entity).CreatedDate;
                    if (createdDate == DateTime.MinValue) // e.CreatedDate == default(DateTime) 
                    {
                        ((BaseEntity)entityEntry.Entity).CreatedDate = DateTime.Now;
                    }
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }
    }
}
