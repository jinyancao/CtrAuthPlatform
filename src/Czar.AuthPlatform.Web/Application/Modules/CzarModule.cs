using Autofac;
using Czar.AuthPlatform.Web.Application.Repository;
using Czar.AuthPlatform.Web.Application.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Czar.AuthPlatform.Web.Application.Modules
{
    /// <summary>
    /// 金焰的世界
    /// 2018-12-18
    /// 使用程序集注册
    /// </summary>
    public class CzarModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            //注册Repository程序集
            builder.RegisterAssemblyTypes(typeof(CzarUsersRepository).GetTypeInfo().Assembly).AsImplementedInterfaces().InstancePerLifetimeScope();
            //注册Services程序集
            builder.RegisterAssemblyTypes(typeof(CzarUsersServices).GetTypeInfo().Assembly).AsImplementedInterfaces().InstancePerLifetimeScope();
        }
    }
}
