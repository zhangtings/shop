using System;
using System.Text;
using System.Threading.Tasks;

namespace SHAN.Service
{
    public class 客人信息DTO 
    {
        public int? Id { get; set; }
        /// <summary>
        /// 客人名字
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 客人类型
        /// </summary>
        public int? GTId { get; set; }
        /// <summary>
        /// 性别
        /// </summary>
        public int? Sex { get; set; }
        /// <summary>
        /// 电话
        /// </summary>
        public string Mobile { get; set; }
        /// <summary>
        /// 身份证号
        /// </summary>
        public string PId { get; set; }
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
