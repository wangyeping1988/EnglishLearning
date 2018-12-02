using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Security;
using Model;
using EnglishLearning.WebApp.Biz;

namespace EnglishLearning.WebApp.Controllers.Api
{
    /// <summary>
    /// 账户相关接口
    /// </summary>
    public class AccountController : ApiController
    {
        /// <summary>
        /// 登陆验证并设置cookie，员工Id和密码必传
        /// </summary>
        /// <param name="user">用户信息</param>
        /// <returns></returns>
        [HttpPost]
        public ResponseResult<bool> Login(User user)
        //public ResponseResult<string> Login(UserLoginModel user, string returnUrl)
        {
            var result = ResponseResult<bool>.MakeFailResult();

            if (string.IsNullOrEmpty(user.LoginName) || string.IsNullOrEmpty(user.Password))
            {
                result.Message = CommonMsg.Error_EmptyLoginInfo;
                result.Data = false;
                return result;
            }

            if (!user.LoginName.Equals("wangyeping") || !user.Password.Equals("123456"))
            {
                result.Message = CommonMsg.Error_LoginFail;
                result.Data = false;
                return result;
            }
            else
            {
                result.Message = CommonMsg.Info_LoginSuccess;
                result.Data = true;
                result.Success();
            }

            // 登陆成功后设置IsAdmin值，Password置空，再放到cookie中
            //user.IsAdmin = resultFromWcf.Data.IsAdmin;
            user.Password = "";

            FormsAuthentication.SetAuthCookie(user.LoginName, true);
            var cookiesName = string.Format("{0}-{1}", "EnglishLearning.WebApp.Controllers.Api", user.LoginName);
            CookiesManage.SetCookie(cookiesName, user.ToJson(), 10);

            //if (!string.IsNullOrEmpty(returnUrl))
            //{
            //    //return Redirect(returnUrl);
            //    return result;
            //}

            //return RedirectToAction("Index", "Home", new { area = "" });
            return result;
        }

        /// <summary>
        /// 验证是否已登陆
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ResponseResult<bool> HasLoggedIn()
        {
            var result = ResponseResult<bool>.MakeFailResult();

            if (System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
            {
                result.Success();
                result.Data = true;
                result.Message = CommonMsg.Info_HasLoggedIn;
            }
            else
            {
                result.Success();
                result.Data = false;
                result.Message = CommonMsg.Info_NotLoggedIn;
            }

            //return RedirectToAction("Index", "Home", new { area = "" });
            return result;
        }

        /// <summary>
        /// 退出登陆
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ResponseResult<string> LogOut()
        {
            var result = ResponseResult<string>.MakeFailResult();

            FormsAuthentication.SignOut();
            result.Success();
            result.Message = CommonMsg.Info_LogoutSuccess;

            //return RedirectToAction("Login");
            return result;
        }
    }
}
