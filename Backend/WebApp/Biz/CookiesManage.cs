using System;
using System.Web;

namespace EnglishLearning.WebApp.Biz
{
    public class CookiesManage
    {
        private const string Path = "/";

        #region Cookie基本操作

        public static void Clear(string cookiesNames)
        {
            if (HttpContext.Current.Request.Cookies[cookiesNames] != null)
            {
                var myCookie = new HttpCookie(cookiesNames) { Expires = DateTime.Now.AddDays(-1d) };
                HttpContext.Current.Response.Cookies.Add(myCookie);
            }
        }

        /// <summary>
        /// 设置Cookie值
        /// </summary>
        /// <param name="ckName">Cookie名称</param>
        /// <param name="ckValue">Cookie值</param>
        public static void SetCookie(string ckName, string ckValue)
        {
            SetCookie(ckName, ckValue, 1);
        }

        /// <summary>
        /// 设置Cookie
        /// </summary>
        /// <param name="ckName"></param>
        /// <param name="ckValue"></param>
        /// <param name="ckday"></param>
        /// <param name="bhash"></param>
        public static void SetCookie(string ckName, string ckValue, int ckday, bool bhash = false)
        {
            var myCookie = new HttpCookie(ckName, HttpContext.Current.Server.UrlEncode(ckValue))
            {
                Path = "/",
                Expires = DateTime.Now.AddDays(ckday),
                HttpOnly = true
            };
            HttpContext.Current.Response.Cookies.Add(myCookie);
            //写入校验码，来源不需要校验
            if (!bhash) return;
            var myCookieHash = new HttpCookie(ckName + "_hash", StringExtension.TripleDesCrypto(ckValue))
            {
                Domain = Path,
                Expires = DateTime.Now.AddDays(ckday)
            };

            HttpContext.Current.Response.Cookies.Add(myCookieHash);
        }

        /// <summary>
        /// 根据Cookie名称得到相应的值
        /// </summary>
        /// <param name="ckName">Cookie名称</param>
        /// <param name="bhash"></param>
        /// <returns>返回相应的值</returns>
        public static string GetCookie(string ckName, bool bhash = false)
        {
            return GetCookie(ckName, string.Empty, bhash);
        }

        private static string GetCookie(string ckName, string defValue, bool bhash = false)
        {
            var ckValue = defValue;
            var ckValueHash = StringExtension.TripleDesCrypto(defValue);
            if (ckName.Length > 0)
            {
                var ht = HttpContext.Current.Request.Cookies[ckName];
                if (ht != null)
                {
                    ht.Path = "/";
                    ckValue = ht.Value;
                }

                ckValue = HttpContext.Current.Server.UrlDecode(ckValue);
                //读取校验码，来源不需要校验
                if (bhash)
                {
                    var htHash = HttpContext.Current.Request.Cookies[ckName + "_hash"];

                    if (htHash != null)
                    {
                        htHash.Domain = Path;
                        ckValueHash = htHash.Value;
                    }

                    if (StringExtension.TripleDesCryptoDe(ckValueHash).TrimEnd() != ckValue && ckName != "laiyuan")
                        ckValue = string.Empty;
                }
            }
            return ckValue;
        }
        #endregion

    }
}
