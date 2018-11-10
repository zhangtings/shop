using System;
using System.Text;
using System.Threading.Tasks;

namespace SHAN.Service
{
    public class 入住DTO
    {
        public int? Id { get; set; }
        /// <summary>
        /// 客人序号
        /// </summary>
        public int? GId { get; set; }
        /// <summary>
        /// 房间序号
        /// </summary>
        public int? RId { get; set; }
        /// <summary>
        /// 押金
        /// </summary>
        public decimal? GMoney { get; set; }
        /// <summary>
        /// 入住时间
        /// </summary>
        public int? InTime { get; set; }
        /// <summary>
        /// 离开时间
        /// </summary>
        public int? OutTime { get; set; }
        /// <summary>
        /// 酒店编号
        /// </summary>
        public int? CompanyId { get; set; }

    }
}
