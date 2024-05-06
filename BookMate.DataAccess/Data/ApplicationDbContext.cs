using book_mate.Entities;
using BookMate.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;


namespace BookMate.DataAccess.Data
{
    public class ApplicationDbContext(DbContextOptions options) : IdentityDbContext<ApplicationUser>(options)
    {
        
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<Club> Clubs { get; set; }
        //public DbSet<ApplicationUserApplicationUser> Friends { get; set; }
        
        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<Club>()
        //        .HasOne(e => e.ApplicationUser)
        //        .WithMany(e => e.Clubs)
        //        .HasForeignKey(e => e.ApplicationUserId)
        //        .IsRequired();
        //}

    }
}
