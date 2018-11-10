using SHAN.Repository;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using SHAN.Entity;
using System;

namespace SHAN.Service
{
    public class GuestTypeService : IGuestTypeService
    {
        public IEFRepository<客人类型> _iGuestTypeRep { get; set; }

        public List<客人类型DTO> GuestType
        {
            get
            {
                return Mapper.Map<List<客人类型DTO>>(_iGuestTypeRep.Entities.ToList());
            }
        }

        public List<客人类型DTO> List(客人类型DTO dto)
        {
            var libiao = _iGuestTypeRep.Entities;
            if (dto.CompanyId !=null)
                libiao = libiao.Where(m => m.CompanyId == dto.CompanyId);
            libiao = libiao.OrderByDescending(m => m.Id);
            //var libiao = _iGuestTypeRep.Entities.Where(m => m.CompanyId == 1).OrderByDescending(m => m.Id);
            return Mapper.Map<List<客人类型>, List<客人类型DTO>>(libiao.ToList());
        }

        public 客人类型DTO GetDTO(int? Id)
        {
            var dTO = new 客人类型DTO();
            if (Id != null)
                dTO =  Mapper.Map<客人类型DTO>(_iGuestTypeRep.GetById(Id.Value));
            return dTO;
        }

        public 客人类型DTO Save(客人类型DTO dto)
        {
            try
            {
                var model = Mapper.Map<客人类型> (dto);
                if (dto.Id > 0)
                {
                    _iGuestTypeRep.Update(model);
                }
                else
                {
                    dto.Id= _iGuestTypeRep.Insert(model);
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

        public 客人类型DTO Del(客人类型DTO dto)
        {
            dto.Id = _iGuestTypeRep.Delete(dto.Id);
            return dto;
        }
    }
}
