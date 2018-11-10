using System;
using System.Text;
using System.Threading.Tasks;

namespace SHAN.Service
{
    public class 开房记录DTO 
    {
        public int? Id { get; set; }
        /// <summary>
        /// 酒店编号
        /// </summary>
        public int? CompanyId { get; set; }
        /// <summary>
        /// 入住时间
        /// </summary>
        public int? InTime { get; set; }
        /// <summary>
        /// 房间序号
        /// </summary>
        public int? RId { get; set; }

    }
}
