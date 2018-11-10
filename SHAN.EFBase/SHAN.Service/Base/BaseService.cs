using SHAN.Repository;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using SHAN.Entity;
using System;

namespace SHAN.Service
{
    public class HotelUserService<T,T1> : IBaseService<T,T1> where T1:BaseEntity
    {
        public IEFRepository<T1> _iRep { get; set; }

        public List<T> List(T dto)
        {
            var libiao = _iRep.Entities.OrderByDescending(m => m.Id);
            return Mapper.Map<List<T1>, List<T>>(libiao.ToList());
        }

        public T GetDTO(int? Id)
        {
            var dTO = new T();
            if (Id != null)
                dTO =  Mapper.Map<T>(_iRep.GetById(Id.Value));
            return dTO;
        }

        public T Save(T dto)
        {
            try
            {
                var model = Mapper.Map<T1> (dto);
                if (dto.Id > 0)
                {
                    _iRep.Update(model);
                }
                else
                {
                    dto.Id=_iRep.Insert(model);
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

        public T Del(T dto)
        {
            _iRep.Delete(Mapper.Map<T1>(dto));
            return dto;
        }
    }
}
