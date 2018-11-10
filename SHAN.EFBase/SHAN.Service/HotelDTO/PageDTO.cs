using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SHAN.Service.DTO
{
    /// <summary>
    /// 分页
    /// </summary>
    public class PageDTO
    {
        public int PageSize
        {
            get;
            set;
        }

        public int CurrentPage
        {
            get;
            set;
        }

        //记录总数
        public int Total
        {
            get;
            set;
        }

        public int PageNum
        {
            get;
            set;
        }

        //
        public int OrderType
        {
            get;
            set;
        }

        //
        public string OrderFiled
        {
            get;
            set;
        }
    }
}
