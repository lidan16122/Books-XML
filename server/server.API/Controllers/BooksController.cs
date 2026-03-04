using server.BL.Services;
using server.Models.Entities;
using server.Models.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace server.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly BooksService booksService;
        public BooksController(BooksService _booksService)
        {
            booksService = _booksService;
        }
        [HttpGet]
        public ActionResult<List<Book>> GetBooks([FromQuery] int pageNumber, [FromQuery] int pageSize)
        {
            List<Book> result = booksService.GetAllBooks(pageNumber, pageSize);
            return Ok(result);
        }
        [HttpPost]
        public ActionResult<bool> AddBook([FromBody] Book newBook)
        {
            eStatusCodes result = booksService.AddNewBook(newBook);
            return StatusCode((int)result, result == eStatusCodes.Success);
        }
        [HttpPut]
        public ActionResult<bool> UpdateBook([FromBody] Book book)
        {
            eStatusCodes result = booksService.UpdateBook(book);
            return StatusCode((int)result, result == eStatusCodes.Success);
        }
        [HttpDelete("{isbn}")]
        public ActionResult<bool> DeleteBook([FromRoute] string isbn) 
        { 
            eStatusCodes result = booksService.DeleteBook(isbn);
            return StatusCode((int)result, result == eStatusCodes.Success);
        }
    }
}
