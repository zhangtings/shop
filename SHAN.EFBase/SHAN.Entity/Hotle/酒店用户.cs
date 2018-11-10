using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Threading.Tasks;

namespace SHAN.Entity
{
    [Table("hoteluser")]
    public class 酒店用户 : BaseEntity
    {
        [Key]
        public int Id { get; set; }
        /// <summary>
        /// 登陆账号
        /// </summary>
        public string LogName { get; set; }
        /// <summary>
        /// 登陆密码
        /// </summary>
        public string Pwd { get; set; }
        /// <summary>
        /// 用户名称
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 酒店名称
        /// </summary>
        public string CompanyName { get; set; }
        /// <summary>
        /// 酒店地址
        /// </summary>
        public string Address { get; set; }
        /// <summary>
        /// 酒店图片
        /// </summary>
        public string Pic { get; set; }
        /// <summary>
        /// 酒店图片
        /// </summary>
        public string Pic2 { get; set; }
        /// <summary>
        /// 酒店图片
        /// </summary>
        public string Pic3 { get; set; }
        /// <summary>
        /// 酒店备注
        /// </summary>
        public string Mark { get; set; }
        /// <summary>
        /// 酒店建议
        /// </summary>
        public string Tips { get; set; }


    }
}
