using System;
using System.Collections.Generic;
using SHAN.Entity;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using MySql.Data.Entity;

namespace SHAN.Web
{
    [DbConfigurationType(typeof(MySqlEFConfiguration))]
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
                //base.OnModelCreating(modelBuilder);
                //modelBuilder.RegisterEntitiesFromAssembly(this.GetType().Assembly);
        }

        //public DbSet<UAddress> address { get; set; }
        //public DbSet<admin_app> admin_app { get; set; }
        //public DbSet<adminuser> adminuser { get; set; }
        //public DbSet<attribute> attribute { get; set; }
        //public DbSet<brand> brand { get; set; }
        //public DbSet<Category> category { get; set; }
        //public DbSet<china_city> china_city { get; set; }
        //public DbSet<course> course { get; set; }
        //public DbSet<fankui> fankui { get; set; }
        //public DbSet<group_joins> group_joins { get; set; }
        //public DbSet<guanggao> guanggao { get; set; }
        //public DbSet<guige> guige { get; set; }
        //public DbSet<hot> hot { get; set; }
        //public DbSet<indeximg> indeximg { get; set; }
        //public DbSet<news> news { get; set; }
        //public DbSet<news_cat> news_cat { get; set; }
        //public DbSet<Order> order { get; set; }
        //public DbSet<OrderProduct> order_product { get; set; }
        //public DbSet<post> post { get; set; }
        public DbSet<Product> product { get; set; }
        //public DbSet<ProductDP> product_dp { get; set; }
        //public DbSet<ProductSC> product_sc { get; set; }
        //public DbSet<program> program { get; set; }
        //public DbSet<Sccat> sccat { get; set; }
        //public DbSet<search_record> search_record { get; set; }
        //public DbSet<shangchang> shangchang { get; set; }
        //public DbSet<shangchang_dp> shangchang_dp { get; set; }
        //public DbSet<shangchang_sc> shangchang_sc { get; set; }
        //public DbSet<ShoppingChar> shopping_char { get; set; }
        //public DbSet<student_style> student_style { get; set; }
        //public DbSet<User> user { get; set; }
        //public DbSet<user_course> user_course { get; set; }
        //public DbSet<UserVoucher> user_voucher { get; set; }
        //public DbSet<Voucher> voucher { get; set; }
    }
}
