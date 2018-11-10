using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SHAN.Service
{
    public interface IRosterService
    {
        List<企业用户DTO> List(企业用户DTO dto);
        企业用户DTO GetDTO(int? Id);
        企业用户DTO Save(企业用户DTO dto);
        企业用户DTO Del(企业用户DTO dto);
    }
}
