using LibraryAPI.BAL.Interface;
using LibraryAPI.DAL.Dapper;
using LibraryAPI.MODEL.Db_Model;
using LibraryAPI.MODEL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace LibraryAPI.BAL.Service
{
   public class StudentRepository : IStudent
    {
        public ResultModel<Student_Details> GetAllStudent_Details()
        {
            ResultModel<Student_Details> Result = new ResultModel<Student_Details>();
            Result.lstModel = Context.ExeQueryList<Student_Details>("select  b.*,b1.Image  from Student_Details b left join student_image b1 on b.Student_Id=b1.Student_Id where b.status=1 ORDER BY b.Student_Id");
            return Result;
        }
        public ResultModel<Student_Details> GetStudent_Detail(int id)
        {
            ResultModel<Student_Details> Result = new ResultModel<Student_Details>();
            Dapper.DynamicParameters pars = new Dapper.DynamicParameters();
            pars.Add("@Student_Id", id);
            pars.Add("@Status", 1);
            Result.model = Context.ExeScalarQuery<Student_Details>("select *from Student_Details where Student_Id=@Student_Id and Status=@Status",pars);

            return Result;
        }

       public ResultModel<Student_Details> StudentRegistrationConfirmationMail(Student_Details details)
        {
            ResultModel<Student_Details> Result = new ResultModel<Student_Details>();
            using (MailMessage mail = new MailMessage())
            {
                mail.From = new MailAddress("kpnlibrary13@gmail.com");
                mail.To.Add(details.Email);
                mail.Subject = "Successfully Registered";
                mail.Body = "<h1>Thankyou"+" "+details.Name+" "+"for Being a part of KPN Library </h1>" +
                    "<p>Your registration has been recorded.</p> " +
                    "<p>Our Admissions team will soon provide you Student Library Card.</p></br>" +
                    "<p> Looking forward to seeing you!</p></br>";

                mail.IsBodyHtml = true;
                //  mail.Attachments.Add(new Attachment("C:\\file.zip"));

                using (SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587))
                {// vmnxwxlcmjhlznhf mine
                    smtp.Credentials = new NetworkCredential("kpnlibrary13@gmail.com", "nlgbbnkvwxmvlerw");
                    smtp.EnableSsl = true;
                    try
                    {
                        smtp.Send(mail);
                        //Result.respMsg = "Email Send Successfully";
                    }
                    catch (Exception ex)
                    {
                        Result.respMsg = ex.Message + ex.StackTrace;
                    }
                }
            };

            return Result;

        }
        public ResultModel<Student_Details> Add_StudentDetail(Student_Details _Details)
        {
            
            ResultModel<Student_Details> Result = new ResultModel<Student_Details>();


            try
            {
                Dapper.DynamicParameters pars = new Dapper.DynamicParameters();
              // 
                pars.Add("@Class ", _Details.Class);
                pars.Add("@Name", _Details.Name);
                pars.Add("@Mobile_no", _Details.Mobile_no);
                pars.Add("@Email", _Details.Email);
                pars.Add("@Incharge", _Details.Incharge);
                pars.Add("@Image", _Details.Image);
                pars.Add("@Status", 1);

                int studentId = Context.ExeScalarQuery<int>("Insert into Student_Details (  Class, Name, Mobile_no, Email, Incharge, Status )" +
                "Values(  @Class,@Name,@Mobile_no,@Email,@Incharge ,@Status) Select SCOPE_IDENTITY()", pars) ;
               
                pars.Add("@Student_Id", studentId);

                Context.ExeQuery("Insert into Student_Image ( Student_Id,Image,Status )" +
               "Values( @Student_Id,@Image,@Status)", pars);


                

                Result.respMsg = "Student added successfully";
                Result.respStatus = true;
                
            }
            catch(Exception ex)
            {
                Result.respStatus = false;
                Result.respMsg = ex.Message + ex.StackTrace;
            }
           return Result;
            
        }
        
        public ResultModel<Student_Details> UpdateStudent_Detail(Student_Details UpdData)
        {
            ResultModel<Student_Details> Result = new ResultModel<Student_Details>();
            
            Dapper.DynamicParameters pars = new Dapper.DynamicParameters();
            pars.Add("@Student_Id", UpdData.Student_Id);
            pars.Add("@Class ", UpdData.Class);
            pars.Add("@Name", UpdData.Name);
            pars.Add("@Mobile_no", UpdData.Mobile_no);
            pars.Add("@Email", UpdData.Email);
            pars.Add("@Incharge", UpdData.Incharge);
            pars.Add("@Image", UpdData.Image);
            pars.Add("@Status", 1);
            try
            {
               Result.respStatus=  Context.ExeQuery("Update Student_Details set  Class=@Class, Name=@Name,  Mobile_no=@Mobile_no,  Email=@Email ,Incharge=@Incharge , Status=@Status where Student_Id=@Student_Id", pars) == 0 ? false : true;
                if (UpdData.Image != null)
                {
                    Result.respStatus = Context.ExeQuery("Update Student_Image set Image=@Image ,Status=@Status   where Student_Id=@Student_Id", pars) == 0 ? false : true;
                }
                if(Result.respStatus=true)
                {
                    Result.respMsg = "updated successfully";
                }
            }
            catch (Exception ex)
            {
                Result.respMsg = "error";
                Result.respStatus = false;
            }

            return Result;
        }
        
       public Fine_Detail CheckFines(int Id)
        {

            Fine_Detail addfine = new Fine_Detail();

            var student_Record = Context.ExeScalarQuery<Book_Records>("Select Issue_Date,Due_Date,Book_Id from Student_Record where Student_Id= " + Id);

            if (student_Record != null)
            {
                var check = Context.ExeScalarQuery<Fine_Detail>("select Student_Id from Return_Detail where Student_Id=" + Id);

                var totelday = (DateTime.Now.Subtract(student_Record.Issue_Date)).Days;
                int current_days = 15;
                int totel_fine = 0;

                if (totelday > current_days)
                {
                    var overday = totelday - current_days;
                    totel_fine = overday * 10;
                    Dapper.DynamicParameters pars = new Dapper.DynamicParameters();
                    pars.Add("@Student_Id", Id);
                    pars.Add("@Return_Date", student_Record.Due_Date);
                    pars.Add("@Book_Id", student_Record.Book_Id);
                    pars.Add("@Fine", totel_fine);


                    if (check != null)
                    {
                        addfine = Context.ExeScalarQuery<Fine_Detail>("Update  Return_Detail set  Student_Id=@Student_Id, Return_Date=@Return_Date, Book_Id=@Book_Id, Fine=@Fine  ", pars);
                    }
                    else
                    {

                        addfine = Context.ExeScalarQuery<Fine_Detail>("Insert into Return_Detail (Return_Date,Fine, Student_Id,Book_Id)" +
                        "Values(@Return_Date, @Fine,@Student_Id,@Book_Id)", pars);
                    }

                }
            }
            var result = Context.ExeScalarQuery<Fine_Detail>("Select *from Return_Detail where Student_Id="+Id);
           
            return result;
        }
        public ResultModel<Book_Images> DeleteStudent(int Id)
        {
            ResultModel<Book_Images> Result = new ResultModel<Book_Images>();
            Dapper.DynamicParameters pars = new Dapper.DynamicParameters();
            pars.Add("@Student_Id", Id);
            pars.Add("@Status", 0);
            try
            {
                Result.respStatus = Context.ExeQuery("update Student_Details set Status = @Status where Student_Id=@Student_Id", pars) == 0 ? false : true;
                Result.respStatus = Context.ExeQuery("update Student_Image set Status = @Status where Student_Id=@Student_Id", pars) == 0 ? false : true;
                if (Result.respStatus=true)
                {
                    Result.respMsg = "Book Deleted Successfully";
                    Result.respStatus = true;
                }



            }
            catch (Exception ex)
            {
                Result.respMsg = "Book Not Deleted ";
                Result.respStatus = false;
            }
            return Result;
        }

    }
}
