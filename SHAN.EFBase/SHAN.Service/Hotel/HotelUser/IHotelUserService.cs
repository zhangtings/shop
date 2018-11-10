using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SHAN.Service
{
    public interface IHotelUserService
    {
        
        List<酒店用户DTO> List(酒店用户DTO dto);
        酒店用户DTO GetDTO(int? Id);
        酒店用户DTO Save(酒店用户DTO dto);
        酒店用户DTO Del(酒店用户DTO dto);
    }
}
