using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SHAN.Service
{
    public interface IGuestTypeService
    {
        List<客人类型DTO> GuestType { get; }
        List<客人类型DTO> List(客人类型DTO dto);
        客人类型DTO GetDTO(int? Id);
        客人类型DTO Save(客人类型DTO dto);
        客人类型DTO Del(客人类型DTO dto);
    }
}
