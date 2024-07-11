﻿using DWebProjFinal.Models;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace dotNet8CRUDWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenGenerateController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        //private SymmetricSecurityKey _key;


        public TokenGenerateController(IConfiguration configuration)
        {
            _configuration = configuration;

        }
        [HttpPost]
        public IActionResult Post(Login loginRequest)
        {
            if (loginRequest.email == "Admin" || loginRequest.password == "Passw0rd")
            {
                var issuer = _configuration["JwtSetting:Issuer"];
                var audience = _configuration["JwtSetting:Audience"];
                var _key = Encoding.ASCII.GetBytes(_configuration["JwtSetting:Key"]);


                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new[]
                    {
                        new Claim("Id",Guid.NewGuid().ToString()),
                        new Claim(JwtRegisteredClaimNames.Sub, loginRequest.email),
                        new Claim(JwtRegisteredClaimNames.Email, loginRequest.email),
                        new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString())
                   }),

                    Expires = DateTime.UtcNow.AddMinutes(10),
                    Issuer = issuer,
                    Audience = audience,
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(_key), SecurityAlgorithms.HmacSha256Signature)
                };



                var tokenHandler = new JwtSecurityTokenHandler();
                var token = tokenHandler.CreateToken(tokenDescriptor);
                var jwttoken = tokenHandler.WriteToken(token);
                var stringToken = tokenHandler.WriteToken(token);
                return Ok(stringToken);
            }


            return Ok("Unauthorized");
        }
    }
}