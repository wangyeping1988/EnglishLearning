using System.Web.Script.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace EnglishLearning.WebApp.Biz
{
    public static class JsonExtension
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string ToJson(this object obj)
        {
            if (obj == null)
                return string.Empty;
            IsoDateTimeConverter timeConverter = new IsoDateTimeConverter();
            timeConverter.DateTimeFormat = "yyyy-MM-dd HH:mm:ss";//设置时间格式 
            return JsonConvert.SerializeObject(obj, Formatting.Indented, timeConverter);
        }
        public static T JsonToObject<T>(this string str)
        {
            if (string.IsNullOrEmpty(str))
                return default(T);
            return JsonConvert.DeserializeObject<T>(str);
        }
        public static string ToJsonMs(this object obj)
        {

            if (obj == null)
                return string.Empty;
            var jsonSerializer = new JavaScriptSerializer();
            return jsonSerializer.Serialize(obj);
        }
    }
}
