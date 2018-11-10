using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Data.Common;
using SHAN.Entity;

namespace SHAN.Repository
{
    public interface INCPRepository<TEntity> :IEFRepository<TEntity> where TEntity : class
    {

    }
}
