using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using YMM.Data.Entities.Identity;
using YMM.Infrastructure.Context;

namespace YMM.Api.Middleware
{
    public class CheckAccountStatusMiddleware
    {
        private readonly RequestDelegate _next;
        public CheckAccountStatusMiddleware( RequestDelegate next)
        {
            _next = next;
        }
        public async Task Invoke(HttpContext context,AppDb appDb)
        {
            if(context.User.Identity.IsAuthenticated==true)
            {
                var userClaim = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (userClaim != null)
                {
                    var IsBanned = await appDb.Users.Where(x => x.AccountStatus == "Banned" && x.Id == userClaim).FirstOrDefaultAsync();
                    if(IsBanned !=null)
                    {
                        context.Response.StatusCode = StatusCodes.Status403Forbidden;
                        await context.Response.WriteAsJsonAsync(new
                        {
                            message = "Your account has been banned."
                        });

                        return;
                    }
                    var IsSuppend= await appDb.Users.Where(x => x.AccountStatus == "Suspension" && x.Id == userClaim).FirstOrDefaultAsync();
                    if (IsSuppend != null)
                    {
                        context.Response.StatusCode = StatusCodes.Status403Forbidden;
                        await context.Response.WriteAsJsonAsync(new
                        {
                            message = "Your account has been Suspension."
                        });

                        return;
                    }
                }

            }
            await _next(context);
        }
    }
}
