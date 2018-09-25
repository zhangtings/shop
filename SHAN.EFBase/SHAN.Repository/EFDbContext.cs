using System;
using System.Collections.Generic;
using System.Linq;
using SHAN.Entity;
using System.Threading.Tasks;
using System.Data.Entity;
using MySql.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Reflection;
using System.Data.Entity.ModelConfiguration;

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

            modelBuilder.RegisterEntityType(typeof(Product));
            //modelBuilder;
            //modelBuilder.Entity<BaseEntity>;
            //base.OnModelCreating(modelBuilder);//RegisterEntityType

            var typesToRegister = Assembly.Load("SHAN.Entity").GetTypes()
                .Where(type => type.BaseType == typeof(BaseEntity));
                //.Where(type => type.BaseType != null && type.BaseType.BaseType != null && type.BaseType.BaseType.GetGenericTypeDefinition() == typeof(EntityTypeConfiguration<>));
            foreach (var type in typesToRegister)
            {
                modelBuilder.RegisterEntityType(type);
                //dynamic configurationInstance = Activator.CreateInstance(type);
                //modelBuilder.Configurations.Add(configurationInstance);
            }
            base.OnModelCreating(modelBuilder);

            //modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }

        
    }
}
