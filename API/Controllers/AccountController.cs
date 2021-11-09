using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using API.Data;
using API.DTOs;
using API.Entites;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class AccountController : BaseApiController
    {
        private readonly DataContext _context;
        public ITokenService TokenService { get; }
        private readonly ITokenService _tokenService;
        public AccountController(DataContext context, ITokenService tokenService)
        {
            _tokenService = tokenService;
            TokenService = tokenService;
            _context = context;
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto regiserDto)
        {
            if(await UserExists(regiserDto.Email)) return BadRequest("Username or Email is Taken");
            Random rnd = new Random();
            var hash = 0;
            bool hashExists = true;
            do
            {
                
                hash = rnd.Next(10000,10000000);
                var userByHash = _context.Users.Where(u => u.UserHash == hash).ToList();
                if(userByHash == null)
                {
                    hashExists = false;
                }

            }while(!hashExists);
            using var hmac = new HMACSHA512();
        
            var user = new AppUser
            {
                Email = regiserDto.Email.ToLower(),
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(regiserDto.Password)),
                PasswordSalt = hmac.Key,
                UserHash = hash
                
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return new UserDto
            {
                Email = user.Email,
                Token = _tokenService.CreateToken(user),
                UserHash = hash
                
            };

        } 
        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            var user = await _context.Users.SingleOrDefaultAsync( x => x.Email == loginDto.Email.ToLower());

            if(user == null) return Unauthorized("Invalid Email");

            using var hmac = new HMACSHA512(user.PasswordSalt);
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));
            for(int i = 0; i < computedHash.Length; i++)
            {
                if(computedHash[i] != user.PasswordHash[i]) return Unauthorized("Invalid password");
            }
            return new UserDto
            {
                Email = user.Email,
                Token = _tokenService.CreateToken(user)
            };
        }
        [HttpDelete("{hash}")]
        public async Task<ActionResult<UserDto>> DeleteAccount(int hash)
        {
            var user = await _context.Users.Where(u => u.UserHash == hash).FirstOrDefaultAsync();
            if(user != null)
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
                return Ok();
            }
            return BadRequest("User does not exist");
        }
        private async Task<bool> UserExists(string email)
        {
            return await _context.Users.AnyAsync(x => x.Email == email.ToLower());
        }
    }
}