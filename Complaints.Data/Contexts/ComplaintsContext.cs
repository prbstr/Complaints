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
    }
}
