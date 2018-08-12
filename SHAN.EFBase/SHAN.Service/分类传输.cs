using System;
using System.Text;
using System.Threading.Tasks;

namespace SHAN.Service
{
    public class 分类传输
    {
        public int Id { get; set; }
        public int Tid { get; set; }
        public Nullable<long> Brand_id { get; set; }
        public string Name { get; set; }
        public string Bz_1 { get; set; }
        public int Bz_4 { get; set; }
        public string Bz_2 { get; set; }
        public string Bz_3 { get; set; }
        public string Bz_5 { get; set; }
        public string Concent { get; set; }
        public Nullable<int> Addtime { get; set; }
        public Nullable<int> Sort { get; set; }
    }
}
