using AutoMapper;
using System.Collections.Generic;
using SHAN.Entity;
using System.Text;
using System.Threading.Tasks;

namespace SHAN.Service
{
    /// <summary>
    /// AutoMapper 自定义扩展配置
    /// </summary>
    public class AutoMapperConfiguration
    {
        public static void ConfigExt()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.ValidateInlineMaps = false;
                cfg.CreateMissingTypeMaps = true;
            });
        }
    }
}
