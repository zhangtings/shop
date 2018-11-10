using SHAN.Repository;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using SHAN.Entity;
using System;

namespace SHAN.Service
{
    public class CategoryService : ICategoryService
    {
        public IEFRepository<商品分类 > _iCatRep { get; set; }



        public List<分类DTO> List(分类DTO dto)
        {
            var libiao = _iCatRep.Entities;
            
            libiao = libiao.OrderByDescending(m => m.Id);
            //var libiao = _iRoomTypeRep.Entities.Where(m => m.CompanyId == 1).OrderByDescending(m => m.Id);
            return Mapper.Map<List<分类DTO>>(libiao.ToList());
        }

        public 分类DTO GetDTO(int? Id)
        {
            var dTO = new 分类DTO();
            if (Id != null)
                dTO =  Mapper.Map<分类DTO>(_iCatRep.GetById(Id.Value));
            return dTO;
        }

        public 分类DTO Save(分类DTO dto)
        {
            try
            {
                var model = Mapper.Map<商品分类> (dto);
                if (dto.Id > 0)
                {
                    _iCatRep.Update(model);
                }
                else
                {
                    dto.Id= _iCatRep.Insert(model);
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

        public 分类DTO Del(分类DTO dto)
        {
            dto.Id = _iCatRep.Delete(dto.Id);
            return dto;
        }
    }
}
