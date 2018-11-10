using System;
using System.Text;
using System.Threading.Tasks;

namespace SHAN.Service
{
    public class 客人类型DTO
    {
        public int? Id { get; set; }
        /// <summary>
        /// 会员名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 折扣比例
        /// </summary>
        public decimal? Trate { get; set; }
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
