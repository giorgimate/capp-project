using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserManagement.Domain.Models.Domain;

namespace UserManagement.Domain.Models.Domain
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options) { }
        public DbSet<User> users { get; set; }
        public DbSet<UserProfile> userProfiles { get; set; }
    }
}
