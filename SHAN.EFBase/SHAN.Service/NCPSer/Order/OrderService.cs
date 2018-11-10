using SHAN.Repository;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using SHAN.Entity;
using System;

namespace SHAN.Service
{
    public class OrderService : IOrderService
    {
        public IEFRepository<订单信息 > _iOrderRep { get; set; }
        public IEFRepository<订单的商品> _iOPRep { get; set; }


        public List<订单DTO> List(订单DTO dto)
        {
            var libiao = _iOrderRep.Entities;
            
            libiao = libiao.OrderByDescending(m => m.Id);
            return Mapper.Map<List<订单DTO>>(libiao.ToList());
        }

        public 订单DTO GetDTO(int? Id)
        {
            var dTO = new 订单DTO();
            if (Id != null)
                dTO =  Mapper.Map<订单DTO>(_iOrderRep.GetById(Id.Value));
            return dTO;
        }

        public 订单DTO Save(订单DTO dto)
        {
            try
            {
                var model = Mapper.Map<订单信息> (dto);
                if (dto.Id > 0)
                {
                    _iOrderRep.Update(model);
                }
                else
                {
                    dto.Id= _iOrderRep.Insert(model);
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

        public 订单DTO Del(订单DTO dto)
        {
            dto.Id = _iOrderRep.Delete(dto.Id);
            return dto;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public List<订单的商品DTO> List(订单的商品DTO dto)
        {
            var libiao = _iOrderRep.Entities;

            libiao = libiao.OrderByDescending(m => m.Id);
            return Mapper.Map<List<订单的商品DTO>>(libiao.ToList());
        }

        public 订单的商品DTO GetOPDTO(int? Id)
        {
            var dTO = new 订单的商品DTO();
            if (Id != null)
                dTO = Mapper.Map<订单的商品DTO>(_iOPRep.GetById(Id.Value));
            return dTO;
        }

        public 订单的商品DTO Save(订单的商品DTO dto)
        {
            try
            {
                var model = Mapper.Map<订单的商品>(dto);
                if (dto.Id > 0)
                {
                    _iOPRep.Update(model);
                }
                else
                {
                    dto.Id = _iOPRep.Insert(model);
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

        public 订单的商品DTO Del(订单的商品DTO dto)
        {
            dto.Id = _iOPRep.Delete(dto.Id);
            return dto;
        }
    }
}
