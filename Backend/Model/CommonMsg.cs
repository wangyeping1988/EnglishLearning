using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class CommonMsg
    {
        public static readonly string Error_EmptyLoginInfo = "账号或密码为空。";
        public static readonly string Error_LoginFail = "账号或密码错误。";
        public static readonly string Error_AuthFail = "用户认证过程出错。";

        public static readonly string Info_LoginSuccess = "登陆成功。";
        public static readonly string Info_LogoutSuccess = "登出成功。";
        public static readonly string Info_HasLoggedIn = "该用户已登陆。";
        public static readonly string Info_NotLoggedIn = "该用户未登录。";
    }
}
