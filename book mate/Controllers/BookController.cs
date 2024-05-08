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
        [Route("/GetAllBooks")]
        public IActionResult Index()
        {

            List<BookResponse> Books=_booksService.GetAllBooks();

            return Ok(Books);
        }



        [HttpGet]
        [Route("/GetBookByTitle")]
        public IActionResult GetBookByTitle(string title) {
            
            BookResponse? response =_booksService.GetBookByBookTitle(title);

            return Ok(response);
        }

        [HttpGet]
        [Route("/GetBooksByCategory")]
        
        public IActionResult GetBooksByCategory(string? category)
        {

            if(category == null)
            {
                return BadRequest("Category is null");
            }

            List<BookResponse> Books=_booksService.GetBooksByCategory(category);

            if(Books.Count == 0)
            {
                return Ok("This category is Empty");

            }

            return Ok(Books);



        }




        [HttpPost]
        [Route("/addBook")]
        public async Task<IActionResult> addBook([FromForm] BookAddRequestWithFiles? model)
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
                Category = model.Category,
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

