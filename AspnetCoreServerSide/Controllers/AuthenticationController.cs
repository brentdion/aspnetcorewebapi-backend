using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using System.Dynamic;
using AspnetCoreServerSide.Objects.Models;

namespace AspnetCoreServerSide.Controllers
{
    [Produces("application/json")] 
    public class AuthenticationController : Controller
    {
        private SignInManager<IdentityUser> _signInManager;
        private UserManager<IdentityUser> _userManager;
        private IConfiguration _configuration;

        public AuthenticationController(
            SignInManager<IdentityUser> injectedSignInManager,
            UserManager<IdentityUser> injectedUserManager,
            IConfiguration injectedConfiguration
            )
        {
            _signInManager = injectedSignInManager;
            _userManager = injectedUserManager;
            _configuration = injectedConfiguration;
        }


        [HttpPost]
        [ActionName("Login")]
        public async Task<dynamic> Login([FromBody] LoginModel model)
        {
            dynamic loginResponse = new ExpandoObject();

            try
            {
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, false, false);

                if (result.Succeeded)
                {
                    var appUser = _userManager.Users.SingleOrDefault(r => r.Email == model.Email);
                    loginResponse.token = GenerateJwtToken(model.Email, appUser);
                    loginResponse.status = "Success";
                }
            }
            catch (Exception ex)
            {
                loginResponse.status = "Error";
                loginResponse.error = ex.Message;
            }

            return loginResponse;
        }//end method


        [HttpPost]
        [ActionName("Register")]
        public async Task<dynamic> Register([FromBody] RegistrationModel model)
        {
            dynamic registerResponse = new ExpandoObject();

            try
            {
                var user = new IdentityUser
                {
                    UserName = model.Email,
                    Email = model.Email
                };

                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, false);
                    registerResponse.token = GenerateJwtToken(model.Email, user);
                    registerResponse.status = "Success";
                }
            }
            catch (Exception ex)
            {
                registerResponse.status = "Error";
                registerResponse.error = ex.Message;
            }

            return registerResponse;
        }//end method


        [HttpGet]
        [ActionName("TestTokenValidity")]
        [Authorize]
        public bool TestTokenValidity()
        {
            //if in this method then the token is valid
            bool isTokenvalid = true;

            return isTokenvalid;
        }//end method



        private string GenerateJwtToken(string email, IdentityUser user)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddDays(Convert.ToDouble(_configuration["JwtExpireDays"]));

            var token = new JwtSecurityToken(
                _configuration["JwtIssuer"],
                _configuration["JwtIssuer"],
                claims,
                expires: expires,
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }//end method






    }//end class
}