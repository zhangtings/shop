using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Threading.Tasks;

namespace SHAN.Entity
{
    [Table("ncp_cart")]
    public class 购物车 : BaseEntity
    {
        [Key]
        public int Id { get; set; }
        
    }
}
