using SHAN.Repository;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using SHAN.Entity;

namespace SHAN.Service
{
    public class EFProductService : IEFProductService
    {
        public IEFRepository<Product> _iEFProductRep { get; set; }

        public EFProductService(IEFRepository<Product> iEFProductRep)
        {
            _iEFProductRep = iEFProductRep;
        }

        public List<ProductDTO> products
        {

            get
            {
                var libiao = _iEFProductRep.Entities.Where(m => m.Is_hot == 1 && m.Del == 0).OrderByDescending(m => m.Sort);
                //List<Product> 
                var linqprdoct = (from ls in _iEFProductRep.Entities where ls.Is_hot == 1 && ls.Del == 0 orderby ls.Sort, ls.Id select ls).ToList();
                //System.Diagnostics.Debug.WriteLine(((ObjectQuery)libiao).ToTraceString());
                //System.Diagnostics.Debug.WriteLine(((ObjectQuery)linqprdoct).ToTraceString());
                return Mapper.Map<List<Product>, List<ProductDTO>>(libiao.ToList());
            }
        }

    }
}
