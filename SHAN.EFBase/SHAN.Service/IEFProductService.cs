using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SHAN.Service
{
    public interface IEFProductService
    {
        List<ProductDTO> products { get; }
    }
}
