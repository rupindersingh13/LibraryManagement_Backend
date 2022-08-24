using LibraryAPI.MODEL.Db_Model;
using LibraryAPI.MODEL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryAPI.BAL.Interface
{
     public interface Auth
    {
        ResultModel<SignUp> Sign(SignUp data);
        ResultModel<Login> LoginIn(Login data);
        ResultModel<ResetPassword> ResetPasswordRequest(string Email);
        ResultModel<ResetPassword> ValidateOTP(String email, string otp);
        ResultModel<SignUp> UpdatePassword(SignUp data);


    }
}
