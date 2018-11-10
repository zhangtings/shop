using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Threading.Tasks;

namespace SHAN.Entity
{
    [Table("roomtype")]
    public class 房间类型 : BaseEntity
    {
        [Key]
        public int Id { get; set; }
        /// <summary>
        /// 房间类型
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 房间价格
        /// </summary>
        public decimal Price { get; set; }
        /// <summary>
        /// 加床价格
        /// </summary>
        public decimal AddPrice { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Mark { get; set; }
        /// <summary>
        /// 酒店编号
        /// </summary>
        public int CompanyId { get; set; }

    }
}
