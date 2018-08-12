using SHAN.Repository;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using SHAN.Entity;

namespace SHAN.Service
{
    public class 分类Service : 接口分类Service
    {
        public IEFRepository<分类实体> _分类仓库 { get; set; }

        //public 分类业务fl(IEFRepository<分类实体> _分类仓库)
        //{
        //    _分类仓库 = _分类仓库;
        //}

        public List<分类传输> 分类集合fljh(int tid)
        {
            if (tid < 1) tid = 1;
            var libiao = 分类递归查询(tid);
                //var libiao = _分类仓库.Entities.Where(m => m.Tid == 1 && m.Del == 0).OrderByDescending(m => m.Sort);
                //List<Product> 
                //var linqprdoct = (from ls in _分类仓库.Entities where ls.Is_hot == 1 && ls.Del == 0 orderby ls.Sort, ls.Id select ls).ToList();
                //System.Diagnostics.Debug.WriteLine(((ObjectQuery)libiao).ToTraceString());
                //System.Diagnostics.Debug.WriteLine(((ObjectQuery)linqprdoct).ToTraceString());
            return Mapper.Map<List<分类实体>, List<分类传输>>(libiao.ToList());
        }

        public List<分类实体> 分类递归查询(int tid) {

            var query = _分类仓库.Entities.Where(c => c.Tid == tid);
            var xx = query.ToList().Concat(query.ToList().SelectMany(t => 分类递归查询(t.Id))).ToList();

            return xx; 
        }
    }
}
