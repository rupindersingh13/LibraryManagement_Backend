using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryAPI.MODEL.Db_Model
{
    public class Fine_Detail
    {
        public int Book_Id { get; set; }
        public int Student_Id { get; set; }
        public DateTime Return_Date  { get; set; }
        public float Fine { get; set; }
        public int Status { get; set; }
    }
}
