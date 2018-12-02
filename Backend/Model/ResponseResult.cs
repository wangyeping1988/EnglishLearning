using System.Runtime.Serialization;

namespace Model
{
    [DataContract]
    public class ResponseResult<T>
    {
        /// <summary>
        /// 状态码
        /// </summary>
        [DataMember]
        public RetCode RetCode { get; set; }

        /// <summary>
        /// 调用信息
        /// </summary>
        [DataMember]
        public string Message { get; set; }

        /// <summary>
        /// 返回数据
        /// </summary>
        [DataMember]
        public T Data { get; set; }

        public static ResponseResult<T> MakeFailResult()
        {
            return new ResponseResult<T> { RetCode = RetCode.Error, Message = "内部错误，请查日志" };
        }

        public void Success()
        {
            RetCode = RetCode.Success;
            Message = "";
        }
    }
}
