using System;
using System.ComponentModel.DataAnnotations;

namespace book_mate.Entities
{
    public class Book
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public string Title { get; set; }

        public string? Author  { get; set; }


        [Required]
        public string Categories { get; set; }
    

        public string? ImageUrl {  get; set; }

        public string? Description { get; set; }

        public int Number_Of_Page { get; set; }
        public int Published_Year { get; set; }

        public double Average_Rating { get; set; }

        public int Ratings_Count { get; set; }


    }
}
