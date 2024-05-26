using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BookMate.Entities;
namespace ServiceContracts.DTO
{
    public class BookResponse
    {

        public Guid? Id { get; set; }
        public string? Title { get; set; }
        public string? Author { get; set; }
        public List<string>? Categories { get; set; }
        public string? ImageUrl { get; set; }

        public string? PdfUrl { get; set; }

        public string? VoiceUrl {  get; set; }
        public string? Description { get; set; }
        public int? NumberOfPages { get; set; }
        public string? PublishedYear { get; set; }
        public double? AverageRating { get; set; }
        public int? RatingsCount { get; set; }

        public int? ReadingCount { get; set; }
    }

    public static class BookExtension
    {

        public static BookResponse? ToBookResponse(this Book book)
        {
            return new BookResponse()
            {
                Id = book.Id,
                Title = book.Title,
                Author = book.Author,
                Categories = book.Categories?.Select(c => c.categoryName).ToList(),
                ImageUrl = book.ImageUrl,
                PdfUrl = book.PdfUrl,
                VoiceUrl = book.VoiceUrl,
                Description = book.Description,
                NumberOfPages = book.NumberOfPages,
                PublishedYear = book.PublishedYear,
                AverageRating = book.AverageRating,
                RatingsCount = book.RatingsCount,
                ReadingCount = book.ReadingCount,

            };

        }
    }
}
