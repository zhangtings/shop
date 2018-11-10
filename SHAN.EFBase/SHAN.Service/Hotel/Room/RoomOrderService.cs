using SHAN.Repository;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using SHAN.Entity;
using System;

namespace SHAN.Service
{
    public class RoomOrderService : IRoomOrderService
    {
        public IEFRepository<预定> _iRoomOrderRep { get; set; }

        public RoomOrderService(IEFRepository<预定> iRoomRep)
        {
            _iRoomOrderRep = iRoomRep;
        }

        public List<预定DTO> RoomOrder
        {
            get
            {
                return Mapper.Map<List<预定DTO>>(_iRoomOrderRep.Entities.ToList());
            }
        }

        public List<预定DTO> List(预定DTO dto)
        {
            var libiao = _iRoomOrderRep.Entities;
            if (dto.CompanyId > 0)
                libiao = libiao.Where(m => m.CompanyId == dto.CompanyId);
            libiao = libiao.OrderByDescending(m => m.Id);
            //var libiao = _iRoomOrderRep.Entities.Where(m => m.CompanyId == 1).OrderByDescending(m => m.Id);
            return Mapper.Map<List<预定>, List<预定DTO>>(libiao.ToList());
        }

        public 预定DTO GetDTO(int? Id)
        {
            var dTO = new 预定DTO();
            if (Id != null)
                dTO =  Mapper.Map<预定DTO>(_iRoomOrderRep.GetById(Id.Value));
            return dTO;
        }

        public 预定DTO Save(预定DTO dto)
        {
            try
            {
                var model = Mapper.Map<预定> (dto);
                if (dto.Id > 0)
                {
                    _iRoomOrderRep.Update(model);
                }
                else
                {
                    dto.Id= _iRoomOrderRep.Insert(model);
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

        public 预定DTO Del(预定DTO dto)
        {
            dto.Id = _iRoomOrderRep.Delete(Mapper.Map<预定>(dto));
            return dto;
        }
    }
}
