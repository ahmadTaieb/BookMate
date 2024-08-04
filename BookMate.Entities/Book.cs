using System;
using System.ComponentModel.DataAnnotations;

namespace BookMate.Entities
{
    public class Book
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [StringLength(40)]
        public string Title { get; set; }

        [StringLength(40)]
        public string? Author  { get; set; }
        
        public ICollection<Category>? Categories { get; set; }
        public string? ImageUrl {  get; set; }
     

        public string? PdfUrl { get; set; }

        public string? VoiceUrl { get; set; }

        public string? Description { get; set; }
        public int? NumberOfPages { get; set; }
        public string? PublishedYear { get; set; }

        public double? AverageRating { get; set; }

        public int? RatingsCount { get; set; }

        public int? ReadingCount { get; set; }


        public List<BookLibrary>? BookLibrary { get; set; } = new List<BookLibrary>();

        public List<BookFavorite> BookFavorite { get; set; }= new List<BookFavorite>();

    }
}
