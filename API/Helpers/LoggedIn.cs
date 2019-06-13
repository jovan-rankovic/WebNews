using Application;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace API.Helpers
{
    public class LoggedIn : Attribute, IResourceFilter
    {
        private readonly string _role;

        public LoggedIn() { }

        public LoggedIn(string role)
            => _role = role;

        public void OnResourceExecuted(ResourceExecutedContext context) { }

        public void OnResourceExecuting(ResourceExecutingContext context)
        {
            var user = context.HttpContext.RequestServices.GetService<LoggedUser>();

            if (!user.IsLogged)
                context.Result = new UnauthorizedResult();
            else
            {
                if (_role != null)
                    if (user.Role != _role)
                        context.Result = new UnauthorizedResult();
            }
        }
    }
}