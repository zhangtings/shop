using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Threading.Tasks;

namespace SHAN.Entity
{
    [Table("room")]
    public class 房间信息 : BaseEntity
    {
        [Key]
        public int Id { get; set; }
        /// <summary>
        /// 房号
        /// </summary>
        public string Number { get; set; }
        /// <summary>
        /// 房间类型
        /// </summary>
        public int RTId { get; set; }
        /// <summary>
        /// 房间状态
        /// </summary>
        public int RSId { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string mark { get; set; }
        /// <summary>
        /// 酒店编号
        /// </summary>
        public int CompanyId { get; set; }
    }
}
