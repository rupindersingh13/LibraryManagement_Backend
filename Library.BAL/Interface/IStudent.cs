using LibraryAPI.MODEL.Db_Model;
using LibraryAPI.MODEL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryAPI.BAL.Interface
{
  public  interface IStudent
    {
        ResultModel<Student_Details> GetAllStudent_Details();
        ResultModel<Student_Details> GetStudent_Detail(int id);
        ResultModel<Student_Details> Add_StudentDetail(Student_Details _Details);
        ResultModel<Student_Details> UpdateStudent_Detail(Student_Details UpdData);
        ResultModel<Book_Images> DeleteStudent(int Id);
        Fine_Detail CheckFines(int Id);
        ResultModel<Student_Details> StudentRegistrationConfirmationMail(Student_Details details);
    }
}
