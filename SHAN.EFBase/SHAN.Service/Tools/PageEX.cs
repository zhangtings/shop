using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using SHAN.Service.DTO;

namespace Hotel.Application.Tools
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
        public static IQueryable<TEntity> GetPage<TEntity>(this IQueryable<TEntity> source, int pageIndex, int pageSize, out int total) where TEntity: SHAN.Entity.BaseEntity
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
        public static IQueryable<TEntity> GetPage<TEntity>(this IQueryable<TEntity> source, PageDTO Page) where TEntity : SHAN.Entity.BaseEntity
        {
            if (Page == null)
            {
                Page = new PageDTO()
                {
                    CurrentPage = 1,
                    PageSize = 10
                };
            }

            if (Page.Total==0) Page.Total = source.Count();
            Page.PageNum = Convert.ToInt32( Math.Ceiling(Convert.ToDecimal(Page.Total) / Page.PageSize));
            return source.Skip((Page.CurrentPage - 1) * Page.PageSize).Take(Page.PageSize);
        }
    }
}
