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
    public class TeacherRepository: ITeacherRepository
    {
        public ResultModel<AllDetail> GetallRecord()
        {
           // string to = "amn26333@gmail.com"; //To address    
            //string from = "rupindersingh5719@gmail.com"; //From address    
           // MailMessage message = new MailMessage(from, to);

            using (MailMessage mail = new MailMessage())
            {
                mail.From = new MailAddress("rupindersingh5719@gmail.com");
                mail.To.Add("amn26333@gmail.com");
                mail.Subject = "Hello";
                mail.Body = "<h1>Hello veer jii</h1>";
                mail.IsBodyHtml = true;
              //  mail.Attachments.Add(new Attachment("C:\\file.zip"));

                using (SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587))
                {
                    smtp.Credentials = new NetworkCredential("rupindersingh5719@gmail.com", "vmnxwxlcmjhlznhf");
                    smtp.EnableSsl = true;
                    try
                    {

                        
                        smtp.Send(mail);

                    }
                    catch (Exception ex)
                    {
                        //vmnxwxlcmjhlznhf
                    }
                }
            }
            ResultModel<AllDetail> Result = new ResultModel<AllDetail>();
            try
            {
                Result.lstModel = Context.ExeQueryList<AllDetail>("select *from Student_Details left join Book_Records on Student_Details.Student_Id=Book_Records.Student_Id;");
                return Result;
                 
            }
            catch (Exception ex)
            {
                Result.respStatus = false;
                Result.respMsg = ex.Message + ex.StackTrace.ToString();
            }
            return Result;
        }
        public ResultModel<Book_Records> GetRecord(int Id)
        {
            ResultModel<Book_Records> Result = new ResultModel<Book_Records>();
            try
            {
                Result.lstModel = Context.ExeQueryList<Book_Records>("Select *from Book_Records where Student_Id= " + Id);
                if(Result.model==null)
                {
                    Result.respMsg = "No Record found";
                    Result.respStatus = false;
                }
            }
            catch(Exception ex)
            {
                Result.respStatus = false;
            }
            return Result;
        }

        public ResultModel<Book_Records> GetRecordbyIds(int stdId,int Book_id)

        {
            ResultModel<Book_Records> Result = new ResultModel<Book_Records>();
            try
            {
                Dapper.DynamicParameters pars = new Dapper.DynamicParameters();
                pars.Add("@Student_Id", stdId);
                pars.Add("@Book_Id", Book_id);
                Result.model = Context.ExeScalarQuery<Book_Records>("select *from Book_Records where Student_Id=@Student_Id and Book_Id=@Book_Id", pars);
                if(Result.model==null)
                {
                    Result.respStatus = false;
                    Result.respMsg = "Please Enter Valid Id ";
                }
            }
            catch(Exception ex)
            {
                Result.respMsg = ex.StackTrace + ex.Message;
                Result.respStatus = false;
            }
            return Result;
        }
        public ResultModel<Book_Records> issue_book(Book_Records rec)
        {
            //Fine_Detail check = new Fine_Detail();
              Book_Records recbooks = new Book_Records();
            Book book = new Book();
            ResultModel<Book_Records> Result = new ResultModel<Book_Records>();
            Dapper.DynamicParameters parm = new Dapper.DynamicParameters();
            int checkFine = Context.ExeScalarQuery<int>("Select Fine from Fine_Detail Where Student_Id= " + rec.Student_Id) ;
            if (checkFine <=0  )
            {
             int checkStudent = Context.ExeScalarQuery<int>("select Student_Id from Student_Details where Student_Id= " + rec.Student_Id);

                if (checkStudent >0)
                {
                    parm.Add("@Book_Id",rec.Book_Id);
                    parm.Add("@Student_Id", rec.Student_Id);
                    recbooks = Context.ExeScalarQuery<Book_Records>("SELECT( SELECT COUNT(Book_Id)FROM   Book_Records where Book_Id=@Book_Id and Student_Id=@Student_Id) AS Book_Id,(SELECT COUNT(Student_Id)FROM   Book_Records where Student_Id= @Student_Id) AS Student_Id FROM Book_Records", parm);
                    
                    if(  recbooks==null|| recbooks.Student_Id < 2)
                    { 
                       if(recbooks == null||recbooks.Book_Id < 1 )
                        {
                            book = Context.ExeScalarQuery<Book>("Select Book_Id,Quantity from Book where Status=1 and Book_Id=" + rec.Book_Id);
                            if (book != null)
                            {
                                if (book.Quantity >= 1)
                                {

                                    List<Book_Records> issue = new List<Book_Records>();
                                    //Dapper.DynamicParameters parm = new Dapper.DynamicParameters();
                                    parm.Add("@Student_Id", rec.Student_Id);
                                    parm.Add("@Name", rec.Name);
                                    parm.Add("@Class", rec.Class);
                                    parm.Add("@Book_Name", rec.Book_Name);
                                    parm.Add("@Book_Id", rec.Book_Id);
                                    parm.Add("@Issue_Date", rec.Issue_Date);
                                    parm.Add("@Due_Date", rec.Issue_Date.AddDays(15));
                                    parm.Add("@Status", 1);

                                    try
                                    {
                                        Result.respStatus = Context.ExeQuery("Insert into Book_Records (Student_Id,Name,Class,Book_Name,Book_Id,Issue_Date,Due_Date,Status)" +
                                        "values(@Student_Id,@Name,@Class,@Book_Name,@Book_Id,@Issue_Date,@Due_Date,@Status)", parm) == 0 ? false : true;
                                        if (Result.respStatus)
                                        {
                                            parm.Add("@Quantity", book.Quantity - 1);
                                            parm.Add("@Book_Id", rec.Book_Id);
                                            Context.ExeQuery("update Book Set Quantity=@Quantity where Book_Id=@Book_Id ", parm);
                                        }
                                        Result.respMsg = "Book Issue Successfully";
                                    }
                                    catch (Exception ex)
                                    {
                                        Result.respStatus = false;
                                        Result.respMsg = ex.StackTrace + ex.Message;
                                    }




                                }
                                else
                                {
                                    Result.respStatus = false;
                                    Result.respMsg = "No book found";

                                }
                            }
                            else
                            {
                                Result.respMsg = "Book not exsist";
                                Result.respStatus = false;
                            }
                       }
                        else
                        {
                            Result.respMsg = "Already taken book";
                            Result.respStatus = false;
                        }
                        
                        
                    }
                    else
                    {
                        Result.respMsg = "Can't issue more than 2 books";
                        Result.respStatus = false;

                    }
                     

                    
                }
                else 
                {
                  Result.respStatus = false;
                  Result.respMsg = "Student doesn't Exsist";
                }
                
                
            }
             else
             {
                Result.respMsg = "Fine Pending: " + checkFine;
                 Result.respStatus = false;

            }

            return Result;
        }
        public Response DeleteRecord(int Id)
        {
             Response response = new Response();
             Book_Records DelBook = new Book_Records();
             Dapper.DynamicParameters pars = new Dapper.DynamicParameters();
             pars.Add("@Student_Id", Id);
              
            try
            {
             DelBook = Context.ExeScalarQuery<Book_Records>("Delete from Book_Records where Student_Id=@Student_Id", pars);
             response.result = "Student Record Deleted Successfully";
             response.status = true;

            }
            catch (Exception ex)
            {
             response.result = "Student Record  Deleted Unsuccessfully ";
             response.status = true;
            }
             return response;
        }
       public ResultModel<Book_Records> Checkfine()
        {
            ResultModel<Book_Records> result = new ResultModel<Book_Records>();
            var todayDate = DateTime.Today.AddDays(1);
             Dapper.DynamicParameters pars = new Dapper.DynamicParameters();
            pars.Add("@Date", todayDate);
          result.lstModel = Context.ExeQueryList<Book_Records>("select *from book_records where Due_Date=@Date", pars);
            if(result.lstModel!=null)
            {
                result.respMsg =  todayDate.ToString();   
            }
           
          return result;
       }

      

       //public Response submitBook(int Id)
       // {
       //     Response response = new Response();
       //     var check=CheckFines(Id);
       //     if (check != null)
       //     {

       //     }
       //     var submitDate = Context.ExeScalarQuery<Book_Records>("select Due_Date from Book_Records where Student_Id= "+Id);
       //     if (DateTime.Now <= submitDate.Due_Date  )
       //     {
       //         var delete_returnDetail = Context.ExeScalarQuery<Fine_Detail>("Delete from Book_Records where Student_Id=" + Id);
       //         var delete_studentRecord = Context.ExeScalarQuery<Book_Records>("Delete from Book_Records where Student_Id="+Id);
       //         response.result = "Book Submitted";
       //         return response;
       //     }
       //     else 
       //     {
       //       response.result = "Book not Submitted";
       //       response.status = false;
       //     }
       //     return response;
       // }

        public ResultModel<SubmitBookRecord>Submitbook(SubmitBookRecord submitRec)
        {
            ResultModel<SubmitBookRecord> Result = new ResultModel<SubmitBookRecord>();
            Dapper.DynamicParameters pars = new Dapper.DynamicParameters();
            try
            {
                Book_Records record = new Book_Records();
                pars.Add("@Student_Id", submitRec.Student_Id);
                pars.Add("@Book_Id", submitRec.Book_Id);
                record = Context.ExeScalarQuery<Book_Records>("select *from Book_Records where Student_Id=@Student_Id and Book_Id=@Book_Id ", pars);
                if(submitRec.Submit_Date <= record.Due_Date)
                {
                    // var fine = Context.ExeScalarQuery<int>("select fine from Fine_Detail where Student_Id=@Student_Id", pars);
                    pars.Add("@Name", record.Name);
                    pars.Add("@Class", record.Class);
                    pars.Add("@Book_Name", record.Book_Name);
                    pars.Add("@Issue_Date", record.Issue_Date);
                    pars.Add("@Due_Date", record.Due_Date);
                    pars.Add("@Submit_Date", DateTime.Now);
                    Result.respStatus = Context.ExeQuery("Insert into SubmitBookRecord (Student_Id,Book_Id,Name,Class,Book_Name,Issue_Date,Due_Date,Submit_Date)" +
                        "values(@Student_Id ,@Book_Id,@Name,@Class,@Book_Name,@Issue_Date,@Due_Date,@Submit_Date )", pars)==0 ? false : true;
                     
                    Context.ExeQuery("Update Book Set quantity=quantity+1 where Book_Id=@Book_Id", pars);
                    Result.respMsg = "No";
                }
                else
                {

                    var totelday = (DateTime.Now.Subtract(record.Issue_Date)).Days;
                    int current_days = 15;
                    int totel_fine = 0;


                    if (totelday > current_days)
                    {
                        var overday = totelday - current_days;
                        totel_fine = overday * 10;
                        pars.Add("@fine", totel_fine);
                        Context.ExeQuery("update Fine_Detail set fine=@fine where Student_Id=@Student_Id", pars);

                        Result.respMsg =  totel_fine.ToString();
                    }
                     
                }

            }

            catch
            {

            }

            return Result;

        }
        public bool CheckFines(int Id)
        {
                 
            Fine_Detail addfine = new Fine_Detail(); 
            
            var Book_Records = Context.ExeScalarQuery<Book_Records>("Select Issue_Date,Due_Date,Book_Id from Book_Records where Student_Id= " + Id);

            if (Book_Records != null)
            {
                var check = Context.ExeScalarQuery<Fine_Detail>("select Student_Id from Fine_Detail where Student_Id=" + Id);

                var totelday = (DateTime.Now.Subtract(Book_Records.Issue_Date)).Days;
                int current_days = 15;
                int totel_fine = 0;
               

                if (totelday > current_days)
                {
                    var overday = totelday - current_days;
                    totel_fine = overday * 10;
                    Dapper.DynamicParameters pars = new Dapper.DynamicParameters();
                    pars.Add("@Student_Id", Id);
                    pars.Add("@Return_Date", Book_Records.Due_Date);
                    pars.Add("@Book_Id", Book_Records.Book_Id);
                    pars.Add("@Fine", totel_fine);


                    if (check != null)
                    {
                        addfine = Context.ExeScalarQuery<Fine_Detail>("Update  Fine_Detail set  Student_Id=@Student_Id, Return_Date=@Return_Date, Book_Id=@Book_Id, Fine=@Fine  ", pars);
                        var result = Context.ExeScalarQuery<Fine_Detail>("Select *from Fine_Detail where Student_Id=" + Id);
                        
                    }
                    //else
                    //{

                    //    addfine = Context.ExeScalarQuery<Fine_Detail>("Insert into Fine_Detail (Return_Date,Fine, Student_Id,Book_Id)" +
                    //    "Values(@Return_Date, @Fine,@Student_Id,@Book_Id)", pars);
                    //    var result = Context.ExeScalarQuery<Fine_Detail>("Select *from Fine_Detail where Student_Id=" + Id);
                    //    return result;
                    //}


                }
            }
            return true;
        }
      
    }

}
