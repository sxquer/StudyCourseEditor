using System;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace StudyCourseEditor.Extensions
{
    /// <summary>
    /// Allows to use miltiple submits in form and divide actions with single attr [HttpParamAction]
    /// http://blog.ashmind.com/2010/03/15/multiple-submit-buttons-with-asp-net-mvc-final-solution/
    /// </summary>
    public class HttpParamActionAttribute : ActionNameSelectorAttribute
    {
        public override bool IsValidName(ControllerContext controllerContext,
                                         string actionName,
                                         MethodInfo methodInfo)
        {
            if (actionName.Equals(methodInfo.Name,StringComparison.InvariantCultureIgnoreCase))
                return true;

            if (!actionName.Equals("Action", StringComparison.InvariantCultureIgnoreCase))
                return false;

            HttpRequestBase request =
                controllerContext.RequestContext.HttpContext.Request;
            return request[methodInfo.Name] != null;
        }
    }
}