using Autofac;
using System.Linq;
using System.Reflection;

namespace MyAPI.Modules
{
    public class MyAPIModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            var core = typeof(MyAPIModule).GetTypeInfo().Assembly;
            builder.RegisterAssemblyTypes(core)
                   .Where(t => t.Name.EndsWith("Service"))
                   .AsImplementedInterfaces()
                   .InstancePerLifetimeScope();
        }
    }
}
