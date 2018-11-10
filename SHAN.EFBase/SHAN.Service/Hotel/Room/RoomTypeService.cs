using SHAN.Repository;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using SHAN.Entity;
using System;

namespace SHAN.Service
{
    public class RoomTypeService : IRoomTypeService
    {
        public IEFRepository<房间类型 > _iRoomTypeRep { get; set; }

        public List<房间类型DTO> RoomType
        {
            get
            {
                return Mapper.Map<List<房间类型DTO>>(_iRoomTypeRep.Entities.ToList());
            }
        }

        public List<房间类型DTO> List(房间类型DTO dto)
        {
            var libiao = _iRoomTypeRep.Entities;
            if (dto.CompanyId != null)
                libiao = libiao.Where(m => m.CompanyId == dto.CompanyId);
            libiao = libiao.OrderByDescending(m => m.Id);
            //var libiao = _iRoomTypeRep.Entities.Where(m => m.CompanyId == 1).OrderByDescending(m => m.Id);
            return Mapper.Map<List<房间类型>, List<房间类型DTO>>(libiao.ToList());
        }

        public 房间类型DTO GetDTO(int? Id)
        {
            var dTO = new 房间类型DTO();
            if (Id != null)
                dTO =  Mapper.Map<房间类型DTO>(_iRoomTypeRep.GetById(Id.Value));
            return dTO;
        }

        public 房间类型DTO Save(房间类型DTO dto)
        {
            try
            {
                var model = Mapper.Map<房间类型> (dto);
                if (dto.Id > 0)
                {
                    _iRoomTypeRep.Update(model);
                }
                else
                {
                    dto.Id= _iRoomTypeRep.Insert(model);
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

        public 房间类型DTO Del(房间类型DTO dto)
        {
            dto.Id = _iRoomTypeRep.Delete(dto.Id);
            return dto;
        }
    }
}
