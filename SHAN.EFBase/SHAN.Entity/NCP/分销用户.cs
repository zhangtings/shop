using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Threading.Tasks;

namespace SHAN.Entity
{
    [Table("ncp_fx_user")]
    public class 分销用户 : BaseEntity
    {
        [Key]
        public int Id { get; set; }
        
    }
}
