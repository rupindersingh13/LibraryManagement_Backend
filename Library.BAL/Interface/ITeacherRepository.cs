using LibraryAPI.MODEL.Db_Model;
using LibraryAPI.MODEL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryAPI.BAL.Interface
{
   public interface ITeacherRepository
    {
        ResultModel<AllDetail> GetallRecord();
        ResultModel<Book_Records> GetRecord(int Id);
        ResultModel<Book_Records> issue_book(Book_Records rec);
        Response DeleteRecord(int Id);
        ResultModel<SubmitBookRecord> Submitbook(SubmitBookRecord submitRec);
        ResultModel<Book_Records> Checkfine();
        ResultModel<Book_Records> GetRecordbyIds(int stdId, int Book_id);

    }
}
