﻿using BookMate.Entities;
using Microsoft.AspNetCore.Identity;
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

        public DbSet<Book> Books { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<Library> Librarys { get; set; }

        public DbSet<BookLibrary> BookLibraries { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);


            builder.Entity<Category>().HasData(new Category()
            { categoryID = 1, categoryName = "drama" });
            builder.Entity<Category>().HasData(new Category()
            { categoryID = 2, categoryName = "action" });

            //builder.Entity<ApplicationUser>()
            //    .HasMany(c => c.Clubs)
            //    .WithOne(u => u.ApplicationUser)
            //    .HasForeignKey(i => i.ApplicationUserId);

        }



    }
}
