using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SHAN.Service
{
    public interface ICartService
    {
        List<购物车DTO> List(购物车DTO dto);
        购物车DTO GetDTO(int? Id);
        购物车DTO Save(购物车DTO dto);
        购物车DTO Del(购物车DTO dto);
    }
}
