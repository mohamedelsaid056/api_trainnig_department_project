using department.DTO;
using department.models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace department.Controllers
{
    [Route("api/[controller]")]
    [ApiController]


   
    public class AccountController : ControllerBase
    {

        private readonly ApplicationDbContext _db;
        private readonly UserManager<Applicationuser> _userManager; 
        private readonly SignInManager<Applicationuser> _signInManager;
        private readonly IConfiguration _configuration;
        private readonly RoleManager<IdentityRole> _roleManager;


        public AccountController(ApplicationDbContext db, UserManager<Applicationuser> userManager, SignInManager<Applicationuser> signInManager, IConfiguration configuration, RoleManager<IdentityRole> roleManager)
        {
            _db = db;
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _roleManager = roleManager;
        }
        // add private method to generate token
        private string GenerateJwtToken(Applicationuser user)
        {
            // Implement JWT token generation logic here
            // Example:
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        //Register User

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto registerDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            //mapping the registerDTO to the applicationuser

            var user = new Applicationuser
            {
                UserName = registerDTO.Email,
                Email = registerDTO.Email,
                FirstName = registerDTO.FirstName,
                LastName = registerDTO.LastName
            };

            var result = await _userManager.CreateAsync(user, registerDTO.Password);

              if (result.Succeeded)
            {
                
            }

            return BadRequest(result.Errors);
        }
       

      // login

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto loginDTO)
        {
            var user = await _userManager.FindByEmailAsync(loginDTO.Email);
            if (user == null)
            {
                return Unauthorized(new { message = "Invalid credentials" });
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDTO.Password, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                return Ok(new { token = GenerateJwtToken(user) });
            }

            return Unauthorized(new { message = "Invalid credentials" });
        }



    }
}
