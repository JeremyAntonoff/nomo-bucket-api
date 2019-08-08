using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NomoBucket.API.Data;
using NomoBucket.API.Dtos;

namespace NomoBucket.API.Controllers
{
  [Authorize]
  [Route("/api/{controller}")]
  [ApiController]

  public class UsersController : ControllerBase
  {
    private readonly IUserRepository _repo;
    private readonly IMapper _mapper;

    public UsersController(IUserRepository repo, IMapper mapper)
    {
      this._repo = repo;
      this._mapper = mapper;
    }
    [HttpGet]
    public async Task<IActionResult> getUsers()
    {
        var users = await _repo.getUsers();
        var usersToReturn = _mapper.Map<IEnumerable<UserListDto>>(users);
        return Ok(usersToReturn);
    }
    [HttpGet("{id}")]
      public async Task<IActionResult> getUser(int id)
    {
        var user = await _repo.getUser(id);
        if (user == null) 
        {
          return NotFound();
        }
        var userToReturn = _mapper.Map<UserDetailsDto>(user);

        return Ok(userToReturn);
    }
  }
}