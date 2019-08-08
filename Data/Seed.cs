using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using NomoBucket.API.Models;

namespace NomoBucket.API.Data
{
  public class Seed
  {
    private readonly DataContext _context;
    public Seed(DataContext context)
    {
      this._context = context;

    }
    public void SeedUsers()
    {
      var userData = File.ReadAllText("Data/SeedData.json");
      var users = JsonConvert.DeserializeObject<List<User>>(userData);

      foreach (var user in users)
      {
          byte[] passwordHash, passwordSalt;
          CreatePasswordHash("password", out passwordHash, out passwordSalt);
          user.PasswordHash = passwordHash;
          user.PasswordSalt = passwordSalt;
          user.Username = user.Username.ToLower();
          _context.Users.Add(user);
      }
      _context.SaveChanges();
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