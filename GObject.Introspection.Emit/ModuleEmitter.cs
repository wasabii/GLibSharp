using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

using GObject.Introspection.CodeGen.Model;

namespace GObject.Introspection.Emit
{

    /// <summary>
    /// Provides support for emitting an <see cref="Dynamic.Module"/> as a dynamic assembly.
    /// </summary>
    public class ModuleEmitter
    {

        readonly ModuleLibrary library;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="library"></param>
        public ModuleEmitter(ModuleLibrary library)
        {
            this.library = library ?? throw new ArgumentNullException(nameof(library));
        }

        /// <summary>
        /// Emits the introspected module.
        /// </summary>
        /// <param name="module"></param>
        /// <param name="references"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public AssemblyBuilder Emit(Dynamic.Module module, IEnumerable<Assembly> references, string name = null)
        {
            if (module is null)
                throw new ArgumentNullException(nameof(module));
            if (references is null)
                throw new ArgumentNullException(nameof(references));

            // default name to something that can be looked up easily
            if (name == null)
                name = "GInterop." + module.Name;

            // emit types into new assembly
            var b = AssemblyBuilder.DefineDynamicAssembly(new AssemblyName(name), AssemblyBuilderAccess.Run);
#if NET472
            Emit(module, b, references, name, name + ".dll");
#else
            Emit(module, b, references, name);
#endif
            return b;
        }

#if NET472

        /// <summary>
        /// Emits the the introspected module into the specified assembly builder.
        /// </summary>
        /// <param name="module"></param>
        /// <param name="assembly"></param>
        /// <param name="references"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public ModuleBuilder Emit(Dynamic.Module module, AssemblyBuilder assembly, IEnumerable<Assembly> references, string name, string fileName)
        {
            if (assembly is null)
                throw new ArgumentNullException(nameof(assembly));
            if (references is null)
                throw new ArgumentNullException(nameof(references));

            // default to the assembly name
            if (name is null)
                name = assembly.GetName().Name;

            var builder = assembly.DefineDynamicModule(name, fileName, true);
            Emit(module, builder, references);
            return builder;
        }

#endif

        /// <summary>
        /// Emits the the introspected module into the specified assembly builder.
        /// </summary>
        /// <param name="module"></param>
        /// <param name="assembly"></param>
        /// <param name="references"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public ModuleBuilder Emit(Dynamic.Module module, AssemblyBuilder assembly, IEnumerable<Assembly> references, string name)
        {
            if (assembly is null)
                throw new ArgumentNullException(nameof(assembly));
            if (references is null)
                throw new ArgumentNullException(nameof(references));

            // default to the assembly name
            if (name is null)
                name = assembly.GetName().Name;

            var builder = assembly.DefineDynamicModule(name);
            Emit(module, builder, references);
            return builder;
        }

        /// <summary>
        /// Emits the types of the module into the specified moduble builder.
        /// </summary>
        /// <param name="module"></param>
        /// <param name="builder"></param>
        /// <param name="references"></param>
        public void Emit(Dynamic.Module module, ModuleBuilder builder, IEnumerable<Assembly> references)
        {
            if (module is null)
                throw new ArgumentNullException(nameof(module));
            if (builder is null)
                throw new ArgumentNullException(nameof(builder));
            if (references is null)
                throw new ArgumentNullException(nameof(references));

            var p = new TypeInfoResolver(new ModuleTypeInfoSource(builder), new AssemblyTypeInfoSource(references));
            var c = new Context(builder, p);
            var l = module.Types.SelectMany(i => c.EmitDynamicType(i)).ToList();
            var t = l.Select(i => i.FinalTypeInfo.GetTypeInfo()).ToList();
        }

    }

}
