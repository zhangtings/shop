using System.Linq;
using System.Web.Mvc;
using Autofac;
using Autofac.Integration.Mvc;
using System.Reflection;
using SHAN.Repository;

namespace SHAN.Web
{
    public class AutofacConfig
    {
        public static void Reg()
        {
            var builder = new ContainerBuilder();
            
            Assembly servicesAss = Assembly.Load("SHAN.Service");
            //Type[] sty=servicesAss.GetTypes();
            builder.RegisterAssemblyTypes(servicesAss)
                .Where(t => t.Name.EndsWith("Service"))
                .AsImplementedInterfaces().InstancePerLifetimeScope().PropertiesAutowired();

            Assembly repositoryAss = Assembly.Load("SHAN.Repository");
            builder.RegisterAssemblyTypes(repositoryAss)
                .Where(t => t.Name.EndsWith("Repository"))
                .AsImplementedInterfaces().InstancePerLifetimeScope().PropertiesAutowired();
            builder.RegisterGeneric(typeof(BaseEFRepository<>)).As(typeof(IEFRepository<>))
                .InstancePerDependency().InstancePerLifetimeScope();

            builder.Register(context=> new EFDbContext()).InstancePerDependency().SingleInstance();

            builder.RegisterControllers(Assembly.GetExecutingAssembly()).PropertiesAutowired();

            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }
    }
}