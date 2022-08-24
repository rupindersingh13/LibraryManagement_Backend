using LibraryAPI.MODEL.Db_Model;
using LibraryAPI.BAL.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibraryAPI.MODEL.Model;

namespace LibraryAPI.BAL.Interface
{
    public interface IBookRepository
    {
        ResultModel<Book> GetAllBooks();
        ResultModel<Book> GetBook(int Id);
        ResultModel<Book> InsertBook(Book book);
        ResultModel<Book> UpdateBook(Book UpdData );
        ResultModel<Book_Images> DeleteBook(int Id);
    }
 }  
