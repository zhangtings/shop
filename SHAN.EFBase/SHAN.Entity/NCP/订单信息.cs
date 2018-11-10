using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Threading.Tasks;

namespace SHAN.Entity
{
    [Table("ncp_Order")]
    public class 订单信息 : BaseEntity
    {
        [Key]
        public int Id { get; set; }
        
    }
}
