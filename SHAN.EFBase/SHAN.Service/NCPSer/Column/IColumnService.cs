using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SHAN.Service
{
    public interface IColumnService
    {
        List<栏目DTO> List(栏目DTO dto);
        栏目DTO GetDTO(int? Id);
        栏目DTO Save(栏目DTO dto);
        栏目DTO Del(栏目DTO dto);
    }
}
