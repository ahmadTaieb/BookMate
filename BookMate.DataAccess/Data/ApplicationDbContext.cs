using BookMate.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
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
        public DbSet<ApplicationUserClub> ApplicationUserClubs { get; set; }
        public DbSet<ApplicationUserRelation> ApplicationUserRelations { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);


            builder.Entity<Category>().HasData(new Category()
            { categoryID = 1, categoryName = "drama" });
            builder.Entity<Category>().HasData(new Category()
            { categoryID = 2, categoryName = "action" });

            builder.Entity<Book>().HasData(new Book()
            {
                Id = Guid.NewGuid(),
                Title = "Test1",
                Author = "Author1",
                NumberOfPages = 100,
            });

            builder.Entity<Book>().HasData(new Book()
            {
                Id = Guid.NewGuid(),
                Title = "Test2",
                Author = "Author2",
                NumberOfPages = 200,
            });
            builder.Entity<Book>().HasData(new Book()
            {
                Id = Guid.NewGuid(),
                Title = "Test3",
                Author = "Author3",
                NumberOfPages = 300,
            });
            builder.Entity<Book>().HasData(new Book()
            {
                Id = Guid.NewGuid(),
                Title = "Test4",
                Author = "Author4",
                NumberOfPages = 400,
            });
            builder.Entity<Book>().HasData(new Book()
            {
                Id = Guid.NewGuid(),
                Title = "Test5",
                Author = "Author5",
                NumberOfPages = 500,
            });

            //builder.Entity<ApplicationUser>()
            //    .HasMany(c => c.Clubs)
            //    .WithOne(u => u.ApplicationUser)
            //    .HasForeignKey(i => i.ApplicationUserId);



            //builder.Entity<ApplicationUserClub>()
            //    .HasKey(c => new { c.ApplicationUserId, c.ClubId });

            //builder.Entity<ApplicationUserClub>()
            //    .HasOne(c => c.ApplicationUser)
            //    .WithMany(c => c.ClubsMember)
            //    .IsRequired()
            //    .HasForeignKey(c => c.ApplicationUserId);

            //builder.Entity<ApplicationUserClub>()
            //    .HasOne(c => c.Club)
            //    .WithMany(c => c.ApplicationUsersMember)
            //    .IsRequired()
            //    .HasForeignKey(c => c.ClubId);

            builder.Entity<ApplicationUser>()
                .HasMany(x => x.ClubsMember)
                .WithOne(x => x.ApplicationUser)
                .OnDelete(DeleteBehavior.SetNull);

            builder.Entity<Club>()
                .HasMany(x => x.ApplicationUsersMember)
                .WithOne(x => x.Club)
                .OnDelete(DeleteBehavior.SetNull);

            builder.Entity<ApplicationUserRelation>()
                .HasKey(x => new { x.ApplicationUserParentId, x.ApplicationUserChildId });

            builder.Entity<ApplicationUserRelation>()
                .HasOne(x => x.ApplicationUserParent)
                .WithMany(x => x.Followers)
                .HasForeignKey(x => x.ApplicationUserParentId)
                .OnDelete(DeleteBehavior.Restrict);


            builder.Entity<ApplicationUserRelation>()
                .HasOne(x => x.ApplicationUserChild)
                .WithMany(x => x.Following)
                .HasForeignKey(x => x.ApplicationUserChildId)
                .OnDelete(DeleteBehavior.Restrict);

        }



    }
}
