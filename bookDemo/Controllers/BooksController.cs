using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using bookDemo.Data;
using bookDemo.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace bookDemo.Controllers
{
    [Route("api/books")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetAllBooks()
        {
            var books = ApplicationContext.Books;
            return Ok(books);
        }

        [HttpGet("{id:int}")]
        public IActionResult GetOneBook([FromRoute(Name = "id")] int id)
        {
            var book = ApplicationContext
                .Books
                .Where(b => b.Id.Equals(id))
                .SingleOrDefault();
            if (book is null) return NotFound(); //404

            return Ok(book);
        }

        [HttpPost]
        public IActionResult CreateOneBook([FromBody] Book book)
        {
            try
            {
                if (book is null)
                {
                    return BadRequest(); //400;
                }
                ApplicationContext.Books.Add(book);
                return StatusCode(201, book);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id:int}")]
        public IActionResult UpdateOneBook([FromRoute] int id, [FromBody] Book book)
        {
            //check db has a book??
            var dbBook = ApplicationContext.Books.Find(b => b.Id.Equals(id));

            if (dbBook is null) return NotFound();  //404


            if (id != book.Id) return BadRequest(); //400

            ApplicationContext.Books.Remove(dbBook);
            book.Id = dbBook.Id;
            ApplicationContext.Books.Add(book);
            return Ok(book);    //200
        }
    }
}

