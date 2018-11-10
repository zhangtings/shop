using System;
using System.Text;
using System.Threading.Tasks;

namespace SHAN.Service
{
    public class 商家DTO
    {
        public int Id { get; set; }

        public string Title
        {
            get;
            set;
        }

        public int? Area
        {
            get;
            set;
        }

        public string Address
        {
            get;
            set;
        }

        public string Tags
        {
            get;
            set;
        }

        public string Pic
        {
            get;
            set;
        }

        public string Lat
        {
            get;
            set;
        }

        public string Lng
        {
            get;
            set;
        }

        public string Zoom
        {
            get;
            set;
        }

        public string Tel
        {
            get;
            set;
        }

        public string Des
        {
            get;
            set;
        }

        public string Quan
        {
            get;
            set;
        }

        public string ProductPic
        {
            get;
            set;
        }

        public double Distance
        {
            get;
            set;
        }

        public string EatTime
        {
            get;
            set;
        }
        public string Zhekou { get; set; }
        public string Video { get; set; }
        //标签
        public string BiaoQian { get; set; }
        public string GuangGao { get; set; }
        public int? ShenHe { get; set; }
        public string Photox { get; set; }
        public DateTime? Addtime { get; set; }

    }
}
