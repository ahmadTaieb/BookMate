﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookMate.Entities;
using Microsoft.AspNetCore.Http;

namespace ServiceContracts.DTO
{
    public class BookAddRequest
    {


        [Required]
        public string Title { get; set; }
         
       
        public string? Author { get; set; }

        public List<int>? CategoryIds { get; set; }

        // Properties for file uploads
        public IFormFile? ImageFile { get; set; }
        public IFormFile? PdfFile { get; set; }
        public IFormFile? VoiceFile { get; set; }

       
        public string? Description { get; set; }
        public int? NumberOfPage { get; set; }
        public int? PublishedYear { get; set; }

        

        private string? ImageUrl { get; set; }

        private string? PdfUrl { get; set; }

        private string? VoiceUrl { get; set; }





        public Book ToBook(IEnumerable<Category>? allCategories)
        {



            
            // Create a new Book instance
            var book = new Book()
            {
             //   Id = Guid.NewGuid(),
                Title = Title,
                Author = Author,
                ImageUrl = ImageUrl,
                PdfUrl = PdfUrl,
                VoiceUrl = VoiceUrl,
                Description = Description,
                NumberOfPage = NumberOfPage,
                PublishedYear = PublishedYear,
               
            };

            if (CategoryIds != null)
            {


                book.Categories = allCategories.Where(c => this.CategoryIds.Contains(c.categoryID)).ToList();


            }
            else
            {
                book.Categories= new List<Category>();  
            }

        

            return book;
        }

        



    }
}
