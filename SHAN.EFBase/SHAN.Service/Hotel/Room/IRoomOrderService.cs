using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SHAN.Service
{
    public interface IRoomOrderService
    {
        List<预定DTO> RoomOrder { get; }
        List<预定DTO> List(预定DTO dto);
        预定DTO GetDTO(int? Id);
        预定DTO Save(预定DTO dto);
        预定DTO Del(预定DTO dto);
    }
}
