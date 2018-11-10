using SHAN.Repository;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using SHAN.Entity;
using System;

namespace SHAN.Service
{
    public class ProductService : IProductService
    {
        public IEFRepository<商品信息 > _iProRep { get; set; }



        public List<商品DTO> List(商品DTO dto)
        {
            var libiao = _iProRep.Entities;
            if (dto.Is_down != null) libiao = libiao.Where(a => a.Is_down == dto.Is_down);
            if (dto.Del == null) dto.Del = 0;//默认查询删除标记是0的记录,大于2查所有记录。
            if (dto.Del < 2) libiao = libiao.Where(a => a.Del == dto.Del);
            if (dto.Is_show < 2) libiao = libiao.Where(a => a.Is_show == dto.Is_show);
            if (dto.Is_hot == 1) libiao = libiao.Where(a => a.Is_hot == dto.Is_hot);
            if (dto.Cid >0) libiao = libiao.Where(a => a.Cid == dto.Cid);
            libiao = libiao.OrderByDescending(m => m.Id);
            //var libiao = _iRoomTypeRep.Entities.Where(m => m.CompanyId == 1).OrderByDescending(m => m.Id);
            return Mapper.Map<List<商品DTO>>(libiao.ToList());
        }

        public 商品DTO GetDTO(int? Id)
        {
            var dTO = new 商品DTO();
            if (Id != null)
                dTO =  Mapper.Map<商品DTO>(_iProRep.GetById(Id.Value));
            return dTO;
        }

        public 商品DTO Save(商品DTO dto)
        {
            try
            {
                var model = Mapper.Map<商品信息> (dto);
                if (dto.Id > 0)
                {
                    _iProRep.Update(model);
                }
                else
                {
                    dto.Id= _iProRep.Insert(model);
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

        public 商品DTO Del(商品DTO dto)
        {
            dto.Id = _iProRep.Delete(dto.Id);
            return dto;
        }
    }
}
