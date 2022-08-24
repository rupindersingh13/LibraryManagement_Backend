using LibraryAPI.BAL.Interface;
using LibraryAPI.DAL.Dapper;
using LibraryAPI.MODEL.Db_Model;
using LibraryAPI.MODEL.Model;
using System;
using System.Net;
using System.Net.Mail;

namespace LibraryAPI.BAL.Service
{
    public class LoginRepository : Auth
    {
        public ResultModel<Login> LoginIn(Login data)
            {
            ResultModel<Login> Result = new ResultModel<Login>();
            try
            {
                Dapper.DynamicParameters Pars = new Dapper.DynamicParameters();
                Pars.Add("@Password", data.Password);
                Pars.Add("@Email", data.Email);
                Result.model = Context.ExeScalarQuery<Login>("Select *from Signup Where Password=@Password and Email=@Email ", Pars);
                if (Result.model != null)
                {
                    Result.respMsg = "";
                    Result.respStatus = true;
                }
            }
            catch (Exception ex)
            {
                Result.respMsg = ex.StackTrace + ex.Message;
                Result.respStatus = false;
            }
            return Result;
        }
        public ResultModel<SignUp> Sign(SignUp data)
        {
            ResultModel<SignUp> Result = new ResultModel<SignUp>();
            Dapper.DynamicParameters Pars = new Dapper.DynamicParameters();
            Pars.Add("@Email", data.Email);
            Pars.Add("@Password", data.Password);
            Pars.Add("@ConfirmPassword", data.ConfirmPassword);
            Pars.Add("@CreatedOn", DateTime.Today);
            Pars.Add("@Status", 1);
            try
            {
                Result.respStatus = Context.ExeQuery("Insert into Signup (Email,Password,ConfirmPassword,CreatedOn,Status) " + "values(@Email,@Password,@ConfirmPassword,@CreatedOn,@Status)", Pars) == 0 ? false : true;

            }
            catch (Exception ex)
            {
                Result.respStatus = false;
                Result.respMsg = ex.Message + ex.StackTrace;
            }
            return Result;

        }
        public ResultModel<SignUp> UpdatePassword(SignUp data)
        {
            ResultModel<SignUp> Result = new ResultModel<SignUp>();
            Dapper.DynamicParameters Pars = new Dapper.DynamicParameters();
            Pars.Add("@Email", data.Email);
            Pars.Add("@Password", data.Password);
            Pars.Add("@ConfirmPassword", data.ConfirmPassword);
            Pars.Add("@CreatedOn", DateTime.Today);
            Pars.Add("@Status", 1);
            try
            {
                Result.respStatus = Context.ExeQuery("Update Signup set Password=@Password,ConfirmPassword=@ConfirmPassword,CreatedOn=@CreatedOn,Status=@Status where Email=@Email ", Pars) == 0 ? false : true;

            }
            catch (Exception ex)
            {
                Result.respStatus = false;
                Result.respMsg = ex.Message + ex.StackTrace;
            }
            return Result;

        }

        public ResultModel<ResetPassword> ResetPasswordRequest(string Email)
        {
            ResultModel<ResetPassword> ResPass = new ResultModel<ResetPassword>();
            ResultModel<Login> log = new ResultModel<Login>();
            Dapper.DynamicParameters pars = new Dapper.DynamicParameters();
            pars.Add("Email", Email);
            log.model = Context.ExeScalarQuery<Login>("select *from SignUp where Email like '%'+@email+'%'", pars);
            if (log.model != null)
            {
                string OTP = "";
                Random rd = new Random();
                   OTP = rd.Next(1000, 9998).ToString();

                using (MailMessage mail = new MailMessage())
                {
                    mail.From = new MailAddress("kpnlibrary13@gmail.com");
                    mail.To.Add(Email);
                    mail.Subject = "Verification";
                    mail.Body = "<h1> OTP </h1>" +
                        "<p>Dear Customer, as per your request we have generated  OTP" + " " + OTP + " " + "</p" +
                        "<p>Never share your OTP with others</p></br>" +
                        "<p>Please fill your One Time Password in the required field of the application.</p></br>"
                        +"<p>Please note!! Your OTP is valid for only 3 minutes</p>";

                    mail.IsBodyHtml = true;
                    //  mail.Attachments.Add(new Attachment("C:\\file.zip"));

                    using (SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587))
                    {// vmnxwxlcmjhlznhf mine
                        smtp.Credentials = new NetworkCredential("kpnlibrary13@gmail.com", "nlgbbnkvwxmvlerw");
                        smtp.EnableSsl = true;
                        
                        try
                        {
                               smtp.Send(mail);
                            //Result.respMsg = "Email Send Successfully";
                            pars.Add("@OTP", OTP);
                            pars.Add("@createdOn", DateTime.Now);
                            pars.Add("@status", 1);
                            pars.Add("@ExpireTime", DateTime.Now.AddMinutes(3));
                            ResPass.respStatus = Context.ExeQuery("insert into ResetPassword (Email,OTP,createdOn,status,ExpireTime)" + "values(@Email,@OTP,@createdOn,@status,@ExpireTime)", pars) == 0 ? false : true;
                            if (ResPass.respStatus)
                            {
                                ResPass.respStatus = true;
                                ResPass.respMsg = " OTP Sent Successfully";
                            }
                            else
                            {
                                ResPass.respStatus = false;
                                ResPass.respMsg = "Something Went Wrong";
                            }
                        }
                        
                        catch (Exception ex)
                        {
                            ResPass.respStatus = false;
                            ResPass.respMsg = "Something Went Wrong";
                        }
                    }
                };    
            }
            else
            {
                ResPass.respStatus = false;
                ResPass.respMsg = "Invalid Email";
            }
            return ResPass;
        }

        public ResultModel<ResetPassword>ValidateOTP(String email,string otp)
        {
            ResultModel<ResetPassword> result = new ResultModel<ResetPassword>();
            Dapper.DynamicParameters pars = new Dapper.DynamicParameters();
            pars.Add("@Email",email);
            pars.Add("@OTP", otp);
            pars.Add("@status", 0);
            result.model=Context.ExeScalarQuery<ResetPassword>("select * from ResetPassword Where Email=@Email AND OTP =@OTP AND status=1",pars);
            if(result.model!=null)
            {
               
                if (otp.Equals(result.model.OTP) )
                {
                    if(DateTime.Now<=result.model.ExpireTime)
                    {
                        result.respStatus = Context.ExeQuery("update ResetPassword set status=@status where Email=@Email and OTP =@OTP", pars) == 0 ? false : true;
                        result.respStatus = Context.ExeQuery("update SignUp set status=@status where Email=@Email ", pars) == 0 ? false : true;
                        result.respMsg = "Valid OTP";
                    }
                    else
                    {
                        result.respMsg = "OTP expired";
                        result.respStatus = false;
                    }
                    
                }
                else
                {
                    result.respMsg = "Invalid OTP";
                    result.respStatus = false;
                }
            }
            else
            {
                result.respMsg = "Invalid OTP";
                result.respStatus = false;
            }
            return result;
        }
    }
}

