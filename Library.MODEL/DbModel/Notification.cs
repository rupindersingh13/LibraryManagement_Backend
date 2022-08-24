using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryAPI.MODEL.Db_Model
{
   public class Notification
    {
        public string Headline { get; set; }
        public int Status { get; set; }
        public DateTime CreatedTimeStamp { get; set; }
        public int Student_Id { get; set; }
    }
}
