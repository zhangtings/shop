using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Linq;
using System.Reflection;
using System.Web;

namespace SHAN.Web
{
    public static class Class1
    {
        /// <summary>
        /// 注册某个程序集中所有<typeparamref name="TEntityBase"/>的非抽象实体子类
        /// </summary>
        /// <typeparam name="TEntityBase">实体基类</typeparam>
        /// <param name="modelBuilder"></param>
        /// <param name="assembly">注册程序集</param>
        public static void RegisterEntitiesFromAssembly<TEntityBase>(this DbModelBuilder modelBuilder, Assembly assembly)
            where TEntityBase : class
        {
            //modelBuilder.RegisterEntitiesFromAssembly(assembly, r => !r.IsAbstract && r.IsClass && r.IsSubclassOf(TEntityBase.GetType()));
            modelBuilder.RegisterEntitiesFromAssembly(assembly, r => !r.IsAbstract && r.IsClass );//&& r.IsSubclassOf(TEntityBase.GetType())
        }

        /// <summary>
        /// 注册某个程序集中所有<typeparamref name="TEntityBase"/>的非抽象实体子类
        /// </summary>
        /// <typeparam name="TEntityBase">实体基类</typeparam>
        /// <param name="modelBuilder"></param>
        /// <param name="assembly">注册程序集</param>
        /// <param name="assembly">注册程序集</param>
        public static void RegisterEntitiesFromAssembly(this DbModelBuilder modelBuilder, Assembly assembly, Func<Type, bool> entityTypePredicate)
        {
            if (assembly == null)
                throw new ArgumentNullException(nameof(assembly));

            //反射得到DbModelBuilder的Entity方法
            var entityMethod = modelBuilder.GetType().GetMethod("Entity");

            //反射得到ConfigurationRegistrar的Add<TEntityType>方法
            var addMethod = typeof(ConfigurationRegistrar)
                   .GetMethods()
                   .Single(m =>
                     m.Name == "Add"
                     && m.GetGenericArguments().Any(a => a.Name == "TEntityType"));
            //扫描所有fluent api配置类,要求父类型必须是EntityTypeConfiguration<TEntityType>
            var configTypes = assembly
                               .GetTypes()
                               .Where(t =>
                                     !t.IsAbstract && t.BaseType != null && t.IsClass
                                     && t.BaseType.IsGenericType
                                     && t.BaseType.GetGenericTypeDefinition() == typeof(EntityTypeConfiguration<>)
                                     )
                               .ToList();

            HashSet<Type> registedTypes = new HashSet<Type>();

            //存在fluent api配置的类,必须在Entity方法之前调用
            configTypes.ForEach(mappingType =>
            {
                var entityType = mappingType.BaseType.GetGenericArguments().Single();
                if (!entityTypePredicate(entityType))
                    return;
                var map = Activator.CreateInstance(mappingType);
                //反射调用ConfigurationRegistrar的Add方法注册fluent api配置,该方法会同时注册实体
                addMethod.MakeGenericMethod(entityType)
                     .Invoke(modelBuilder.Configurations, new object[] { map });

                registedTypes.Add(entityType);
            });

            //反射调用Entity方法 注册实体
            assembly.GetTypes()
                //.GetTypesSafely()
                .Where(entityTypePredicate)
                .ForEach(r =>
                {
                    entityMethod.MakeGenericMethod(r).Invoke(modelBuilder, new object[0]);
                });
        }
    }
}