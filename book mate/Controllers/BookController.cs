using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services;
using BookMate.DataAccess.Data;
using ServiceContracts ;
using Microsoft.AspNetCore.Http;

using System;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using ServiceContracts.DTO;
using NuGet.Protocol;
using BookMate.Entities;
using Azure.Core;
using NuGet.Protocol.Plugins;
namespace book_mate.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {

        private readonly IBooksService _booksService;
        private ApplicationDbContext applicationDbContext;

        public BookController(IBooksService booksService) { 

            _booksService = booksService;

        }


        
        



        
        [HttpGet]
        [Route("/Books")]
        public IActionResult GetBookByTitle([FromQuery] string title = null, [FromQuery] Guid id = default(Guid) )
        {
            if (!string.IsNullOrEmpty(title))
            {
                // If title is provided, return the book with that title
                BookResponse? response = _booksService.GetBookByBookTitle(title);
                if (response != null)
                {
                    return Ok(response);
                }
            } 

            if (id != Guid.Empty)
            {
                // If id is provided, return the book with that id
                BookResponse? response = _booksService.GetBookByBookId(id);
                if (response != null)
                {
                    return Ok(response);
                }
            }

           


            List<BookResponse> Books = _booksService.GetAllBooks();

            return Ok(Books);


        }


        [HttpGet("/Books/categories")]
        public async Task<ActionResult<List<BookResponse>>> GetBooksByCategory([FromQuery] List<string> categoriesName )
        {
            if (categoriesName == null || categoriesName.Count == 0)
            {
                return BadRequest("Category IDs must be provided.");
            }

           
            var books = await _booksService.GetBooksByCategory(categoriesName);
            if (books.Count == 0)
            {
                return NotFound("No books found for the specified categories.");
            }
            return Ok(books);
        }







        [HttpPost]
        [Route("/addBook")]
        public async Task<IActionResult> addBook([FromForm] BookAddRequest? model)
        {

            if(model == null)
            {
                return BadRequest("The Title filed is required");
            }
            
            string? imageUrl = await GetImageUrl(file: model.ImageFile);
            string? pdfUrl = await GetPdfUrl(file: model.PdfFile);
            string? voiceUrl = await GetVoiceUrl(file: model.VoiceFile);


            BookAddRequest request = new BookAddRequest()
            {
                Title = model.Title,
                Author = model.Author,
                CategoryIds = model.CategoryIds,
                Description = model.Description,
                ImageUrl = imageUrl,
                PdfUrl = pdfUrl,
                VoiceUrl = voiceUrl,
                PublishedYear = model.PublishedYear,
                NumberOfPage = model.NumberOfPage,

            };

            BookResponse response = _booksService.AddBook(request);

            return new JsonResult("The book has been added successfully");


        }




        [HttpPost]
        [Route("editBook")]
        public async Task<IActionResult> EditBook([FromQuery] Guid id, [FromForm] BookAddRequest editedBook)
        {
            string? imageUrl = await GetImageUrl(file: editedBook.ImageFile);
            string? pdfUrl = await GetPdfUrl(file: editedBook.PdfFile);
            string? voiceUrl = await GetVoiceUrl(file: editedBook.VoiceFile);

            BookAddRequest request = new BookAddRequest()
            {
                Title = editedBook.Title,
                Author = editedBook.Author,
                CategoryIds = editedBook.CategoryIds,
                Description = editedBook.Description,
                ImageUrl = imageUrl,
                PdfUrl = pdfUrl,
                VoiceUrl = voiceUrl,
                PublishedYear = editedBook.PublishedYear,
                NumberOfPage = editedBook.NumberOfPage
            };

            await _booksService.EditBookAsync(id, request); // Await the EditBookAsync method call

            return Ok("Book edited successfully");
        }

















        private async Task<string> GetImageUrl(IFormFile? file)
        {

            if(file==null)
            {
                return null;
            }

            string filename = "";
            try
            {
                var extension = "." + file.FileName.Split('.')[file.FileName.Split('.').Length - 1];
                filename = DateTime.Now.Ticks.ToString() + extension;

                var filepath = Path.Combine(Directory.GetCurrentDirectory(), "C:\\dotNet\\book mate\\BookMate.DataAccess\\Upload\\Books\\Images\\");

                if (!Directory.Exists(filepath))
                {
                    Directory.CreateDirectory(filepath);
                }
           
                var exactpath = Path.Combine(Directory.GetCurrentDirectory(), "C:\\dotNet\\book mate\\BookMate.DataAccess\\Upload\\Books\\Images\\", filename);
                using (var stream = new FileStream(exactpath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
            }
            catch (Exception ex)
            {
            }
            return filename;
        }

        private async Task<string> GetPdfUrl(IFormFile? file)
        {
            if (file == null)
            {
                return null;
            }
            string filename = "";
            try
            {
                var extension = "." + file.FileName.Split('.')[file.FileName.Split('.').Length - 1];
                filename = DateTime.Now.Ticks.ToString() + extension;

                var filepath = Path.Combine(Directory.GetCurrentDirectory(), "C:\\dotNet\\book mate\\BookMate.DataAccess\\Upload\\Books\\Pdfs\\");

                if (!Directory.Exists(filepath))
                {
                    Directory.CreateDirectory(filepath);
                }

                var exactpath = Path.Combine(Directory.GetCurrentDirectory(), "C:\\dotNet\\book mate\\BookMate.DataAccess\\Upload\\Books\\Pdfs\\", filename);
                using (var stream = new FileStream(exactpath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
            }
            catch (Exception ex)
            {
            }
            return filename;
        }

        private async Task<string> GetVoiceUrl(IFormFile? file)
        {
            if (file == null)
            {
                return null;
            }
            string filename = "";
            try
            {
                var extension = "." + file.FileName.Split('.')[file.FileName.Split('.').Length - 1];
                filename = DateTime.Now.Ticks.ToString() + extension;

                var filepath = Path.Combine(Directory.GetCurrentDirectory(), "C:\\dotNet\\book mate\\BookMate.DataAccess\\Upload\\Books\\Voices\\");

                if (!Directory.Exists(filepath))
                {
                    Directory.CreateDirectory(filepath);
                }

                var exactpath = Path.Combine(Directory.GetCurrentDirectory(), "C:\\dotNet\\book mate\\BookMate.DataAccess\\Upload\\Books\\Voices\\", filename);
                using (var stream = new FileStream(exactpath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
            }
            catch (Exception ex)
            {
            }
            return filename;
        }



    }






}

