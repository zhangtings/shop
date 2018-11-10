using SHAN.Repository;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using SHAN.Entity;
using System;

namespace SHAN.Service
{
    public class DimService : IDimService
    {
        public IEFRepository<维度信息 > _iColRep { get; set; }



        public List<维度DTO> List(维度DTO dto)
        {
            var libiao = _iColRep.Entities;
            
            libiao = libiao.OrderByDescending(m => m.Id);
            //var libiao = _iRoomTypeRep.Entities.Where(m => m.CompanyId == 1).OrderByDescending(m => m.Id);
            return Mapper.Map<List<维度DTO>>(libiao.ToList());
        }

        public 维度DTO GetDTO(int? Id)
        {
            var dTO = new 维度DTO();
            if (Id != null)
                dTO =  Mapper.Map<维度DTO>(_iColRep.GetById(Id.Value));
            return dTO;
        }

        public 维度DTO Save(维度DTO dto)
        {
            try
            {
                var model = Mapper.Map<维度信息> (dto);
                if (dto.Id > 0)
                {
                    _iColRep.Update(model);
                }
                else
                {
                    dto.Id= _iColRep.Insert(model);
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

        public 维度DTO Del(维度DTO dto)
        {
            dto.Id = _iColRep.Delete(dto.Id);
            return dto;
        }
    }
}
