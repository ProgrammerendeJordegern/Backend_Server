using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using ASPwebApp.Data;
using DataBase;
using DataBase.Data;
using DataBase.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using static BCrypt.Net.BCrypt;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace ASPwebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {


        private readonly MyDbContext _context;
        private readonly AppSettings _appSettings;
        private const int BcryptWorkFactor = 10;
        private readonly ILogger<AccountsController> _logger;

        public AccountsController(MyDbContext context, IOptions<AppSettings> appSettings, ILogger<AccountsController> logger)
        {
            _context = context;
            _logger = logger;
            _appSettings = appSettings.Value;
        }

        [HttpPost("register"), AllowAnonymous]
        public async Task<ActionResult<UserDto>> Register(UserDto regUser)
        {
            _logger.LogInformation("Register called with email"+ regUser.Email);
            _context.Database.Migrate();
            _logger.LogInformation("Database.Migrate() ran ");
            regUser.Email = regUser.Email.ToLower();
            var emailExists = await _context.User.Where(u =>
                u.Email == regUser.Email).FirstOrDefaultAsync();
            if (emailExists != null)
                return BadRequest(new { errorMessage = "Email already in use" });
            UserDb user = new UserDb
            {
                FullName = regUser.FullName,
                Email = regUser.Email,
                PpUser = new PpUser(),
                CreationDate = DateTime.Today
            };
            user.PwHash = HashPassword(regUser.Password, BcryptWorkFactor);
            _logger.LogInformation("Adding new user to DB");
            await _context.User.AddAsync(user);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Just added new user " + regUser.Email);

            return CreatedAtAction("Get", new { id = user.UserId }, regUser);

        }

        //Get api/account/3
        [HttpGet("{id}"), ActionName("Get"), Authorize]
        public async Task<ActionResult<UserDto>> Get(int id)
        {
            var user = await _context.User.FindAsync((long)id);
            if (user == null)
                return NotFound();
            var userDto = new UserDto();
            userDto.Email = user.Email;
            userDto.FullName = user.FullName;
            return userDto;
        }

        [HttpPost("login"), AllowAnonymous]
        public async Task<ActionResult<UserDto>> Login(UserDto login)
        {
            _logger.LogInformation("Login from "+ login.Email);
            login.Email = login.Email.ToLower();
            var user = await _context.User.Where(u =>
                u.Email == login.Email).FirstOrDefaultAsync();
            if (user != null)
            {
                var validPwd = Verify(login.Password, user.PwHash);
                if (validPwd)
                {
                    login.Password = null;
                    var token = new TokenDto(); 
                    token.JWT = GenerateToken(user);
                    login.AccessJWTToken = token.JWT;
                    user.AccessJWTToken = token.JWT;
                    await _context.SaveChangesAsync();
                    login.FullName = user.FullName;
                    return login;
                }
            }
            _logger.LogInformation("user not found: " + login.Email);
            ModelState.AddModelError(string.Empty, "Wrong username or password");
            return BadRequest(ModelState);
        }


        //Logout

        [HttpPost("logout"), Authorize]
        public async Task<OkObjectResult> Logout([FromHeader] string Authorization)
        {
            string[] authorizationJwt = Authorization.Split(" ");
            var JwtToken = authorizationJwt[1];
            var user = await _context.User.Where(u =>
                u.AccessJWTToken == JwtToken).FirstOrDefaultAsync();
            user.AccessJWTToken = null;
            await _context.SaveChangesAsync();
            return Ok("Logout completed");
        }


        private string GenerateToken(UserDb user)
        {
            var claims = new Claim[]
            {
                new Claim("Email", user.Email),
                new Claim(JwtRegisteredClaimNames.GivenName, user.FullName),     
                new Claim("UserId", user.UserId.ToString()),
                new Claim(JwtRegisteredClaimNames.Exp, 
                    new DateTimeOffset(DateTime.Now.AddDays(1)).ToUnixTimeSeconds().ToString()),
            }; 
            var key = Encoding.ASCII.GetBytes(_appSettings.SecretKey); 
            var token = new JwtSecurityToken(
                new JwtHeader(
                    new SigningCredentials(
                        new SymmetricSecurityKey(key), 
                        SecurityAlgorithms.HmacSha256)), 
                new JwtPayload(claims)); 
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }

}
