using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace LibraryAPI.MODEL.Db_Model
{
    public class Login
    {

        [Required]
        public string Email { get; set; }

        [Required]
         
        public string Password { get; set; }

        

    }
    public class SignUp
    {
        [Required]
        public String Email { get; set; }

        [Required]
        [Compare("ConfirmPassword")]
        public String Password { get; set; }
        [Required]
        public string ConfirmPassword { get; set; }
        public DateTime CreatedOn { get;  set; }
        public int Status { get; set; }

    }

    public class ResetPassword
    {
        public string Email { get; set; }
        public string OTP { get; set; }
        public DateTime CreatedOn { get; set; }
        public int Status { get; set; }
        public DateTime ExpireTime { get; set; }
    }
}   
