using System;
using System.Text;
using System.Threading.Tasks;

namespace SHAN.Service
{
    public class 预定DTO
    {
        public int Id { get; set; }
        /// <summary>
        /// 酒店编号
        /// </summary>
        public int CompanyId { get; set; }
        /// <summary>
        /// 酒店名称
        /// </summary>
        public string CompanyName { get; set; }
        /// <summary>
        /// 入住时间
        /// </summary>
        public DateTime? InTime { get; set; }
        /// <summary>
        /// 离开时间
        /// </summary>
        public DateTime? OutTime { get; set; }
        /// <summary>
        /// 客人序号
        /// </summary>
        public int GId { get; set; }
        /// <summary>
        /// 客人名称
        /// </summary>
        public string GName { get; set; }
        /// <summary>
        /// 客人电话
        /// </summary>
        public string GMobile { get; set; }
        /// <summary>
        /// 房间类型
        /// </summary>
        public int RTId { get; set; }
        /// <summary>
        /// 客人类型/会员类型
        /// </summary>
        public int GTId { get; set; }
        /// <summary>
        /// 从哪里定的
        /// </summary>
        public string FromAPP { get; set; }
        public string InTimeStr { get { return InTime.Value.ToString("MM月dd日"); }}

    }
}
