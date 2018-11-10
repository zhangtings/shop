using SHAN.Repository;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using SHAN.Entity;
using System;

namespace SHAN.Service
{
    public class GuestService : IGuestService
    {
        public IEFRepository<客人信息> _iGuestRep { get; set; }

        public List<客人信息DTO> Guest
        {
            get
            {
                return Mapper.Map<List<客人信息DTO>>(_iGuestRep.Entities.ToList());
            }
        }

        public List<客人信息DTO> List(客人信息DTO dto)
        {
            var libiao = _iGuestRep.Entities;
            if (dto.CompanyId > 0)
                libiao = libiao.Where(m => m.CompanyId == dto.CompanyId);
            libiao = libiao.OrderByDescending(m => m.Id);
            //var libiao = _iGuestRep.Entities.Where(m => m.CompanyId == 1).OrderByDescending(m => m.Id);
            return Mapper.Map<List<客人信息>, List<客人信息DTO>>(libiao.ToList());
        }

        public 客人信息DTO GetDTO(int? Id)
        {
            var dTO = new 客人信息DTO();
            if (Id != null)
                dTO =  Mapper.Map<客人信息DTO>(_iGuestRep.GetById(Id.Value));
            return dTO;
        }

        public 客人信息DTO Save(客人信息DTO dto)
        {
            try
            {
                var model = Mapper.Map<客人信息> (dto);
                if (dto.Id > 0)
                {
                    _iGuestRep.Update(model);
                }
                else
                {
                    dto.Id= _iGuestRep.Insert(model);
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

        public 客人信息DTO Del(客人信息DTO dto)
        {
            dto.Id = _iGuestRep.Delete(dto.Id);
            return dto;
        }
    }
}
