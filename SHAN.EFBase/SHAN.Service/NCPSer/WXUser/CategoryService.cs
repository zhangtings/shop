using SHAN.Repository;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using SHAN.Entity;
using System;

namespace SHAN.Service
{
    public class WXUserService : IWXUserService
    {
        public IEFRepository<微信用户 > _iWXUserRep { get; set; }



        public List<前端用户DTO> List(前端用户DTO dto)
        {
            var libiao = _iWXUserRep.Entities;
            
            libiao = libiao.OrderByDescending(m => m.Id);
            //var libiao = _iRoomTypeRep.Entities.Where(m => m.CompanyId == 1).OrderByDescending(m => m.Id);
            return Mapper.Map<List<前端用户DTO>>(libiao.ToList());
        }

        public 前端用户DTO GetDTO(int? Id)
        {
            var dTO = new 前端用户DTO();
            if (Id != null)
                dTO =  Mapper.Map<前端用户DTO>(_iWXUserRep.GetById(Id.Value));
            return dTO;
        }

        public 前端用户DTO Save(前端用户DTO dto)
        {
            try
            {
                var model = Mapper.Map<微信用户> (dto);
                if (dto.Id > 0)
                {
                    _iWXUserRep.Update(model);
                }
                else
                {
                    dto.Id= _iWXUserRep.Insert(model);
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

        public 前端用户DTO Del(前端用户DTO dto)
        {
            dto.Id = _iWXUserRep.Delete(dto.Id);
            return dto;
        }
    }
}
