using SHAN.Repository;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using SHAN.Entity;
using System;

namespace SHAN.Service
{
    public class CartService : ICartService
    {
        public IEFRepository<购物车> _iCartRep { get; set; }



        public List<购物车DTO> List(购物车DTO dto)
        {
            var libiao = _iCartRep.Entities;
            
            libiao = libiao.OrderByDescending(m => m.Id);
            //var libiao = _iRoomTypeRep.Entities.Where(m => m.CompanyId == 1).OrderByDescending(m => m.Id);
            return Mapper.Map<List<购物车DTO>>(libiao.ToList());
        }

        public 购物车DTO GetDTO(int? Id)
        {
            var dTO = new 购物车DTO();
            if (Id != null)
                dTO =  Mapper.Map<购物车DTO>(_iCartRep.GetById(Id.Value));
            return dTO;
        }

        public 购物车DTO Save(购物车DTO dto)
        {
            try
            {
                var model = Mapper.Map<购物车> (dto);
                if (dto.Id > 0)
                {
                    _iCartRep.Update(model);
                }
                else
                {
                    dto.Id= _iCartRep.Insert(model);
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

        public 购物车DTO Del(购物车DTO dto)
        {
            dto.Id = _iCartRep.Delete(dto.Id);
            return dto;
        }
    }
}
