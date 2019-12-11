using Autofac;

using Cogito.Autofac;

namespace GObject.Introspection.CodeGen
{

    /// <summary>
    /// Provides a module for Autofac registration.
    /// </summary>
    public class AssemblyModule : ModuleBase
    {

        protected override void Register(ContainerBuilder builder)
        {
            builder.RegisterFromAttributes(typeof(AssemblyModule).Assembly);
        }

    }

}
