﻿using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Warehouse.Core.Repositories;
using WarehouseCore.Model;

namespace WarehouseCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]

    public class LoginController : ControllerBase
    {
        WarehouseRequestsRepositoryUnits Repository = new WarehouseRequestsRepositoryUnits();

        private IConfiguration _config;

        public LoginController(IConfiguration config)
        {
            _config = config;
        }
        [HttpPost]
        public IActionResult Login([FromBody]UserModel login)
        {
            IActionResult response = Unauthorized();
            var user = AuthenticateUser(login);

            if (user != null)
            {
                var tokenString = GenerateJSONWebToken(user);
                response = Ok(new { token = tokenString });
            }

            return response;
        }

        private string GenerateJSONWebToken(UserModel userInfo)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[] {
 new Claim(JwtRegisteredClaimNames.Sub, userInfo.Username), 
 new Claim("DateOfJoing", userInfo.DateOfJoing.ToString("yyyy-MM-dd")),
 new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
 };

            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
            _config["Jwt:Issuer"],
            claims,
            expires: DateTime.Now.AddMinutes(120),
            signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private UserModel AuthenticateUser(UserModel login)
        {
            UserModel user = null;
          var d= Repository.GetUserIdentityByName(login.Username, login.Password);
            //Validate the User Credentials 
            //Demo Purpose, I have Passed HardCoded User Information 
            if (d.UserId!= "000000000000000000000000")
            {
                user = new UserModel { Username = d.Name  };
            }
            return user;
        }
    }
}