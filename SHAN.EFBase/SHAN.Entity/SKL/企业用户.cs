using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Threading.Tasks;

namespace SHAN.Entity
{
    [Table("company_user")]
    public class 企业用户 : BaseEntity
    {
        [Key]
        public int Id { get; set; }

        public string LogName { get; set; }

        public string Pwd { get; set; }

        public string UserName { get; set; }

        public string CompanyName { get; set; }

        public string Address { get; set; }

        public string WebSite { get; set; }

        public string Service { get; set; }

        public string Tel { get; set; }

        public string Pic { get; set; }

        public string Pic2 { get; set; }

        public string Pic3 { get; set; }

        public string Mark { get; set; }

        public string Tips { get; set; }

        public int? Cid { get; set; }

    }
}
