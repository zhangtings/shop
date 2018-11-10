using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SHAN.Service
{
    public interface IGuestService
    {
        List<客人信息DTO> Guest { get; }
        List<客人信息DTO> List(客人信息DTO dto);
        客人信息DTO GetDTO(int? Id);
        客人信息DTO Save(客人信息DTO dto);
        客人信息DTO Del(客人信息DTO dto);
    }
}
