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
        
    public class StudentController : ControllerBase
    {
        private readonly IStudent _repo;

        public StudentController(IStudent repo)
        {
            _repo = repo;
        }

        [HttpGet("Getallrecords")]
        public ResultModel<Student_Details> GetAllDetails()
        {
            return _repo.GetAllStudent_Details();
        }

        [HttpGet("GetMyDetail{id}")]
        public ResultModel<Student_Details> GetStudent_Detail(int id)
        {
            return _repo.GetStudent_Detail(id);
        }

        [HttpPost("RegisterStudentDetail")]
        public ResultModel<Student_Details> Add_StudentDetail(Student_Details data)
        {
            return _repo.Add_StudentDetail(data);
        }

        [HttpPut("UpdateMyDetails")]
        public ResultModel<Student_Details> UpdateMyDetails(Student_Details up_Detail)
        {
            return _repo.UpdateStudent_Detail(up_Detail);
        }

        [HttpGet("CheckFine{id}")]
        public Fine_Detail CheckFine(int id)
        {
            return _repo.CheckFines(id);
        }
        [HttpDelete("DeleteStudent{Id}")]
       public ResultModel<Book_Images> DeleteStudent(int Id)
        {
            return _repo.DeleteStudent(Id);
        }
        [HttpPost("SendRegistrationEmail")]
        public ResultModel<Student_Details> SendRegistrationEmail(Student_Details details)
        {
           return _repo.StudentRegistrationConfirmationMail( details);
        }


    }
}
