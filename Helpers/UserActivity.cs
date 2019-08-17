using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;
using NomoBucket.API.Data;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace NomoBucket.API.Helpers
{
  public class UserActivity : IAsyncActionFilter
  {
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
    var result = await next();
     var userId = int.Parse(result.HttpContext.User.FindFirst("userId").Value);
     var repo = result.HttpContext.RequestServices.GetService<IUserRepository>();
     var foundUser = await repo.GetUser(userId);
     if (foundUser != null) {
         foundUser.LastActive = DateTime.Now;
     }
     await repo.SaveAll();
    }
  }
}