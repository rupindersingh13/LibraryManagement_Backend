using LibraryAPI.BAL.Interface;
using LibraryAPI.MODEL.Db_Model;
using LibraryAPI.MODEL.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Library_Managment.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]

    public class TeacherController : ControllerBase
    {
        private readonly ITeacherRepository _repo;

        public TeacherController(ITeacherRepository repo)
        {
            _repo = repo;
        }

        [HttpGet("Getall_Record")]
        public ResultModel<AllDetail> Getall_Record()
        {
            return _repo.GetallRecord();
        }

        [HttpGet("Get_Record{id}")]
        public ResultModel<Book_Records> GetRecord(int id)
        {
            return _repo.GetRecord(id);
        }

        [HttpGet("Getby_id")]
        public ResultModel<Book_Records> Getdata(int std_id,int book_id)
        {
            return _repo.GetRecordbyIds(std_id, book_id);

        }
         
      
        [HttpPost("Issue_Book")]
       public ResultModel<Book_Records> Issue_Book(Book_Records _Record)
        {
            return _repo.issue_book(_Record);
        }
      
        [HttpDelete("DeleteRecord{id}")]
        public Response DeleteRecord(int id)
        {
            return _repo.DeleteRecord(id);
        }

        [HttpPost("submitBook")]
        public ResultModel<SubmitBookRecord> Submitbook(SubmitBookRecord submitRec)
        {
            return _repo.Submitbook(submitRec);
        }
        [HttpGet("checkfine")]
        public ResultModel<Book_Records> Checkfine()
        {
            return _repo.Checkfine();
        }
    }
}
