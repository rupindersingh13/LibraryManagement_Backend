 
namespace LibraryAPI.MODEL.Db_Model
{
   public class Student_Details
    {
        
        public int Student_Id { get; set; }
        public string Name { get; set; }
        public string Class { get; set; }       
        public long Mobile_no { get; set; }
        public int Status { get; set; }

        public string Email { get; set; }
        public string Incharge { get; set; }
        public string Image { get; set; }

    }
}
