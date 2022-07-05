using AaZ_PsD.Model;
using AaZ_PsD.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace AaZ_PsD.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private IConfiguration _config;
        private AuthRepository _authRepository;

        public LoginController(IConfiguration config, AuthRepository authRepository)
        {
            _config = config;
            _authRepository = authRepository;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("/Login")]
        public IActionResult Login([FromBody] UserLogin userLogin)
        {
            var user = Authenticate(userLogin);

            if (user != null)
            {
                var token = Generate(user);
                return Ok(token);
            }

            return StatusCode(403, "Connexion invalide");
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("/Signup")]
        public IActionResult SignUp([FromBody] UserModel user)
        {
            if (user != null)
            {
                var passwordSalt = Helpers.PasswordSaltInBase64();
                var passwordHash = Helpers.PasswordToHashBase64(user.Password, passwordSalt);

                try
                {
                    _authRepository.SignUp(user, passwordSalt, passwordHash);

                    return CreatedAtAction("Login", new UserLogin()
                    {
                        Username = user.Mail,
                        Password = user.Password,
                    });
                }
                catch (Exception ex)
                {
                    return StatusCode(500);
                }
            }

            return StatusCode(500);
        }

        private string Generate(UserModel user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.Email, user.Mail),
                new Claim(ClaimTypes.GivenName, user.Forename),
                new Claim(ClaimTypes.Surname, user.Surname),
                new Claim(ClaimTypes.Role, user.Role.Name)
            };

            var token = new JwtSecurityToken(
              claims :claims,
              expires: DateTime.Now.AddMinutes(15),
              signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private UserModel Authenticate(UserLogin userLogin)
        {
            UserModel currentUser = _authRepository.GetUser(userLogin.Username);

            if (currentUser != null)
            {
                return currentUser;
            }
            else
            {
                bool isOk = Helpers.PasswordValidation(currentUser.Password, currentUser.PasswordKey, userLogin.Password);

                if (isOk)
                {
                    return currentUser;
                }
                else
                {
                    return null;
                }
            }
        }
    }

    

}
