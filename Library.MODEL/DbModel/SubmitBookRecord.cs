using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryAPI.MODEL.Db_Model
{
    public class SubmitBookRecord
    {
        public int Student_Id { get; set; }
        public string Name { get; set; }
        public string Class { get; set; }
        public int Book_Id { get; set; }
        public string Book_Name { get; set; }
        public DateTime Due_Date { get; set; }
        public DateTime Issue_Date { get; set; }
        public DateTime Submit_Date { get; set; }
        public int Status { get; set; }

    }
        
}
