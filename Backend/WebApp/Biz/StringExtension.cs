using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;

namespace EnglishLearning.WebApp.Biz
{
    public static class StringExtension
    {
        public static DateTime FormatWeekUpOfDate(this DateTime dt, DayOfWeek weekday, int number)
        {
            var wd1 = (int)weekday;
            var wd2 = (int)dt.DayOfWeek;
            return wd2 == wd1 ? dt.AddDays(7 * number) : dt.AddDays(7 * number - wd2 + wd1);
        }
        public static string ShowNullOrEmpty(this string text, string defaultText = "-")
        {
            if (string.IsNullOrEmpty(text))
                return defaultText;

            return text;
        }

        /// <summary>
        /// <para>为空的字符串转成替代符合 </para>
        /// </summary>
        /// <param name="str"></param>
        /// <param name="mark"></param>
        /// <returns></returns>
        public static string Format(this String str, string mark = "-")
        {
            if (!string.IsNullOrEmpty(str))
            {
                if (!string.IsNullOrEmpty(str.Trim()))
                {
                    return str.Trim().Replace("\r\n", "").Replace("\r", "").Replace("\n", "");
                }
            }
            return mark;
        }

        public static string Format(this string str, int length)
        {
            if (str.Length > length)
            {
                return str.Substring(0, length) + "...";
            }
            else
            {
                return str;
            }
        }

        /// <summary>
        /// <para>防止sql注入 </para>
        /// </summary>
        /// <param name="strValue"></param>
        /// <returns></returns>
        public static string FormatSql(this string strValue)
        {
            if (string.IsNullOrEmpty(strValue))
                return "";
            string sqlStr = "declare |exec |varchar |cursor |begin |open |drop |creat |select |truncate ";
            string[] sqlStrs = sqlStr.Split('|');
            string strValueLower = strValue.ToLower();
            foreach (string ss in sqlStrs)
            {
                if (strValueLower.ToLower().IndexOf(ss) >= 0)
                {
                    strValue.Remove(strValue.ToLower().IndexOf(ss), ss.Length);
                }
            }
            strValue.Replace("'", "''");
            return strValue;
        }
        public static string ListToString(this IEnumerable<string> list, char separate = ';', string defalutString = "-")
        {
            if (list == null || list.Count() <= 0)
                return defalutString;
            var result = new StringBuilder();
            foreach (var s in list)
            {
                result.AppendFormat("{0}{1}", s, separate);
            }
            var str = result.ToString();
            str = str.TrimEnd(separate);
            return str;
        }
        public static string ListToString(this List<string> list, string returnMark = "-", char fg = ';')
        {
            if (list == null || list.Count <= 0)
                return returnMark;
            var result = list.Aggregate("", (current, s) => current + (s + fg));
            result = result.TrimEnd(fg);//.Substring(0,result.LastIndexOf(';'));
            return result;
        }
        public static string ListFormat(this IEnumerable<string> list, string returnMark = "-", char fg = ';')
        {
            var enumerable = list as string[] ?? list.ToArray();
            if (list == null || !enumerable.Any())
                return returnMark;
            var result = enumerable.Aggregate("", (current, s) => current + (s + fg));
            result = result.TrimEnd(fg);//.Substring(0,result.LastIndexOf(';'));
            return result;
        }
        /// <summary>
        /// 使用TripleDES加密
        /// </summary>
        /// <param name="str">需要加密的字符串</param>
        /// <param name="key">16或24位密钥</param>
        /// <returns></returns>
        public static string TripleDesCrypto(string str, string key = @"]#gfV'8P""3]@xR:Z")
        {
            if (String.IsNullOrEmpty(str) || String.IsNullOrEmpty(key))
                return string.Empty;

            var data = Encoding.Unicode.GetBytes(str);
            var keys = Encoding.ASCII.GetBytes(key);

            var des = new TripleDESCryptoServiceProvider { Key = keys, Mode = CipherMode.ECB };
            ICryptoTransform cryp = des.CreateEncryptor();//加密

            return Convert.ToBase64String(cryp.TransformFinalBlock(data, 0, data.Length));
        }

        /// <summary>
        /// 使用TripleDES解密
        /// </summary>
        /// <param name="str">需要解密的字符串</param>
        /// <param name="key">16或24位密钥</param>
        /// <returns></returns>
        public static string TripleDesCryptoDe(string str, string key = @"]#gfV'8P""3]@xR:Z")
        {
            if (String.IsNullOrEmpty(str) || String.IsNullOrEmpty(key))
                return string.Empty;

            var data = Convert.FromBase64String(str);
            var keys = Encoding.ASCII.GetBytes(key);

            var des = new TripleDESCryptoServiceProvider
            {
                Key = keys,
                Mode = CipherMode.ECB,
                Padding = PaddingMode.PKCS7
            };
            var cryp = des.CreateDecryptor();//解密

            return Encoding.Unicode.GetString(cryp.TransformFinalBlock(data, 0, data.Length));
        }
    }

    /// <summary>
    ///  功    能： 字符串操作类
    /// </summary>
    public class StrOpers
    {
        #region "MD5加密"
        /// <summary>
        /// MD5加密
        /// </summary>
        /// <param name="str">加密字符</param>
        /// <param name="code">加密位数16/32</param>
        /// <returns></returns>
        public static string MD5(string str, int code)
        {
            string strEncrypt = string.Empty;
            if (code == 16)
            {
                strEncrypt = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(str, "MD5").Substring(8, 16);
            }

            if (code == 32)
            {
                strEncrypt = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(str, "MD5");
            }
            return strEncrypt;
        }
        #endregion
        # region 简单的加解密方法
        //字符串加密   
        public static string encode(string strText)
        {
            Byte[] Iv64 = { 11, 22, 33, 44, 55, 66, 77, 88 };
            Byte[] byKey64 = { 10, 20, 30, 40, 50, 60, 70, 80 };
            try
            {
                DESCryptoServiceProvider des = new DESCryptoServiceProvider();
                Byte[] inputByteArray = Encoding.UTF8.GetBytes(strText);
                MemoryStream ms = new MemoryStream();
                CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(byKey64, Iv64), CryptoStreamMode.Write);
                cs.Write(inputByteArray, 0, inputByteArray.Length);
                cs.FlushFinalBlock();
                return Convert.ToBase64String(ms.ToArray());
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        //字符串解密   
        public static string decode(string strText)
        {
            Byte[] Iv64 = { 11, 22, 33, 44, 55, 66, 77, 88 };
            Byte[] byKey64 = { 10, 20, 30, 40, 50, 60, 70, 80 };
            Byte[] inputByteArray = new byte[strText.Length];
            try
            {
                DESCryptoServiceProvider des = new DESCryptoServiceProvider();
                inputByteArray = Convert.FromBase64String(strText);
                MemoryStream ms = new MemoryStream();
                CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(byKey64, Iv64), CryptoStreamMode.Write);
                cs.Write(inputByteArray, 0, inputByteArray.Length);
                cs.FlushFinalBlock();
                System.Text.Encoding encoding = System.Text.Encoding.UTF8;
                return encoding.GetString(ms.ToArray());
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        #endregion
        #region 生成指定长度的字符串,即生成strLong个str字符串
        /// <summary>
        /// 生成指定长度的字符串,即生成strLong个str字符串
        /// </summary>
        /// <param name="strLong">生成的长度</param>
        /// <param name="str">以str生成字符串</param>
        /// <returns></returns>
        public static string StringOfChar(int strLong, string str)
        {
            string ReturnStr = "";
            for (int i = 0; i < strLong; i++)
            {
                ReturnStr += str;
            }
            return ReturnStr;
        }
        #endregion
        #region 字符串截取
        /// <summary>
        /// 左截取
        /// </summary>
        /// <param name="inputString"></param>
        /// <param name="len"></param>
        /// <returns></returns>
        public static string Left(string inputString, int len)
        {
            if (inputString.Length < len)
                return inputString;
            else
                return inputString.Substring(0, len) + "....";
        }
        /// <summary>
        /// 右截取
        /// </summary>
        /// <param name="inputString"></param>
        /// <param name="len"></param>
        /// <returns></returns>
        public static string Right(string inputString, int len)
        {
            if (inputString.Length < len)
                return inputString;
            else
                return inputString.Substring(inputString.Length - len, len);
        }
        /// <summary>
        /// 截取指定长度字符串
        /// </summary>
        /// <param name="inputString"></param>
        /// <param name="len"></param>
        /// <returns></returns>
        public static string CutTitle(string sText, int iLength, bool bl)
        {
            if (iLength < 1) return sText;
            byte[] b = System.Text.Encoding.Default.GetBytes(sText);
            double n = 0.0;
            int m = 0;
            bool l0 = false, l1 = false, l2 = false;
            for (int i = 0; i < b.Length; i++)
            {
                l0 = ((int)b[i] > 128);
                if (l0) i++;
                n += (l0 ? 1.0 : 0.5);
                if (n > iLength)
                {
                    string strOut = (l2 ? sText.Substring(0, m - 1) : sText.Substring(0, m - 2));
                    if (System.Text.Encoding.GetEncoding("GB2312").GetByteCount(strOut) + 2 > iLength * 2)
                        strOut = strOut.Substring(0, strOut.Length - 1);
                    if (bl)
                        strOut += "..";
                    return strOut;
                }
                m++;
                l2 = l1;
                l1 = l0;
            }
            return sText;
        }
        #endregion
        #region Html字符处理
        /// <summary>
        /// 除去所有的Html标记
        /// </summary>
        /// <param name="strHtml"></param>
        /// <returns></returns>
        public static string StripHTML(string strHtml)
        {
            string strOutput = strHtml;
            System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex(@"<[^>]+>|</[^>]+>");
            strOutput = regex.Replace(strOutput, "");
            return strOutput;
        }
        public static string StripHTML2(string strHtml)
        {
            string strOutput = strHtml;
            System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex(@"<[^>]+>|</[^>]+>");
            strOutput = regex.Replace(strOutput, "");
            strOutput = System.Text.RegularExpressions.Regex.Replace(strOutput, "\\s*|\t|\r|\n", "");
            return strOutput;
        }

        /// <summary>
        /// 替换html中的特殊字符
        /// </summary>
        /// <param name="theString">需要进行替换的文本。</param>
        /// <returns>替换完的文本。</returns>
        public static string HtmlEncode(string theString)
        {
            theString = theString.Replace(">", "&gt;");
            theString = theString.Replace("<", "&lt;");
            theString = theString.Replace("  ", " &nbsp;");
            theString = theString.Replace(" ", "&nbsp;");
            theString = theString.Replace("\"", "&quot;");
            theString = theString.Replace("\'", "&#39;");
            theString = theString.Replace("\n", "<br/> ");
            return theString;
        }

        /// <summary>
        /// 恢复html中的特殊字符
        /// </summary>
        /// <param name="theString">需要恢复的文本。</param>
        /// <returns>恢复好的文本。</returns>
        public static string HtmlDecode(string theString)
        {
            theString = theString.Replace("&gt;", ">");
            theString = theString.Replace("&lt;", "<");
            theString = theString.Replace("&nbsp;", " ");
            theString = theString.Replace(" &nbsp;", "  ");
            theString = theString.Replace("&quot;", "\"");
            theString = theString.Replace("&#39;", "\'");
            theString = theString.Replace("<br/> ", "\n");
            return theString;
        }
        #endregion
        #region 统计字符串sin在str中出现的次数
        /// <summary>
        /// 统计字符串sin在str中出现的次数
        /// </summary>
        /// <param name="str">长字符</param>
        /// <param name="sin">要统计的字符</param>
        /// <returns></returns>
        public static int GetSubStringCount(string str, string sin)
        {
            int i = 0;
            int ibit = 0;
            while (true)
            {
                ibit = str.IndexOf(sin, ibit);
                if (ibit > 0)
                {
                    ibit += sin.Length;
                    i++;
                }
                else
                {
                    break;
                }
            }
            return i;
        }
        #endregion
        #region "获取用户IP地址"
        /// <summary>
        /// 获取用户IP地址
        /// </summary>
        /// <returns></returns>
        public static string GetIPAddress()
        {

            string user_IP = string.Empty;
            if (System.Web.HttpContext.Current.Request.ServerVariables["HTTP_VIA"] != null)
            {
                if (System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] != null)
                {
                    user_IP = System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"].ToString();
                }
                else
                {
                    user_IP = System.Web.HttpContext.Current.Request.UserHostAddress;
                }
            }
            else
            {
                user_IP = System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"].ToString();
            }
            return user_IP;
        }
        #endregion
        #region 分割字符串，存入数组里
        /// <summary>
        /// 分割字符串，存入数组里
        /// </summary>
        /// <param name="str">字符串名称</param>
        /// <returns></returns>
        public static string[] GetArrayStr(string str)
        {
            str = str.Replace("，", ",");
            str = str.Replace("|", ",");
            str = str.Replace("；", ",");
            str = str.Replace(";", ",");
            str = str.Replace(";", ",");
            str = str.Replace("、", ",");
            str = str.Replace("/", ",");
            string[] Mstr = str.Split(',');
            return Mstr;
        }
        #endregion
        # region 返回用户级别名称
        /// <summary>
        /// 返回用户级别名称
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string admin_Name(string name)
        {
            string Rname = "";
            if (name == "")
                Rname = "无级别";
            if (name.Trim() == "S")
                Rname = "管理员";
            if (name.Trim() == "U")
                Rname = "普通用户";
            if (name.Trim() == "A")
                Rname = "超级管理员";
            return Rname;
        }
        # endregion
        /// <summary>
        /// 过滤特殊字符
        /// </summary>
        /// <param name="Str"></param>
        /// <returns></returns>
        public static string FilteSQLStr(string Str)
        {
            Str = Str.Replace("'", "");
            Str = Str.Replace("\"", "");
            Str = Str.Replace("&", "&amp");
            Str = Str.Replace("<", "&lt");
            Str = Str.Replace(">", "&gt");
            Str = Str.Replace(";", "；");
            Str = Str.Replace("(", "（");
            Str = Str.Replace(")", "）");
            return Str;
        }
        /// <summary>
        /// 添加meta中keywords，和description
        /// </summary>
        /// <param name="MetaType"></param>
        /// <param name="page"></param>
        /// <param name="MetaContent"></param>
        public static void GetMetaType(int MetaType, System.Web.UI.Page page, string MetaContent)
        {

            System.Web.UI.HtmlControls.HtmlMeta meta = new System.Web.UI.HtmlControls.HtmlMeta();
            switch (MetaType)
            {
                case 1:
                    meta.Name = "keywords";
                    break;
                case 2:
                    meta.Name = "description";
                    break;
                case 3:
                    page.Header.Controls.Add(new System.Web.UI.LiteralControl("\n\r"));
                    break;
            }
            if (MetaType == 3) return;
            meta.Content = MetaContent;
            page.Header.Controls.Add(meta);
            page.Header.Controls.Add(new System.Web.UI.LiteralControl("\n\r"));
        }
        public static void get404()
        {
            string filepath = System.Web.HttpContext.Current.Server.MapPath("\\") + "404.html";
            string tempstr;
            using (StreamReader sr = new StreamReader(filepath, Encoding.Default))
            {
                tempstr = sr.ReadToEnd();
            }
            System.Web.HttpContext.Current.Response.Write(tempstr);
            System.Web.HttpContext.Current.Response.StatusCode = 404;
            System.Web.HttpContext.Current.Response.End();
        }
        public static void get404(string url)
        {
            HttpContext context = HttpContext.Current;
            Control control = new Control();
            context.Response.StatusCode = 404;
            context.Response.Status = "404 Moved Permanently";
            context.Response.Redirect(url, true);
        }
        public static void get301(string url)
        {
            HttpContext context = HttpContext.Current;
            Control control = new Control();
            context.Response.Status = "301 Moved Permanently";
            context.Response.AddHeader("Location", control.ResolveUrl(url));
            context.Response.End();
        }
        /// <summary>
        /// 生成随机字母与数字
        /// </summary>
        /// <param name="Length">生成长度</param>
        /// <returns></returns>
        public static string StrRandom(int Length)
        {
            var pattern = new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };//,'a','b','d','e','f','g','h','m','n' 
            string result = "";
            int n = pattern.Length;
            var random = new Random(~unchecked((int)(DateTime.Now.Ticks + Guid.NewGuid().GetHashCode())));
            for (int i = 0; i < Length; i++)
            {
                int rnd = random.Next(0, n);
                result += pattern[rnd];
            }
            return result;
        }
        #region 全角半角转换
        /// <summary>
        /// 转全角的函数(SBC case)
        /// </summary>
        /// <param name="input">任意字符串</param>
        /// <returns>全角字符串</returns>
        ///<remarks>
        ///全角空格为12288，半角空格为32
        ///其他字符半角(33-126)与全角(65281-65374)的对应关系是：均相差65248
        ///</remarks>
        public static string ToSbc(string input)
        {
            //半角转全角：
            char[] c = input.ToCharArray();
            for (int i = 0; i < c.Length; i++)
            {
                if (c[i] == 32)
                {
                    c[i] = (char)12288;
                    continue;
                }
                if (c[i] < 127)
                    c[i] = (char)(c[i] + 65248);
            }
            return new string(c);
        }


        /// <summary> 转半角的函数(DBC case) </summary>
        /// <param name="input">任意字符串</param>
        /// <returns>半角字符串</returns>
        ///<remarks>
        ///全角空格为12288，半角空格为32
        ///其他字符半角(33-126)与全角(65281-65374)的对应关系是：均相差65248
        ///</remarks>
        public static string ToDbc(string input)
        {
            char[] c = input.ToCharArray();
            for (int i = 0; i < c.Length; i++)
            {
                if (c[i] == 12288)
                {
                    c[i] = (char)32;
                    continue;
                }
                if (c[i] > 65280 && c[i] < 65375)
                    c[i] = (char)(c[i] - 65248);
            }
            return new string(c);
        }
        #endregion
        public static string GetRootURI()
        {
            string AppPath = "";
            HttpContext HttpCurrent = HttpContext.Current;
            HttpRequest Req;
            if (HttpCurrent != null)
            {
                Req = HttpCurrent.Request;

                string UrlAuthority = Req.Url.GetLeftPart(UriPartial.Authority);
                if (Req.ApplicationPath == null || Req.ApplicationPath == "/")
                    //直接安装在   Web   站点   
                    AppPath = UrlAuthority;
                else
                    //安装在虚拟子目录下   
                    AppPath = UrlAuthority + Req.ApplicationPath;
            }
            return AppPath;
        }
        /// <summary>
        /// 取得网站的根目录的URL
        /// </summary>
        /// <param name="Req"></param>
        /// <returns></returns>
        public static string GetRootURI(HttpRequest Req)
        {
            string AppPath = "";
            if (Req != null)
            {
                string UrlAuthority = Req.Url.GetLeftPart(UriPartial.Authority);
                if (Req.ApplicationPath == null || Req.ApplicationPath == "/")
                    //直接安装在   Web   站点   
                    AppPath = UrlAuthority;
                else
                    //安装在虚拟子目录下   
                    AppPath = UrlAuthority + Req.ApplicationPath;
            }
            return AppPath;
        }
        /// <summary>
        /// 取得网站根目录的物理路径
        /// </summary>
        /// <returns></returns>
        public static string GetRootPath()
        {
            string AppPath = "";
            HttpContext HttpCurrent = HttpContext.Current;
            if (HttpCurrent != null)
            {
                AppPath = HttpCurrent.Server.MapPath("~");
            }
            else
            {
                AppPath = AppDomain.CurrentDomain.BaseDirectory;

            }
            if (Regex.Match(AppPath, @"\\$", RegexOptions.Compiled).Success)
                AppPath = AppPath.Substring(0, AppPath.Length - 1);
            return AppPath;
        }
    }
}
