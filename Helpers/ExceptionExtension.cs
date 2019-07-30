using System.Net;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace NomoBucket.API.Helpers
{
  public static class ExceptionExtension
  {
    public static void ConfigureExceptionHandler(this IApplicationBuilder app)
    {
      app.UseExceptionHandler(error =>
      {
        error.Run(async ctx =>
        {
          ctx.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
          ctx.Response.Headers.Add("Access-Control-Allow-Origin", "*");

          var feature = ctx.Features.Get<IExceptionHandlerFeature>();
          if (feature != null)
          {
            var response = JsonConvert.SerializeObject(new
            {
              StatusCode = ctx.Response.StatusCode,
              Message = feature.Error.Message

            });

            await ctx.Response.WriteAsync(response);
          }
        });
      });
    }
  }
}