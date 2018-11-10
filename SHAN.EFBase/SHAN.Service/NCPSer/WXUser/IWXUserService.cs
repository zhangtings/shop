using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SHAN.Service
{
    public interface IWXUserService
    {
        List<前端用户DTO> List(前端用户DTO dto);
        前端用户DTO GetDTO(int? Id);
        前端用户DTO Save(前端用户DTO dto);
        前端用户DTO Del(前端用户DTO dto);
    }
}
