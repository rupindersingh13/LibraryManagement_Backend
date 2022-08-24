using LibraryAPI.BAL.Interface;
using LibraryAPI.MODEL.Db_Model;
using LibraryAPI.MODEL.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
 
namespace Library_Managment.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]

    public class BookController : ControllerBase
    {
        private readonly IBookRepository _repo;

       
        public BookController(IBookRepository repo)
        {
            _repo = repo;
        }

        [HttpGet("Getall_Books")]
        public ResultModel<Book> Getall_Books()    
        {
            return _repo.GetAllBooks();
        }

        [HttpGet("Getbook{id}")]
        public ResultModel<Book> Getbook(int id)
        {
            return _repo.GetBook(id);
        }
        [HttpPost("InsertBook")]
        public ResultModel<Book> InsertBook(Book data )
        {
            return _repo.InsertBook(data );
        }
        [HttpPut("UpdateBook")]
        public ResultModel<Book> UpdateBook(Book UpdData )
        {
            return _repo.UpdateBook(UpdData );
        }
        [HttpDelete("DeleteBook{id}")]
        public ResultModel<Book_Images> DeleteBook(int Id)
        {
            return _repo.DeleteBook(Id);
        }

        
    }
}