using System;
using System.Text;
using System.Threading.Tasks;

namespace SHAN.Service
{
    public class 房间信息DTO 
    {
        public int? Id { get; set; }
        /// <summary>
        /// 房号
        /// </summary>
        public int? Number { get; set; }
        /// <summary>
        /// 房间类型
        /// </summary>
        public int? RTId { get; set; }
        /// <summary>
        /// 房间状态
        /// </summary>
        public string RSId { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Mark { get; set; }
        /// <summary>
        /// 酒店编号
        /// </summary>
        public int? CompanyId { get; set; }
    }
}
