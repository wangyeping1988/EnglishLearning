using System.Collections.Generic;

namespace JasonWang.Dal.EntityFrameworkDal.AuxiliaryModels
{
    /// <summary>
    /// 返回的通用结果集对象，含总数和当前页数据
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Pager<T>
    {
        /// <summary>
        /// 总条数
        /// </summary>
        public long AllCount { get; set; }

        /// <summary>
        /// 当前分页条件下的数据集合
        /// </summary>
        public List<T> List { get; set; }
    }
}
