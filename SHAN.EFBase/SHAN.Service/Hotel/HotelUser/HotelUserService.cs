using SHAN.Repository;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using SHAN.Entity;
using System;

namespace SHAN.Service
{
    public class HotelUserService : IHotelUserService
    {
        public IEFRepository<酒店用户> _iHURep { get; set; }

        public List<酒店用户DTO> List(酒店用户DTO dto)
        {
            var libiao = _iHURep.Entities.OrderByDescending(m => m.Id);
            return Mapper.Map<List<酒店用户>, List<酒店用户DTO>>(libiao.ToList());
        }

        public 酒店用户DTO GetDTO(int? Id)
        {
            var dTO = new 酒店用户DTO();
            if (Id != null)
                dTO =  Mapper.Map<酒店用户DTO>(_iHURep.GetById(Id.Value));
            return dTO;
        }

        public 酒店用户DTO Save(酒店用户DTO dto)
        {
            try
            {
                var model = Mapper.Map<酒店用户> (dto);
                if (dto.Id > 0)
                {
                    _iHURep.Update(model);
                }
                else
                {
                    dto.Id=_iHURep.Insert(model);
                }
                //result.code = "0";
                //result.msg = "保存成功";
                return dto;
            }
            catch (Exception er)
            {
                throw er;
            }
        }

        public 酒店用户DTO Del(酒店用户DTO dto)
        {
            dto.Id = _iHURep.Delete(dto.Id);
            return dto;
        }
    }
}
