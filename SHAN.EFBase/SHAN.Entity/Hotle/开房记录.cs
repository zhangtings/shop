using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Threading.Tasks;

namespace SHAN.Entity
{
    [Table("openroom")]
    public class 开房记录 : BaseEntity
    {
        [Key]
        public int Id { get; set; }
        /// <summary>
        /// 酒店编号
        /// </summary>
        public int CompanyId { get; set; }
        /// <summary>
        /// 入住时间
        /// </summary>
        public DateTime InTime { get; set; }
        /// <summary>
        /// 房间序号
        /// </summary>
        public int RId { get; set; }

    }
}
