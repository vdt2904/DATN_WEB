using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

namespace DATNWEB.Models.Authentication
{
    public class AuthForAccess : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.HttpContext.Session.GetString("UserName") != null)
            {
                QlPhimAnimeContext db = new QlPhimAnimeContext();
                var a = db.Admins.FirstOrDefault(x=>x.UserName == context.HttpContext.Session.GetString("UserName"));
                if (a.Auth == 1)
                {
                    context.Result = new RedirectToRouteResult(
                    new RouteValueDictionary
                    {
                        {"Controller","Admin" },
                        {"Action","Index" }
                    }
                    );
                }
                
            }
        }
    }
}
