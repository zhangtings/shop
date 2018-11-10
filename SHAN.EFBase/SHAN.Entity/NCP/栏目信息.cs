using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Threading.Tasks;

namespace SHAN.Entity
{
    [Table("ncp_column")]
    public class 栏目信息 : BaseEntity
    {
        [Key]
        public int Id { get; set; }
        public int? Cid { get; set; }
        public string Name { get; set; }
        public string Bz_1 { get; set; }
        public int? Bz_4 { get; set; }
        public string Bz_2 { get; set; }
        public string Bz_3 { get; set; }
        public string Bz_5 { get; set; }
        public string Concent { get; set; }
        public DateTime? Addtime { get; set; }
        public int? Sort { get; set; }
    }
}
