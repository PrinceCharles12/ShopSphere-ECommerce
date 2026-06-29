using Microsoft.AspNetCore.Mvc;
using ECommerceAPI.Data;
using ECommerceAPI.Models;
using ECommerceAPI.DTOs;
using Microsoft.AspNetCore.Authorization;

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

using Microsoft.IdentityModel.Tokens;
namespace ECommerceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;
        public AuthController(AppDbContext context,IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        [HttpPost("register")]
        public IActionResult Register(User user)
        {
            var existingUser = _context.Users.FirstOrDefault(u => u.Email == user.Email);

            if (existingUser != null)
            {
                return BadRequest("Email already exists");
            }

            // Hash Password
            user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);

            _context.Users.Add(user);
            _context.SaveChanges();

            return Ok(new
            {
                Message = "User Registered Successfully"
            });
        }

        [HttpPost("login")]
        public IActionResult Login(LoginDto loginDto)
        {
            var user = _context.Users.FirstOrDefault(u => u.Email == loginDto.Email);

            if (user == null)
            {
                return Unauthorized("Invalid Credentials");
            }

            bool passwordValid = BCrypt.Net.BCrypt.Verify(loginDto.Password,user.Password);

            if (!passwordValid)
            {
                return Unauthorized("Invalid Credentials");
            }

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier,user.Id.ToString()),
                new Claim(ClaimTypes.Name,user.Name),
                new Claim(ClaimTypes.Email,user.Email),
                new Claim(ClaimTypes.Role,user.Role)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));

            var creds = new SigningCredentials(key,SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(2),
                signingCredentials: creds
            );

            var jwtToken = new JwtSecurityTokenHandler().WriteToken(token);

            return Ok(new
            {
                Token = jwtToken,
                Role = user.Role
            });
        }
    
        [HttpGet("profile/{email}")]
        public IActionResult Profile(string email)
        {
            var user = _context.Users.FirstOrDefault(u => u.Email == email);

            if (user == null)
                return NotFound();

            return Ok(new UserProfileDto
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                Role = user.Role
            });
        }

        [HttpPut("profile/{email}")]
        public IActionResult UpdateProfile(string email, UpdateProfileDto dto)
        {
            var user = _context.Users.FirstOrDefault(u => u.Email == email);

            if (user == null)
                return NotFound();

            user.Name = dto.Name;
            user.Email = dto.Email;

            _context.SaveChanges();

            return Ok("Profile Updated");
        }

        [HttpPost("change-password/{email}")]
        public IActionResult ChangePassword(string email, ChangePasswordDto dto)
        {
            var user = _context.Users.FirstOrDefault(u => u.Email == email);

            if (user == null)
                return NotFound();

            bool validPassword = BCrypt.Net.BCrypt.Verify(dto.CurrentPassword, user.Password);

            if (!validPassword)
                return BadRequest("Current Password Incorrect");

            user.Password = BCrypt.Net.BCrypt.HashPassword(dto.NewPassword);

            _context.SaveChanges();

            return Ok("Password Changed Successfully");
        }
    }
}