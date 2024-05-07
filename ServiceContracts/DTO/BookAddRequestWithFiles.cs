using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace ServiceContracts.DTO
{
    public class BookAddRequestWithFiles
    {


        [Required]
        public string Title { get; set; }
         
       
        public string? Author { get; set; }

        public string? Category { get; set; }

        // Properties for file uploads
        public IFormFile? ImageFile { get; set; }
        public IFormFile? PdfFile { get; set; }
        public IFormFile? VoiceFile { get; set; }

       
        public string? Description { get; set; }
        public int? NumberOfPage { get; set; }
        public int? PublishedYear { get; set; }
    }
}
