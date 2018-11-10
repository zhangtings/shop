using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SHAN.Service
{
    public interface IProductService
    {
        List<商品DTO> List(商品DTO dto);
        商品DTO GetDTO(int? Id);
        商品DTO Save(商品DTO dto);
        商品DTO Del(商品DTO dto);
    }
}
