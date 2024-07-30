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

        public DbSet<Library> Libraries { get; set; }

        public DbSet<Favorite>Favorites { get; set; }
        public DbSet<BookFavorite>BookFavorites { get; set; }

        public DbSet<BookLibrary> BookLibraries { get; set; }
        public DbSet<ApplicationUserClub> ApplicationUserClubs { get; set; }
        public DbSet<ApplicationUserRelation> ApplicationUserRelations { get; set; }

        public DbSet<Post> Posts { get; set; }
        public DbSet<Comment> Comments {  get; set; } 
        public DbSet<React> Reacts { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);


            builder.Entity<Category>().HasData(new Category()
            { categoryID = 1, categoryName = "Drama" });
            builder.Entity<Category>().HasData(new Category()
            { categoryID = 2, categoryName = "Action" });
            builder.Entity<Category>().HasData(new Category()
            { categoryID = 3, categoryName = "Fantasy" });
            builder.Entity<Category>().HasData(new Category()
            { categoryID = 4, categoryName = "Romance" });
            builder.Entity<Category>().HasData(new Category()
            { categoryID = 5, categoryName = "History" });
            builder.Entity<Category>().HasData(new Category()
            { categoryID = 6, categoryName = "Philosophy" });
            builder.Entity<Category>().HasData(new Category()
            { categoryID = 7, categoryName = "Science" });
           



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

            //builder.Entity<ApplicationUser>()
            //    .HasMany(x => x.ClubsMember)
            //    .WithOne(x => x.ApplicationUser)
            //    .OnDelete(DeleteBehavior.NoAction);
            ////DeleteBehavior.SetNull
            //builder.Entity<Club>()
            //    .HasMany(x => x.ApplicationUsersMember)
            //    .WithOne(x => x.Club)
            //    .OnDelete(DeleteBehavior.NoAction);

            //builder.Entity<Club>()
            //    .HasOne(x => x.ApplicationUser)
            //    .WithMany(x => x.Clubs)
            //    .OnDelete(DeleteBehavior.NoAction);

            //builder.Entity<ApplicationUserClub>()
            //    .HasOne(x => x.ApplicationUser)
            //    .WithMany(x => x.ClubsMember)
            //    .HasForeignKey(x => x.ApplicationUserId)
            //    .OnDelete(DeleteBehavior.SetNull);

            //builder.Entity<ApplicationUserClub>()
            //    .HasOne(x => x.Club)
            //    .WithMany(x => x.ApplicationUsersMember)
            //    .HasForeignKey(x => x.ClubId)
            //    .OnDelete(DeleteBehavior.SetNull);

            //builder.Entity<ApplicationUserRelation>()
            //    .HasKey(x => new { x.ApplicationUserParentId, x.ApplicationUserChildId });

            //builder.Entity<ApplicationUserRelation>()
            //    .HasOne(x => x.ApplicationUserParent)
            //    .WithMany(x => x.Followers)
            //    .HasForeignKey(x => x.ApplicationUserParentId)
            //    .OnDelete(DeleteBehavior.NoAction);


            //builder.Entity<ApplicationUserRelation>()
            //    .HasOne(x => x.ApplicationUserChild)
            //    .WithMany(x => x.Following)
            //    .HasForeignKey(x => x.ApplicationUserChildId)
            //    .OnDelete(DeleteBehavior.NoAction);
            ////////////////////////////////////////////////////////////////////////////////////
            ///
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

            builder.Entity<Club>()
                .HasOne(x => x.ApplicationUser)
                .WithMany(x => x.Clubs)
                .OnDelete(DeleteBehavior.SetNull);

            builder.Entity<ApplicationUserRelation>()
                .HasKey(x => new { x.ApplicationUserParentId, x.ApplicationUserChildId });

            builder.Entity<ApplicationUserRelation>()
                .HasOne(x => x.ApplicationUserParent)
                .WithMany(x => x.Followers)
                .HasForeignKey(x => x.ApplicationUserParentId)
                .OnDelete(DeleteBehavior.NoAction);


            builder.Entity<ApplicationUserRelation>()
                .HasOne(x => x.ApplicationUserChild)
                .WithMany(x => x.Following)
                .HasForeignKey(x => x.ApplicationUserChildId)
                .OnDelete(DeleteBehavior.NoAction);

        }



    }
}
