using System;
using System.Text;
using System.Threading.Tasks;

namespace SHAN.Service
{
    public class 新闻DTO
    {
        public int Id { get; set; }
        public int? Shop_id { get; set; }
        public long? Brand_id { get; set; }
        public string Name { get; set; }
        public string Intro { get; set; }
        public string Pro_number { get; set; }
        public decimal? Price { get; set; }
        public decimal? Price_yh { get; set; }
        public int? Price_jf { get; set; }
        public string Photo_x { get; set; }
        public string Photo_d { get; set; }
        public string photo_string { get; set; }
        public string Content { get; set; }
        public DateTime? Addtime { get; set; }
        public DateTime? Updatetime { get; set; }
        public int? Sort { get; set; }
        public int? Renqi { get; set; }
        public int? Shiyong { get; set; }
        public int? Num { get; set; }
        public int? Type { get; set; }
        public int? Del { get; set; }
        public DateTime? Del_time { get; set; }
        public string Pro_buff { get; set; }
        public int? Cid { get; set; }
        public string Company { get; set; }
        public int? Is_show { get; set; }
        public int? Is_down { get; set; }
        public int? Is_hot { get; set; }
        public int? Is_sale { get; set; }
        public DateTime? Start_time { get; set; }
        public DateTime? End_time { get; set; }
        public int? Pro_type { get; set; }
        public string CName { get; set; }

    }
}
