using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SHAN.Service
{
    public interface IRoomTypeService
    {
        List<房间类型DTO> RoomType { get; }
        List<房间类型DTO> List(房间类型DTO dto);
        房间类型DTO GetDTO(int? Id);
        房间类型DTO Save(房间类型DTO dto);
        房间类型DTO Del(房间类型DTO dto);
    }
}
