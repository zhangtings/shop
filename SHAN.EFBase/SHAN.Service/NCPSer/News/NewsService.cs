using SHAN.Repository;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using SHAN.Entity;
using System;
using AutoMapper.QueryableExtensions;

namespace SHAN.Service
{
    public class NewsService : INewsService
    {
        public IEFRepository<新闻信息 > _iNewsRep { get; set; }
        public IEFRepository<栏目信息> _iColRep { get; set; }


        public List<新闻DTO> List(新闻DTO dto)
        {
            var libiao = new List<新闻DTO>();
            var news = _iNewsRep.Entities;
            var col = _iColRep.Entities;
            //var list= 
            //var kk = xx.OrderByDescending(r=>r.Id).ToList();
            //var kk1 = Mapper.Map<List<新闻DTO>>(kk);
            //try
            //{
            //    var yy = xx.ProjectTo<新闻DTO>();
            //    xx1 = yy.ToList();
            //}
            //catch (Exception e)
            //{

            //    throw e;
            //}
            if (dto.Cid!=null) {
                news = news.Where(r => r.Cid == dto.Cid);
            }

            libiao=news.Join(col, a => a.Cid, b => b.Id, (a, b) => new { a.Id, a.Name, CName = b.Name, a.Cid,a.Addtime })
                .OrderByDescending(m => m.Id).ProjectTo<新闻DTO>().ToList();
            //libiao = news.OrderByDescending(m => m.Id).ProjectTo<新闻DTO>().ToList();
            //var libiao = _iRoomTypeRep.Entities.Where(m => m.CompanyId == 1).OrderByDescending(m => m.Id);
            return libiao;
        }

        public 新闻DTO GetDTO(int? Id)
        {
            var dTO = new 新闻DTO();
            if (Id != null)
                dTO =  Mapper.Map<新闻DTO>(_iNewsRep.GetById(Id.Value));
            return dTO;
        }

        public 新闻DTO Save(新闻DTO dto)
        {
            try
            {
                var model = Mapper.Map<新闻信息> (dto);
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

        public 新闻DTO Del(新闻DTO dto)
        {
            dto.Id = _iNewsRep.Delete(dto.Id);
            return dto;
        }
    }
}
