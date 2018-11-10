using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SHAN.Service
{
    public interface IBusineService
    {
        List<商家DTO> List(商家DTO dto);
        商家DTO GetDTO(int? Id);
        商家DTO Save(商家DTO dto);
        商家DTO Del(商家DTO dto);
    }
}
