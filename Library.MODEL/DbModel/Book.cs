using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryAPI.MODEL.Db_Model
{
    public class Book
    {
        public int Book_Id { get; set; }           
        public string Book_Name { get; set; }
        public int Quantity { get; set; }
        public float Price  { get; set; }
        public string Author { get; set; }
        public string Description { get; set; }
        public int Status { get; set; }
        public string Image { get; set; }

    }
  
   
}