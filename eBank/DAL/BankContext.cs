using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using eBank.Models;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace eBank.DAL
{
    public class BankContext : DbContext
    {
        public virtual DbSet<Account> Accounts { get; set; }
        public virtual DbSet<Classroom> Classrooms { get; set; }
        public virtual DbSet<Expense> Expenses { get; set; }
        public virtual DbSet<Purchase> Purchases { get; set; }
        public virtual DbSet<Store> Stores { get; set; }
        public virtual DbSet<StoreItem> StoreItems { get; set; }
        public virtual DbSet<Student> Students { get; set; }
        public virtual DbSet<StudentExpense> StudentExpenses { get; set; }
        public virtual DbSet<Teacher> Teachers { get; set; }
        public DbSet<UserProfile> UserProfiles { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Membership>()
                .HasMany<Role>(r => r.Roles)
                .WithMany(u => u.Members)
                .Map(m =>
                {
                    m.ToTable("webpages_UsersInRoles");
                    m.MapLeftKey("UserId");
                    m.MapRightKey("RoleId");
                });
        }
    }
}