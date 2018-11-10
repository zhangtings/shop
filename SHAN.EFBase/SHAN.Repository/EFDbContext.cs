using System;
using System.Collections.Generic;
using System.Linq;
using SHAN.Entity;
using System.Threading.Tasks;
using System.Data.Entity;
using MySql.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Reflection;

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

            Assembly.Load("SHAN.Entity").GetTypes()
                .Where(t => t.BaseType == typeof(BaseEntity)).ToList()
                //.ForEach(r=> Console.WriteLine());
            
                .ForEach(r=> modelBuilder.RegisterEntityType(r));
            //modelBuilder.RegisterEntityType(typeof(BaseEntity));
            base.OnModelCreating(modelBuilder);//RegisterEntityType

            //modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }


        //public DbSet<房间信息> 房间信息 { get; set; }
        //public DbSet<房间类型> 房间类型 { get; set; }
        //public DbSet<客人信息> 客人信息 { get; set; }
        //public DbSet<客人类型> 客人类型 { get; set; }
        //public DbSet<入住> 入住 { get; set; }
        //public DbSet<预定> 预定 { get; set; }
        //public DbSet<账目信息> 账目信息 { get; set; }
        //public DbSet<开房记录> 开房记录 { get; set; }


    }
}
