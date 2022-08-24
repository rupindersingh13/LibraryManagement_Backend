using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryAPI.MODEL.Db_Model
{
      public class Book_Records
      {
        public int Student_Id { get; set; }
        public string Name { get; set; }
        public string Class { get; set; }
        public int Book_Id { get; set; }
        public string Book_Name { get; set; }
        public DateTime Due_Date { get; set; }
        public DateTime Issue_Date { get; set; }
        public int Status { get; set; }


    }
    public class AllDetail
    {
        public int Student_Id { get; set; }
        public int Book_Id { get; set; }
        public string Book_Name { get; set; }
        public DateTime Due_Date { get; set; }
        public DateTime Issue_Date { get; set; }
      
        public string Name { get; set; }
        public string Class { get; set; }
        public long Mobile_no { get; set; }
         

    }
}
