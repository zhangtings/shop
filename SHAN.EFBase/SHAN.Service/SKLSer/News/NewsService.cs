using SHAN.Repository;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using SHAN.Entity;
using System;

namespace SHAN.Service
{
    public class RosterService : IRosterService
    {
        public IEFRepository<企业用户 > _iNewsRep { get; set; }

        public List<企业用户DTO> List(企业用户DTO dto)
        {
            var libiao = _iNewsRep.Entities;
            if (dto.Cid > 0)
                libiao = libiao.Where(r => r.Cid == dto.Cid);
            if (!string.IsNullOrEmpty(dto.CompanyName))
            {
                libiao = libiao.Where(r => r.CompanyName.Contains(dto.CompanyName));
            }
            libiao = libiao.OrderByDescending(m => m.Id);
            //var libiao = _iRoomTypeRep.Entities.Where(m => m.CompanyId == 1).OrderByDescending(m => m.Id);
            return Mapper.Map<List<企业用户DTO>>(libiao.ToList());
        }

        public 企业用户DTO GetDTO(int? Id)
        {
            var dTO = new 企业用户DTO();
            if (Id != null)
                dTO =  Mapper.Map<企业用户DTO>(_iNewsRep.GetById(Id.Value));
            return dTO;
        }

        public 企业用户DTO Save(企业用户DTO dto)
        {
            try
            {
                var model = Mapper.Map<企业用户> (dto);
                if (dto.Id > 0)
                {
                    _iNewsRep.Update(model);
                }
                else
                {
                    dto.Id= _iNewsRep.Insert(model);
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

        public 企业用户DTO Del(企业用户DTO dto)
        {
            dto.Id = _iNewsRep.Delete(dto.Id);
            return dto;
        }
    }
}
