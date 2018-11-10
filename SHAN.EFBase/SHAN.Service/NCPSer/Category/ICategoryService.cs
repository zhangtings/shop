using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SHAN.Service
{
    public interface ICategoryService
    {
        List<分类DTO> List(分类DTO dto);
        分类DTO GetDTO(int? Id);
        分类DTO Save(分类DTO dto);
        分类DTO Del(分类DTO dto);
    }
}
