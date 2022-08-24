using LibraryAPI.BAL.Interface;
using LibraryAPI.DAL.Dapper;
using LibraryAPI.MODEL.Db_Model;
using LibraryAPI.MODEL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
 //using Microsoft.AspNetCore.Authorization;

namespace LibraryAPI.BAL.Service
{
 
    public class BookRepository: IBookRepository
    {

        public ResultModel<Book> GetAllBooks()
        {
            ResultModel<Book> Result = new ResultModel<Book>();
             Result.lstModel = Context.ExeQueryList<Book>("select  b.*,b1.Image  from Book b left join Book_Images b1 on b.Book_id=b1.Book_id where b.status=1");
            return Result;
        }
        public ResultModel<Book> GetBook(int Id)
        {
            ResultModel<Book> Result = new ResultModel<Book>();
            //Dapper.DynamicParameters Pars = new Dapper.DynamicParameters();
            //Pars.Add("@Status")
            Result.model = Context.ExeScalarQuery<Book>("Select * from Book Where Status=1 and Book_Id=" + Id);
            if(Result.model==null)
            {
                Result.respStatus = false;
                Result.respMsg = "No book found";
            }
            return Result;
        }

        public ResultModel<Book> InsertBook(Book book )
        {
            ResultModel<Book> Result = new ResultModel<Book>();
            try
            {
                
                Dapper.DynamicParameters pars = new Dapper.DynamicParameters();
                 //pars.Add("@Book_Id", book.Book_Id);
                pars.Add("@Book_Name", book.Book_Name);
                pars.Add("@Description", book.Description);
                pars.Add("@Price ", book.Price);
                pars.Add("@Author", book.Author);
                pars.Add("@Quantity", book.Quantity);
                pars.Add("@Image", book.Image);
                pars.Add("@Status", 1);
                int book_id = Context.ExeScalarQuery<int>("Insert into Book (Book_Name, Description, Price, Author,Quantity,Status)" +
                "Values( @Book_Name, @Description , @Price, @Author,@Quantity,@Status ) Select SCOPE_IDENTITY()", pars);
                 
                pars.Add("@Book_Id", book_id);
               
                Result.respStatus = Context.ExeQuery("Insert into Book_Images (Book_Id, Image, Status )" +
               "Values( @Book_Id, @Image, @Status )", pars) == 0 ? false : true;

                if (Result.respStatus)
                {
                    Result.respMsg = "Book added Successfully";
                }
                

            }

            catch(Exception ex)
            {
                Result.respMsg=  ex.Message+ex.StackTrace;
                Result.respStatus = false;
            }
            return Result;

        }
        public ResultModel<Book> UpdateBook( Book UpdData )
        {
            ResultModel<Book> Result = new ResultModel<Book>();

            try
            {
                Dapper.DynamicParameters pars = new Dapper.DynamicParameters();
                pars.Add("@Book_Id", UpdData.Book_Id);
                pars.Add("@Book_Name", UpdData.Book_Name);
                pars.Add("@Description", UpdData.Description);
                pars.Add("@Price ", UpdData.Price);
                pars.Add("@Author", UpdData.Author);
                pars.Add("@Quantity", UpdData.Quantity);
                pars.Add("@Image", UpdData.Image);
                pars.Add("@Status",1);

                Result.respStatus = Context.ExeQuery("Update Book set Book_Name = @Book_Name, Description = @Description," +
                "Price = @Price, Author = @Author,Quantity=@Quantity,Status=@Status where Book_Id=@Book_Id", pars) == 0 ? false : true;
                if (UpdData.Image != null)
                {
                    Result.respStatus = Context.ExeQuery("Update Book_Images set Image = @Image, Status = @Status where Book_Id = @Book_Id", pars) == 0 ? false : true;
                }
               



            }


            catch (Exception ex)
            {
                Result.respStatus = false;
                Result.respMsg = "Book not Updated Succesfully";

            }

            return Result;
        }
        public ResultModel<Book_Images> DeleteBook(int Id)
        {
            ResultModel<Book_Images> Result = new ResultModel<Book_Images>();
            Dapper.DynamicParameters pars = new Dapper.DynamicParameters();
            pars.Add("@Book_Id", Id);
            pars.Add("@Status", 0);
            try
            {
                Result.respStatus= Context.ExeQuery("update Book set Status = @Status where Book_Id=@Book_Id",pars)== 0 ? false : true;
                Result.respStatus = Context.ExeQuery("update Book_Images set Status = @Status where Book_Id=@Book_Id", pars)== 0 ? false : true;
                if(Result.respStatus)
                {
                    Result.respMsg = "Book Deleted Successfully";
                }
                
                
                
            }
            catch(Exception ex)
            {
               Result.respMsg = "Book Not Deleted ";
               Result.respStatus = false;
            }
            return Result;
        }
       
    }

}
