using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using NomoBucket.API.Models;

namespace NomoBucket.API.Data
{
  public class AuthRepository : IAuthRepository
  {
    private readonly DataContext _context;

    public AuthRepository(DataContext context)
    {
      _context = context;
    }
    public async Task<bool> CheckExistingUser(string username)
    {
      var userFound = await _context.Users.AnyAsync(u => u.Username == username);
      return userFound;
    }

    public async Task<User> Login(string username, string password)
    {
      var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
      if (user == null)
      {
        return null;
      }
      if (!VerifyPassword(password, user.PasswordHash, user.PasswordSalt))
      {
        return null;
      }
      return user;

    }

    private bool VerifyPassword(string password, byte[] passwordHash, byte[] passwordSalt)
    {
      using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))
      {
        var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
        for (int i = 0; i < computedHash.Length; i++)
        {
          if (passwordHash[i] != computedHash[i])
          {
            return false;
          }
        }

      }
      return true;
    }

    public async Task<User> Register(User user, string password)
    {

      byte[] PasswordHash, PasswordSalt;
      CreatePasswordHash(password, out PasswordHash, out PasswordSalt);
      user.PasswordHash = PasswordHash;
      user.PasswordSalt = PasswordSalt;

      await _context.AddAsync(user);
      await _context.SaveChangesAsync();

      return user;

    }
    private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
    {
      using (var hmac = new System.Security.Cryptography.HMACSHA512())
      {
        passwordSalt = hmac.Key;
        passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));

      }
    }
  }
}

