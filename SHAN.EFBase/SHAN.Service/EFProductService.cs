using SHAN.Repository;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using SHAN.Entity;
using System.Data.Entity;
using AutoMapper.QueryableExtensions;

namespace SHAN.Service
{
    public class EFProductService : IEFProductService
    {
        public IEFRepository<Product> _iEFProductRep { get; set; }
        int? i;
        private string s;
        public EFProductService(IEFRepository<Product> iEFProductRep)
        {
            _iEFProductRep = iEFProductRep;
        }

        public List<ProductDTO> products
        {

            get
            {
                
                //List<Product> 
                var linqprdoct = _iEFProductRep.Entities.Where(m => m.Is_hot == 1 && m.Del == 0).OrderByDescending(m => m.Sort);//(from ls in _iEFProductRep.Entities where ls.Is_hot == 1 && ls.Del == 0 orderby ls.Sort, ls.Id select ls).ToList();
                var libiao = _iEFProductRep.实体集.AsNoTracking().Where(m => m.Is_hot == 1 && m.Del == 0).OrderByDescending(m => m.Sort).ProjectTo<ProductDTO>();
                //string aa = string.IsNullOrEmpty(s)?"a":"b";
                //var xx = linqprdoct.Where(r => string.IsNullOrEmpty(s)?true:r.Name==s);
                //var yy = xx.ToString();
                //System.Diagnostics.Debug.WriteLine(((ObjectQuery)libiao).ToTraceString());
                //System.Diagnostics.Debug.WriteLine(((ObjectQuery)linqprdoct).ToTraceString());
                //return Mapper.Map<List<Product>, List<ProductDTO>>(libiao.ToList());
                var tmp= Mapper.Map<List<Product>, List<ProductDTO>>(linqprdoct.ToList());
                tmp.ForEach(r => r.Pro_type1 = 1);
                var tmp1 = Mapper.Map<List<ProductDTO>, List<Product>>(tmp);
                return libiao.ToList();
            }
        }

    }
}
