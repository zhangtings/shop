using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using CZ.Models.Common;

namespace CZ.Application.Tools
{
    /// <summary>
    /// 分页扩展类
    /// </summary>
    public static class PageEX
    {
        /// <summary>
        ///     从指定IQueryable[T]集合 中查询指定分页条件的子数据集
        /// </summary>
        /// <typeparam name="TEntity">动态实体类型</typeparam>
        /// <param name="source">要查询的数据集</param>
        /// <param name="pageIndex">分页索引</param>
        /// <param name="pageSize">分页大小</param>
        /// <param name="total">输出符合条件的总记录数</param>
        /// <returns></returns>
        public static IQueryable<TEntity> GetPage<TEntity>(this IQueryable<TEntity> source, int pageIndex, int pageSize, out int total) where TEntity:DDb.Entity
        {
            total = source.Count();
            return source.Skip((pageIndex - 1) * pageSize).Take(pageSize);
        }

        /// <summary>
        ///     从指定IQueryable[T]集合 中查询指定分页条件的子数据集
        /// </summary>
        /// <typeparam name="TEntity">动态实体类型</typeparam>
        /// <param name="source">要查询的数据集</param>
        /// <param name="pageIndex">分页索引</param>
        /// <param name="pageSize">分页大小</param>
        /// <param name="total">输出符合条件的总记录数</param>
        /// <returns></returns>
        public static IQueryable<TEntity> GetPage<TEntity>(this IQueryable<TEntity> source, Pagination Pager) where TEntity : DDb.Entity
        {
            if (Pager == null)
            {
                Pager=new Pagination()
                {
                    CurrentPage = 1,
                    PageSize = 10
                };
            }

            if (Pager.Total==0) Pager.Total = source.Count();
            Pager.PageNum = Convert.ToInt32( Math.Ceiling(Convert.ToDecimal(Pager.Total) / Pager.PageSize));
            return source.Skip((Pager.CurrentPage - 1) * Pager.PageSize).Take(Pager.PageSize);
        }
    }
}
