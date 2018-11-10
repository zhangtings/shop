using SHAN.Repository;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using SHAN.Entity;
using System;

namespace SHAN.Service
{
    public class BusineService : IBusineService
    {
        public IEFRepository<商家信息 > _iNewsRep { get; set; }



        public List<商家DTO> List(商家DTO dto)
        {
            var libiao = _iNewsRep.Entities;
            if (!string.IsNullOrEmpty(dto.Tags))
            {
                libiao = libiao.Where(r => r.Tags.Contains(dto.Tags));
            }
            libiao = libiao.OrderByDescending(m => m.Id);
            return Mapper.Map<List<商家DTO>>(libiao.ToList());
        }

        public 商家DTO GetDTO(int? Id)
        {
            var dTO = new 商家DTO();
            if (Id != null)
                dTO =  Mapper.Map<商家DTO>(_iNewsRep.GetById(Id.Value));
            return dTO;
        }

        public 商家DTO Save(商家DTO dto)
        {
            try
            {
                var model = Mapper.Map<商家信息> (dto);
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

        public 商家DTO Del(商家DTO dto)
        {
            dto.Id = _iNewsRep.Delete(dto.Id);
            return dto;
        }
    }
}
