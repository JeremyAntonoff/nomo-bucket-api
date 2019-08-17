using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using NomoBucket.API.Data;
using NomoBucket.API.Dtos;
using NomoBucket.API.Models;

namespace NomoBucket.API.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class AuthController : ControllerBase
  {
    private readonly IAuthRepository _repo;
    private readonly IConfiguration _config;
    private readonly IMapper _mapper;

    public AuthController(IAuthRepository repo, IMapper mapper, IConfiguration config)
    {
      _mapper = mapper;
      _repo = repo;
      _config = config;
    }
    [HttpPost("register")]
    public async Task<IActionResult> Register(UserRegistrationDto userRegistrationDto)
    {
      userRegistrationDto.Username = userRegistrationDto.Username.ToLower();

      if (await _repo.CheckExistingUser(userRegistrationDto.Username))
      {
        return BadRequest("User already exists");
      }
      var userToCreate = _mapper.Map<User>(userRegistrationDto);

      var createdUser = await _repo.Register(userToCreate, userRegistrationDto.Password);
      if (createdUser == null)
      {
        return BadRequest("Could not create user");
      }
      var userToReturn = _mapper.Map<UserDetailsDto>(createdUser);
      return CreatedAtRoute("GetUser",new {controller="users", id = createdUser.Id}, userToReturn);
    }
    [HttpPost("login")]
    public async Task<IActionResult> Login(UserLoginDto userLoginDto)
    {
      userLoginDto.Username = userLoginDto.Username.ToLower();
      var user = await _repo.Login(userLoginDto.Username, userLoginDto.Password);
      if (user == null)
      {
        return Unauthorized();
      }
      var claims = new[]
      {
          new Claim("userId", user.Id.ToString()),
          new Claim(ClaimTypes.Name, user.Username)
      };
      var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetSection("Authorization:TokenSecret").Value));
      var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
      var tokenDescriptor = new SecurityTokenDescriptor
      {
        Subject = new ClaimsIdentity(claims),
        Expires = DateTime.Now.AddDays(1),
        SigningCredentials = credentials

      };
      var tokenHandler = new JwtSecurityTokenHandler();
      var token = tokenHandler.CreateToken(tokenDescriptor);
      return Ok(new
      {
        token = tokenHandler.WriteToken(token)
      });

    }
  }
}