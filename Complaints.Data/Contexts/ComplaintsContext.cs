using Complaints.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Complaints.Data.Contexts
{
    public class ComplaintsContext : DbContext
    {
        public ComplaintsContext(DbContextOptions<ComplaintsContext> options) : base(options)
        {

        }

        public DbSet<UserEntity> Users { get; set; }
        public DbSet<ComplaintEntity> Complaints { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserEntity>().ToTable("core_user");
            modelBuilder.Entity<ComplaintEntity>().ToTable("core_complaint");
        }
    }
}
