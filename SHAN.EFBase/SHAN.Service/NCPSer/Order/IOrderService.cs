using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SHAN.Service
{
    public interface IOrderService
    {
        List<订单DTO> List(订单DTO dto);
        订单DTO GetDTO(int? Id);
        订单DTO Save(订单DTO dto);
        订单DTO Del(订单DTO dto);

        List<订单的商品DTO> List(订单的商品DTO dto);
        订单的商品DTO GetOPDTO(int? Id);
        订单的商品DTO Save(订单的商品DTO dto);
        订单的商品DTO Del(订单的商品DTO dto);
    }
}
