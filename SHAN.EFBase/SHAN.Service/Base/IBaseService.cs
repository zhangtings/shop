using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SHAN.Service
{
    public interface IBaseService<T,T1>
    {
        
        List<T> List(T dto);
        T GetDTO(int? Id);
        T Save(T dto);
        T Del(T dto);
    }
}
