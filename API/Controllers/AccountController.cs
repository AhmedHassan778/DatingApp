using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using API.Data;
using API.DTOs;
using API.Entities;
using API.interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{

    public class AccountController : BaseApiController
    {
        private readonly DataContext _context;
        private readonly ITokenService _tokenService;

        public AccountController(DataContext context, ITokenService tokenService)
        {
            _tokenService = tokenService;
            _context = context;
        }


        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {

            AppUser user = await _context.Users
            .Include(p=>p.Photos)
            .SingleOrDefaultAsync(x => x.UserName.ToLower() == loginDto.UserName.ToLower());

            if (user == null) return Unauthorized("Invalid User name");
            using var hmac = new HMACSHA512(user.PasswordSalt);
            var computedHmac = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));
            for (int i = 0; i < computedHmac.Length; i++)
                if (computedHmac[i] != user.PasswordHash[i]) return Unauthorized("Invalid Password");
            return new UserDto
            {
                UserName = user.UserName,
                token = _tokenService.CreateToken(user),
                PhotoUrl=user.Photos.FirstOrDefault(x=>x.IsMain)?.Url,
            };
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {

            if (await userExists(registerDto.UserName)) return BadRequest("User name is token");

            using var hmac = new HMACSHA512();

            AppUser user = new AppUser
            {
                UserName = registerDto.UserName,
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password)),
                PasswordSalt = hmac.Key,
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return new UserDto
            {
                UserName = user.UserName,
                token = _tokenService.CreateToken(user),
            };
        }

        private async Task<bool> userExists(string userName) 
        {
            return await _context.Users.AnyAsync(x => x.UserName.ToLower() == userName.ToLower());
        }

    }
}