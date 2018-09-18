using JasonWang.Dal.EntityFrameworkDal.AuxiliaryModels;
using JasonWang.Dal.EntityFrameworkDal.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace JasonWang.Dal.EntityFrameworkDal
{
    /// <summary>
    /// 作为其他BLL的特定Manager的基类，使用COMDal提供的数据库操作方法
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class COMManager<T> where T : class
    {
        protected static DefaultContext context;

        private COMDal<T> dal;

        public COMManager() : this(true) { }

        public COMManager(Boolean DataAccess)
        {
            if (DataAccess)
            {
                context = new DefaultContext(DBConn.DefaultConn);

                dal = new COMDal<T>(context);
            }
        }

        //默认排除修改字段ctime，isenabled
        protected List<string> defaultExcludeColumns = new string[] { "CreateTime" }.ToList();

        #region CURD操作
        /// <summary>
        /// 获取 All List T Aysnc
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public async Task<List<T>> GetAllAsync(Expression<Func<T, bool>> predicate)
        {
            var list = await dal.GetListTAsync(predicate);
            return list;
        }

        /// <summary>
        /// 获取 List T Aysnc
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public virtual async Task<List<T>> GetListAsync(Expression<Func<T, bool>> predicate)
        {
            var list = await dal.GetListTAsync(predicate);
            return list;
        }

        public virtual List<T> GetList(Expression<Func<T, bool>> predicate)
        {
            var list = dal.GetListT(predicate);
            return list;
        }

        /// <summary>
        /// 获取 List T Count Aysnc
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public virtual async Task<int> GetListCountAsync(Expression<Func<T, bool>> predicate)
        {
            return await dal.GetListTCountAsync(predicate);
        }

        /// <summary>
        /// 获取 List T by Pager Aysnc
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="order"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public virtual async Task<Pager<T>> GetListAsync(Expression<Func<T, bool>> predicate, Order order, int pageIndex, int pageSize)
        {
            return await dal.GetListTAsync(predicate, order, pageIndex, pageSize);
        }

        /// <summary>
        /// 更新 T Aysnc
        /// </summary>
        /// <param name="model">t</param>
        /// <param name="predicate">更新条件</param>
        /// <param name="excludeColumns">不更新字段</param>
        /// <returns></returns>
        public virtual async Task<bool> UpdateAsync(T model, Expression<Func<T, bool>> predicate, List<string> excludeColumns)
        {
            try
            {
                excludeColumns = excludeColumns ?? defaultExcludeColumns;
                var result = await dal.UpdateTAsync(model, predicate, excludeColumns);
                return result;
            }
            catch (Exception ex)
            {
                //ErrorLogManager.Instance.AddErrorLogAsync(new Model.ErrorLog()
                //{
                //    Method = "UpdateAsync Exception<br/>T：" + typeof(T),
                //    Message = "访问URL：<a href='javascript:void(0)'>" + System.Web.HttpContext.Current.Request.Url.OriginalString + "</a><br/>" + ex.Message + "<br />" + ex.StackTrace
                //});
                return false;
            }
        }

        public virtual bool Update(T model, Expression<Func<T, bool>> predicate, List<string> excludeColumns)
        {
            try
            {
                excludeColumns = excludeColumns ?? defaultExcludeColumns;
                var result = dal.UpdateT(model, predicate, excludeColumns);
                return result;
            }
            catch (Exception ex)
            {
                //ErrorLogManager.Instance.AddErrorLogAsync(new Model.ErrorLog()
                //{
                //    Method = "UpdateAsync Exception<br/>T：" + typeof(T),
                //    Message = "访问URL：<a href='javascript:void(0)'>" + System.Web.HttpContext.Current.Request.Url.OriginalString + "</a><br/>" + ex.Message + "<br />" + ex.StackTrace
                //});
                return false;
            }
        }

        /// <summary>
        /// 新增 T Aysnc
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public virtual async Task<bool> AddAsync(T t)
        {
            try
            {
                var result = await dal.AddTAsync(t);
                return result;
            }
            catch (Exception ex)
            {
                //ErrorLogManager.Instance.AddErrorLogAsync(new Model.ErrorLog()
                //{
                //    Method = "AddAsync Exception<br/>T：" + typeof(T),
                //    Message = "访问URL：<a href='javascript:void(0)'>" + System.Web.HttpContext.Current.Request.Url.OriginalString + "</a><br/>" + ex.Message + "<br />" + ex.StackTrace
                //});
                return false;
            }

        }

        /// <summary>
        /// 获取 T Aysnc
        /// </summary>
        /// <param name="predicate">获取条件</param>
        /// <returns></returns>
        public virtual async Task<T> GetModelAsync(Expression<Func<T, bool>> predicate)
        {
            return await dal.GetTAsync(predicate);
        }

        public virtual T GetModel(Expression<Func<T, bool>> predicate)
        {
            return dal.GetT(predicate);
        }

        /// <summary>
        /// 是否存在T Aysnc
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public virtual async Task<bool> isExistsAsync(Expression<Func<T, bool>> predicate)
        {
            return await dal.IsExistsAsync(predicate);
        }

        /// <summary>
        /// 删除T Aysnc
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public virtual async Task<bool> DeleteAsync(Expression<Func<T, bool>> predicate)
        {
            try
            {
                return await dal.DeleteTAsync(predicate);
            }
            catch (Exception ex)
            {
                //ErrorLogManager.Instance.AddErrorLogAsync(new Model.ErrorLog()
                //{
                //    Method = "DeleteAsync Exception<br/>T：" + typeof(T),
                //    Message = "访问URL：<a href='javascript:void(0)'>" + System.Web.HttpContext.Current.Request.Url.OriginalString + "</a><br/>" + ex.Message + "<br />" + ex.StackTrace
                //});
                return false;
            }
        }
        #endregion

        #region 记录日志的方法
        /// <summary>
        /// 添加日志（仅限记录日志调用）
        /// </summary>
        public void AddTLogAsyn(T t)
        {
            dal.AddTLogAsyn(t);
        }
        #endregion

        #region Cache相关
        ///// <summary>
        ///// 清除缓存
        ///// </summary>
        ///// <param name="cache"></param>
        //public void ClearCache(string cacheKey)
        //{
        //    System.Web.HttpRuntime.Cache.Remove(cacheKey);
        //}

        ///// <summary>
        ///// 清除缓存
        ///// </summary>
        ///// <param name="cacheSetKey"></param>
        ///// <param name="paras"></param>
        //public void ClearCache(string cacheSetKey, object[] paras)
        //{
        //    try
        //    {
        //        var codeTableCacheSet = CacheManager.Instance.GetCacheSet(cacheSetKey);
        //        var cacheKey = string.Format(codeTableCacheSet.value, paras);
        //        ClearCache(cacheKey);
        //    }
        //    catch { }
        //}
        #endregion
    }
}
