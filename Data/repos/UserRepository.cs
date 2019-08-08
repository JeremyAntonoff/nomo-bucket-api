using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NomoBucket.API.Dtos;
using NomoBucket.API.Models;

namespace NomoBucket.API.Data
{
  public class UserRepository : IUserRepository
  {
    private readonly DataContext _context;
    public UserRepository(DataContext context)
    {
      this._context = context;
    }
    public async Task<User> getUser(int userId)
    {
      var foundUser = await _context.Users.Include(u => u.BucketList).FirstOrDefaultAsync(u => u.Id == userId);
      return foundUser;
    }

    public async Task<IEnumerable<User>> getUsers()
    {
      var users = await _context.Users.ToListAsync();
      return users;
    }

    public async Task<bool> saveAll()
    {
      var changes = await _context.SaveChangesAsync();
      return changes > 0;
    }
  }
}