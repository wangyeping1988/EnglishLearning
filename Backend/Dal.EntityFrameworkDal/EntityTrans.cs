using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace JasonWang.Dal.EntityFrameworkDal
{
    ///
    /// 类属性/字段的值复制工具
    ///
    public class EntityTrans<T> where T : class
    {
        ///
        /// 复制
        ///
        /// 目标
        /// 来源
        /// 成功复制的值个数
        public static int Copy(T destination, T source)
        {
            if (destination == null || source == null)
            {
                return 0;
            }
            return Copy(destination, source, source.GetType());
        }

        ///
        /// 复制
        ///
        /// 目标
        /// 来源
        /// 复制的属性字段模板
        /// 成功复制的值个数
        public static int Copy(T destination, T source, Type type)
        {
            return Copy(destination, source, type, null);
        }

        ///
        /// 复制
        ///
        /// 目标
        /// 来源
        /// 复制的属性字段模板
        /// 排除下列名称的属性不要复制
        /// 成功复制的值个数
        public static int Copy(T destination, T source, Type type, List<string> excludeName)
        {
            if (destination == null || source == null)
            {
                return 0;
            }

            if (excludeName == null)
            {
                excludeName = new List<string>();
            }

            int i = 0;
            Type desType = destination.GetType();
            foreach (FieldInfo mi in type.GetFields())
            {
                if (excludeName.Contains(mi.Name))
                {
                    continue;
                }

                try
                {
                    FieldInfo des = desType.GetField(mi.Name);
                    if (des != null && des.FieldType == mi.FieldType)
                    {
                        des.SetValue(destination, mi.GetValue(source));
                        i++;
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            foreach (PropertyInfo pi in type.GetProperties())
            {
                if (excludeName.Contains(pi.Name))
                {
                    continue;
                }

                try
                {
                    PropertyInfo des = desType.GetProperty(pi.Name);
                    if (des != null && des.PropertyType == pi.PropertyType && des.CanWrite && pi.CanRead)
                    {
                        des.SetValue(destination, pi.GetValue(source, null), null);
                        i++;
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return i;
        }
    }

    #region 扩展方法 For .NET 3.0+
    ///
    /// 类属性/字段的值复制工具 扩展方法
    ///
    public class EntityTrans2<T> where T : class
    {
        ///
        /// 获取实体类的属性名称
        ///
        /// 实体类
        /// 属性名称列表
        public List<string> GetPropertyNames(T source)
        {
            if (source == null)
            {
                return new List<string>();
            }
            return GetPropertyNames(source.GetType());
        }

        ///
        /// 获取类类型的属性名称（按声明顺序）
        ///
        /// 类类型
        /// 属性名称列表
        public List<string> GetPropertyNames(Type source)
        {
            return GetPropertyNames(source, true);
        }

        ///
        /// 获取类类型的属性名称
        ///
        /// 类类型
        /// 是否按声明顺序排序
        /// 属性名称列表
        public List<string> GetPropertyNames(Type source, bool declarationOrder)
        {
            if (source == null)
            {
                return new List<string>();
            }
            var list = source.GetProperties().AsQueryable();
            if (declarationOrder)
            {
                list = list.OrderBy(p => p.MetadataToken);
            }
            return list.Select(o => o.Name).ToList(); ;
        }

        ///
        /// 从源对象赋值到当前对象
        ///
        /// 当前对象
        /// 源对象
        /// 成功复制的值个数
        public int CopyValueFrom(T destination, T source)
        {
            return CopyValueFrom(destination, source, null);
        }

        ///
        /// 从源对象赋值到当前对象
        ///
        /// 当前对象
        /// 源对象
        /// 排除下列名称的属性不要复制
        /// 成功复制的值个数
        public int CopyValueFrom(T destination, T source, List<string> excludeName)
        {
            if (destination == null || source == null)
            {
                return 0;
            }
            return EntityTrans<T>.Copy(destination, source, source.GetType(), excludeName);
        }

        ///
        /// 从当前对象赋值到目标对象
        ///
        /// 当前对象
        /// 目标对象
        /// 成功复制的值个数
        public int CopyValueTo(T source, T destination)
        {
            return CopyValueTo(destination, source, null);
        }

        ///
        /// 从当前对象赋值到目标对象
        ///
        /// 当前对象
        /// 目标对象
        /// 排除下列名称的属性不要复制
        /// 成功复制的值个数
        public int CopyValueTo(T source, T destination, List<string> excludeName)
        {
            if (destination == null || source == null)
            {
                return 0;
            }
            return EntityTrans<T>.Copy(destination, source, source.GetType(), excludeName);
        }
    }
    #endregion
}
