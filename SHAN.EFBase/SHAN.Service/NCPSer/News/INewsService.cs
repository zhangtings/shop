using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SHAN.Service
{
    public interface INewsService
    {
        List<新闻DTO> List(新闻DTO dto);
        新闻DTO GetDTO(int? Id);
        新闻DTO Save(新闻DTO dto);
        新闻DTO Del(新闻DTO dto);
    }
}
