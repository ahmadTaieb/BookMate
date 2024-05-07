using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using BookMate.Entities;

namespace ServiceContracts.DTO
{
    public class BookAddRequest
    {

        public string? Title { get; set; }
        public string? Author { get; set; }
        public string? Category { get; set; }
        public string? ImageUrl { get; set; }

        public string? PdfUrl { get; set; }

        public string? VoiceUrl {  get; set; }

        public string? Description { get; set; }
        public int? NumberOfPage { get; set; }
        public int? PublishedYear { get; set; }
       


        public Book ToBook()
        {
            return new Book()
            {
                Title = Title,
                Author = Author,
                Category = Category,
                ImageUrl = ImageUrl,
                PdfUrl = PdfUrl,
                VoiceUrl = VoiceUrl,
                Description = Description,
                NumberOfPage = NumberOfPage,
                PublishedYear = PublishedYear,
               

               
            };
        }
    }
}
