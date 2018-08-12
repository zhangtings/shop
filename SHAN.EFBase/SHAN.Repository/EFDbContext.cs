using System;
using System.Collections.Generic;
using System.Linq;
using SHAN.Entity;
using System.Threading.Tasks;
using System.Data.Entity;
using MySql.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace SHAN.Repository
{
    //[DbConfigurationType(typeof(MySqlEFConfiguration))]
    public class EFDbContext : DbContext
    {
        public EFDbContext()
            : base("name=MySqlstring")
        {
        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            Database.SetInitializer<EFDbContext>(null);
            Database.Log = log => System.Diagnostics.Debug.WriteLine(log);

            //modelBuilder.RegisterEntityType(typeof(BaseEntity));
            //base.OnModelCreating(modelBuilder);//RegisterEntityType
            
            //modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }


        public DbSet<Product> product { get; set; }
        public DbSet<分类实体> 分类 { get; set; }
        
    }
}
