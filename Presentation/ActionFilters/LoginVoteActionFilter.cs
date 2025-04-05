using DataAccess.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Presentation.Controllers;

namespace Presentation.ActionFilters
{
    public class LoginVoteActionFilter: ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            //This action filter prevents users from voting if they haven't logged in or if they already voted on a poll

            var user = context.HttpContext.User;

            if (user != null && user.Identity is { IsAuthenticated: false }) {

                //access tempdata from the controller of the context
                var tempData = ((Controller)context.Controller).TempData;
                tempData["error"] = "You must be logged in to Vote!";
                context.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Poll", action = "Index" }));
                return;
            }

            // Try to get the pollId from the action arguments
            if (context.ActionArguments.TryGetValue("pollId", out var pollIdObj) && pollIdObj is int pollId)
            {
                var pollRepo = (IPollRepository)context.HttpContext.RequestServices.GetService(typeof(IPollRepository));

                if (pollRepo != null && pollRepo.UserVotedOnPoll(user.Identity.Name, pollId))
                {
                    var tempData = ((Controller)context.Controller).TempData;
                    tempData["error"] = "You have already voted on this poll!";
                    context.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Poll", action = "Index" }));
                    return;
                }
            }

            base.OnActionExecuting(context);
        }
    }
}
