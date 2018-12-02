using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using Model;
using JasonWang.Util.Log.Log4netLogger;

namespace EnglishLearning.WebApp.Biz
{
    /// <summary>
    /// 通用认证过滤器
    /// </summary>
    public class AuthorizeAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// 在调用操作方法之后发生
        /// </summary>
        /// <param name="actionContext">操作执行的上下文</param>
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            try
            {
                // 记录请求数据

                //string controllName = actionContext.ControllerContext.ControllerDescriptor.ControllerName;
                string actionName = actionContext.ActionDescriptor.ActionName;

                if (actionName != "Login" && actionName != "HasLoggedIn")
                {
                    if (!HttpContext.Current.User.Identity.IsAuthenticated)
                    {
                        actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Forbidden, new ResponseResult<string>() { RetCode = RetCode.Error, Message = CommonMsg.Info_NotLoggedIn });
                    }
                }
            }
            catch (Exception ex)
            {
                Log4netLoggerWrapper.Instance().LogError(ex, $"AuthorizeAttribute|OnActionExecuting--->出错");

                actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Forbidden, new ResponseResult<string>() { RetCode = RetCode.Error, Message = CommonMsg.Error_AuthFail });
                return;
            }

            base.OnActionExecuting(actionContext);
        }

        /// <summary>
        /// 在调用操作方法之后发生
        /// </summary>
        /// <param name="actionExecutedContext">操作执行的上下文</param>
        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            // 记录响应数据

            base.OnActionExecuted(actionExecutedContext);
        }
    }
}