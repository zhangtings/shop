using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Threading.Tasks;

namespace SHAN.Entity
{
    [Table("ncp_wx_user_address")]
    public class 用户地址 : BaseEntity
    {
        [Key]
        public int Id { get; set; }
        
    }
}
