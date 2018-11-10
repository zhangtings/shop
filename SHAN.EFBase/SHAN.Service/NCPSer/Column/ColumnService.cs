using SHAN.Repository;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using SHAN.Entity;
using System;

namespace SHAN.Service
{
    public class ColumnService : IColumnService
    {
        public IEFRepository<栏目信息 > _iColRep { get; set; }



        public List<栏目DTO> List(栏目DTO dto)
        {
            var libiao = _iColRep.Entities;
            if (!string.IsNullOrEmpty(dto.Name))
            {
                libiao = libiao.Where(r => r.Name == dto.Name|| _iColRep.Entities.Where(a => a.Name == dto.Name ).Select(a=>a.Id).Contains(r.Cid.Value));
            }
            libiao = libiao.OrderByDescending(m => m.Id);
            //var libiao = _iRoomTypeRep.Entities.Where(m => m.CompanyId == 1).OrderByDescending(m => m.Id);
            return Mapper.Map<List<栏目DTO>>(libiao.ToList());
        }

        public 栏目DTO GetDTO(int? Id)
        {
            var dTO = new 栏目DTO();
            if (Id != null)
                dTO =  Mapper.Map<栏目DTO>(_iColRep.GetById(Id.Value));
            return dTO;
        }

        public 栏目DTO Save(栏目DTO dto)
        {
            try
            {
                var model = Mapper.Map<栏目信息> (dto);
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

        public 栏目DTO Del(栏目DTO dto)
        {
            dto.Id = _iColRep.Delete(dto.Id);
            return dto;
        }
    }
}
