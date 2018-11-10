using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SHAN.Service
{
    public interface IDimService
    {
        List<维度DTO> List(维度DTO dto);
        维度DTO GetDTO(int? Id);
        维度DTO Save(维度DTO dto);
        维度DTO Del(维度DTO dto);
    }
}
