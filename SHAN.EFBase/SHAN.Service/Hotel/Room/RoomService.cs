using SHAN.Repository;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using SHAN.Entity;
using System;

namespace SHAN.Service
{
    public class RoomService : IRoomService
    {
        public IEFRepository<房间信息> _iRoomRep { get; set; }

        public RoomService(IEFRepository<房间信息> iRoomRep)
        {
            _iRoomRep = iRoomRep;
        }

        public List<房间信息DTO> Room
        {
            get
            {
                return Mapper.Map<List<房间信息DTO>>(_iRoomRep.Entities.ToList());
            }
        }

        public List<房间信息DTO> List(房间信息DTO dto)
        {
            var libiao = _iRoomRep.Entities;
            if (dto.CompanyId>0)
             libiao = libiao.Where(m => m.CompanyId == dto.CompanyId);
            libiao = libiao.OrderByDescending(m => m.Id);
            return Mapper.Map<List<房间信息>, List<房间信息DTO>>(libiao.ToList());
        }

        public 房间信息DTO GetDTO(int? Id)
        {
            var dTO = new 房间信息DTO();
            if (Id != null)
                dTO =  Mapper.Map<房间信息DTO>(_iRoomRep.GetById(Id.Value));
            return dTO;
        }

        public 房间信息DTO Save(房间信息DTO dto)
        {
            try
            {
                var model = Mapper.Map<房间信息> (dto);
                if (dto.Id > 0)
                {
                    _iRoomRep.Update(model);
                }
                else
                {
                    dto.Id=_iRoomRep.Insert(model);
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

        public 房间信息DTO Del(房间信息DTO dto)
        {
            dto.Id = _iRoomRep.Delete(Mapper.Map<房间信息>(dto));
            return dto;
        }
    }
}
