using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SHAN.Service
{
    public interface IRoomService
    {
        List<房间信息DTO> Room { get; }
        List<房间信息DTO> List(房间信息DTO dto);
        房间信息DTO GetDTO(int? Id);
        房间信息DTO Save(房间信息DTO dto);
        房间信息DTO Del(房间信息DTO dto);
    }
}
