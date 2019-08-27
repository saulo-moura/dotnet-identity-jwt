using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using DotnetJwtAuth.Entities;
using DotnetJwtAuth.Helpers;
using DotnetJwtAuth.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;

namespace DotnetJwtAuth.Services
{
    public class AuthService : IAuthenticate
    {
        private readonly TokenConfiguration _tokenConfiguration; 
        private readonly UserManager<ApplicationUser> _userManager; 
        private readonly SignInManager<ApplicationUser> _signInManager;    

        public AuthService(IOptions<TokenConfiguration> tokenConfiguration, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _tokenConfiguration = tokenConfiguration.Value;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public ApplicationUser Authenticate(string username, string password)
        {
            var user = _userManager.FindByNameAsync(username).Result;

            // Retorna nulo caso não encontre o usuário
            if (user == null)
                return null;

            // Retorna erro ao não encontrar o usuário
            var loginResult = _signInManager.CheckPasswordSignInAsync(user, password, false).Result;

            // O login foi efetuado, então gera o token JWT
            if (loginResult.Succeeded)
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_tokenConfiguration.Secret);
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                        new Claim("sid", user.Id)
                    }),
                    Expires = DateTime.UtcNow.AddSeconds(_tokenConfiguration.TimeToExpire),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };
                var token = tokenHandler.CreateToken(tokenDescriptor);
                user.Token = tokenHandler.WriteToken(token);
                user.PasswordHash = null;
                user.SecurityStamp = null;
                user.ConcurrencyStamp = null;
                
                return user;
            }

            return null;
        }
    }
}