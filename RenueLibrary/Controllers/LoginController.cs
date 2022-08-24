using LibraryAPI.BAL.Interface;
using LibraryAPI.MODEL.Db_Model;
using LibraryAPI.MODEL.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Library_Managment.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class LoginController : ControllerBase
    {
        private readonly Auth _repo;
        private readonly IConfiguration _configuration;
        public LoginController(Auth repo, IConfiguration  configuration)
        {
            _repo = repo;
            _configuration = configuration;
        }
        [HttpPost("Login")]
        public ResultModel<Login> Login(Login data)
        { 
            if (ModelState.IsValid)
            {
                ResultModel<Login> Result = new ResultModel<Login>();
                var userId = _repo.LoginIn(data);

                var authClaims = new[]
                {
                  new Claim(JwtRegisteredClaimNames.Sub,data.Email.Trim().ToLower()),
                   new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                };
                var signingkey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Key"]));
                var token = new JwtSecurityToken
                (
                     issuer: _configuration["JWT:Issuer"],
                     audience: _configuration["JWT:Audience"],
                     expires: DateTime.Now.AddHours(3),
                     claims: authClaims,
                     signingCredentials: new SigningCredentials(signingkey, SecurityAlgorithms.HmacSha256));
                     userId.respMsg = new JwtSecurityTokenHandler().WriteToken(token);
              //  var expiration = token.ValidTo;
                if (userId?.model == null)
                {
                    userId.respMsg = "invalid username and password";
                    userId.respStatus = false;
                }

                return userId;
            }
            return new ResultModel<Login>() { respStatus = false, respMsg = "Bad Request" };

        }
        [HttpPost("SingIn")]
        public ResultModel<SignUp>SignInto(SignUp data)
        {
            if(ModelState.IsValid)
            {
               return _repo.Sign(data);
            }
            else
            {
                return new ResultModel<SignUp>() { respMsg = ModelState.ErrorCount.ToString(), respStatus = false };
            }
        }
        [HttpPost("ResetPasswordRequest")]
        public ResultModel<ResetPassword> ResetPassRequest(string Email)
        {
            return _repo.ResetPasswordRequest(Email);
        }
        [HttpPost("ValidateOTP")]
        public ResultModel<ResetPassword> ValidateOTP(string Email,string otp)
        {
            return _repo.ValidateOTP(Email,otp);
        }
        [HttpPost("updatepassword")]
        public ResultModel<SignUp> updatepassword(SignUp data)
        {
            return _repo.UpdatePassword(data);
        }
        //private string GenerateToken(string username)
        //{
        //    var tokenhandler = new JwtSecurityTokenHandler();
        //    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Key"]));
        //    var credintial = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        //    var claims = new[]
        //    {
        //        new Claim(ClaimTypes.Email,username)
        //    };
        //    var token = new JwtSecurityToken(
        //         issuer: _configuration["JWT:ValidIssuer"],
        //             audience: _configuration["JWT:ValidAudience"],
        //             claims,
        //             signingCredentials:credintial
        //        );
        //    return tokenhandler.WriteToken(token)
        //}
    }
}
