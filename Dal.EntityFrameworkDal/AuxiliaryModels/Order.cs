namespace JasonWang.Dal.EntityFrameworkDal.AuxiliaryModels
{
    /// <summary>
    /// 用于Linq排序
    /// </summary>
    public class Order
    {
        /// <summary>
        /// 排序字段
        /// </summary>
        public string Column { get; set; }
        /// <summary>
        /// 排序方式
        /// </summary>
        public string Sort { get; set; }
    }
}
