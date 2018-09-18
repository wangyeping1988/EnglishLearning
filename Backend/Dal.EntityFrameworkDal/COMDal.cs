using JasonWang.Dal.EntityFrameworkDal.AuxiliaryModels;
using JasonWang.Dal.EntityFrameworkDal.Context;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;

namespace JasonWang.Dal.EntityFrameworkDal
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class COMDal<T> where T : class
    {
        public COMDal()
        {
        }

        public COMDal(DefaultContext context)
        {
            _context = context;
        }

        /// <summary>
        /// EF DbContext
        /// </summary>
        protected DefaultContext _context = null;

        /// <summary>
        /// Get DbSet
        /// (use T to instead all kinds of DB table, so no need to specify the DB table name)
        /// (then use this to execute Linq clauses to access DB)
        /// </summary>
        protected DbSet<T> Table { get { return _context.Set<T>(); } }

        #region CURD操作

        #region AsNoTracking
        /// <summary>
        /// get list async
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public async Task<List<T>> GetListTAsync(Expression<Func<T, bool>> predicate)
        {
            var list = await Table.Where(predicate).AsNoTracking().ToListAsync();
            return list.Count() == 0 ? null : list;
        }

        public List<T> GetListT(Expression<Func<T, bool>> predicate)
        {
            var list = Table.Where(predicate).AsNoTracking().ToList();
            return list.Count() == 0 ? null : list;
        }

        public async Task<int> GetListTCountAsync(Expression<Func<T, bool>> predicate)
        {
            return await Table.AsNoTracking().CountAsync(predicate);
        }


        /// <summary>
        /// get T async
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public async Task<T> GetTAsync(Expression<Func<T, bool>> predicate)
        {
            return await Table.AsNoTracking().FirstOrDefaultAsync(predicate);
        }

        public T GetT(Expression<Func<T, bool>> predicate)
        {
            return Table.AsNoTracking().FirstOrDefault(predicate);
        }

        /// <summary>
        /// is value under existing conditions
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public async Task<bool> IsExistsAsync(Expression<Func<T, bool>> predicate)
        {
            return await Table.Where(predicate).AsNoTracking().CountAsync() > 0;
        }
        #endregion

        #region Tracking
        /// <summary>
        /// get list async
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public async Task<List<T>> GetListTAsyncTracking(Expression<Func<T, bool>> predicate)
        {
            var list = await Table.Where(predicate).ToListAsync();
            return list.Count() == 0 ? null : list;
        }

        public async Task<int> GetListTCountAsyncTracking(Expression<Func<T, bool>> predicate)
        {
            return await Table.CountAsync(predicate);
        }
        /// <summary>
        /// get T async
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public async Task<T> GetTAsyncTracking(Expression<Func<T, bool>> predicate)
        {
            return await Table.FirstOrDefaultAsync(predicate);
        }

        public T GetTTracking(Expression<Func<T, bool>> predicate)
        {
            return Table.FirstOrDefault(predicate);
        }

        /// <summary>
        /// is value under existing conditions
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public async Task<bool> IsExistsAsyncTracking(Expression<Func<T, bool>> predicate)
        {
            return await Table.Where(predicate).CountAsync() > 0;
        }
        #endregion

        /// <summary>
        /// get list async by pager
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="order"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public async Task<Pager<T>> GetListTAsync(Expression<Func<T, bool>> predicate, Order order, int pageIndex, int pageSize)
        {
            var allCount = await Table.Where(predicate).AsNoTracking().LongCountAsync();
            if (allCount > (pageIndex - 1) * pageSize)
            {
                var list = Table.Where(predicate).AsNoTracking().OrderBy<T>(order).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
                return new Pager<T>() { AllCount = allCount, List = list };
            }
            return new Pager<T>() { AllCount = allCount, List = default(List<T>) };
        }


        /// <summary>
        /// add T async
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public async Task<bool> AddTAsync(T t)
        {
            if (t == null) return false;
            try
            {
                Table.Add(t);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// update T async
        /// </summary>
        /// <param name="t"></param>
        /// <param name="predicate"></param>
        /// <param name="excludeColumns"></param>
        /// <returns></returns>
        public async Task<bool> UpdateTAsync(T t, Expression<Func<T, bool>> predicate, List<string> excludeColumns)
        {
            if (t == null) return false;
            try
            {
                //var model = await GetTAsync(predicate);//2017-4-12潘珅修改，原方法加上了AsNoTracking
                var model = await GetTAsyncTracking(predicate);
                if (model != null)
                {
                    new EntityTrans2<T>().CopyValueFrom(model, t, excludeColumns);  //copy need update columns from source(T) to destionation(model)
                    await _context.SaveChangesAsync();
                }
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool UpdateT(T t, Expression<Func<T, bool>> predicate, List<string> excludeColumns)
        {
            if (t == null) return false;
            try
            {
                //var model = await GetTAsync(predicate);//2017-4-12潘珅修改，原方法加上了AsNoTracking
                var model = GetT(predicate);
                if (model != null)
                {
                    new EntityTrans2<T>().CopyValueFrom(model, t, excludeColumns);  //copy need update columns from source(T) to destionation(model)
                    RemoveHoldingEntityInContext(model);
                    _context.Entry(model).State = EntityState.Modified;
                    _context.SaveChanges();
                }
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 用于监测Context中的Entity是否存在，如果存在，将其Detach，防止出现问题。
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        private Boolean RemoveHoldingEntityInContext(T entity)
        {
            var objContext = ((IObjectContextAdapter)_context).ObjectContext;
            var objSet = objContext.CreateObjectSet<T>();
            var entityKey = objContext.CreateEntityKey(objSet.EntitySet.Name, entity);

            Object foundEntity;
            var exists = objContext.TryGetObjectByKey(entityKey, out foundEntity);

            if (exists)
            {
                objContext.Detach(foundEntity);
            }

            return (exists);
        }

        /// <summary>
        /// delete T async
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public async Task<bool> DeleteTAsync(Expression<Func<T, bool>> predicate)
        {
            try
            {
                //无法删除多条记录 潘珅修改20170330
                //var model = await GetTAsync(predicate);
                //if (model != null)
                //{
                //    Table.Remove(model);
                //    await _context.SaveChangesAsync();
                //}
                // var model = await GetListTAsync(predicate);//2017-4-12潘珅修改，原方法加上了AsNoTracking
                var model = await GetListTAsyncTracking(predicate);
                if (model != null)
                {
                    foreach (var o in model)
                        Table.Remove(o);
                    await _context.SaveChangesAsync();
                }
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        #endregion

        #region 记录日志的方法
        /// <summary>
        /// 同步添加日志方法
        /// </summary>
        /// <param name="t"></param>
        private void AddTLogSync(T t)
        {
            Table.Add(t);
            _context.SaveChangesAsync();
        }

        /// <summary>
        /// 异步添加日志
        /// </summary>
        /// <param name="t"></param>
        public void AddTLogAsyn(T t)
        {
            System.Threading.Tasks.Task.Factory.StartNew(() => AddTLogSync(t));
        }
        #endregion
    }

    /// <summary>
    /// Extension methods for ordering and paging
    /// </summary>
    public static class OrderSelecter
    {
        public static IOrderedQueryable<T> OrderBy<T>(this IQueryable<T> source, Order order)
        {
            if (order.Sort.ToLower() == "asc")
                return ApplyOrder<T>(source, order.Column, "OrderBy");
            else
                return ApplyOrder<T>(source, order.Column, "OrderByDescending");
        }

        private static IOrderedQueryable<T> OrderBy<T>(this IQueryable<T> source, string property)
        {
            return ApplyOrder<T>(source, property, "OrderBy");

        }

        private static IOrderedQueryable<T> OrderByDescending<T>(this IQueryable<T> source, string property)
        {
            return ApplyOrder<T>(source, property, "OrderByDescending");
        }

        private static IOrderedQueryable<T> ThenBy<T>(this IOrderedQueryable<T> source, string property)
        {
            return ApplyOrder<T>(source, property, "ThenBy");
        }

        private static IOrderedQueryable<T> ThenByDescending<T>(this IOrderedQueryable<T> source, string property)
        {
            return ApplyOrder<T>(source, property, "ThenByDescending");
        }

        private static IOrderedQueryable<T> ApplyOrder<T>(IQueryable<T> source, string property, string methodName)
        {
            string[] props = property.Split('.');
            Type type = typeof(T); ParameterExpression arg = Expression.Parameter(type, "x");
            Expression expr = arg; foreach (string prop in props)
            {
                // use reflection (not ComponentModel) to mirror LINQ 
                PropertyInfo pi = type.GetProperty(prop, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase); expr = Expression.Property(expr, pi); type = pi.PropertyType;
            }
            Type delegateType = typeof(Func<,>).MakeGenericType(typeof(T), type);
            LambdaExpression lambda = Expression.Lambda(delegateType, expr, arg);
            object result = typeof(Queryable).GetMethods().Single(method => method.Name == methodName
                            && method.IsGenericMethodDefinition
                            && method.GetGenericArguments().Length == 2
                            && method.GetParameters().Length == 2)
                            .MakeGenericMethod(typeof(T), type)
                            .Invoke(null, new object[] { source, lambda });
            return (IOrderedQueryable<T>)result;
        }
    }
}
