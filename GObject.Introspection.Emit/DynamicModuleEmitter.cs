using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

using GObject.Introspection.Model;

namespace GObject.Introspection.Emit
{

    /// <summary>
    /// Provides support for emitting an <see cref="Model.Module"/> as a dynamic assembly.
    /// </summary>
    public class DynamicModuleEmitter
    {

        readonly ModuleLibrary library;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="library"></param>
        public DynamicModuleEmitter(ModuleLibrary library)
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
        public AssemblyBuilder Emit(Model.Module module, IEnumerable<Assembly> references, string name = null)
        {
            if (module is null)
                throw new ArgumentNullException(nameof(module));
            if (references is null)
                throw new ArgumentNullException(nameof(references));

            // default name to something that can be looked up easily
            if (name == null)
                name = "GInterop." + module.Name;

            // emit types into new assembly
            var b = AssemblyBuilder.DefineDynamicAssembly(new AssemblyName(name), AssemblyBuilderAccess.RunAndSave);
            Emit(module, b, references, name, name + ".dll");
            return b;
        }

        /// <summary>
        /// Emits the the introspected module into the specified assembly builder.
        /// </summary>
        /// <param name="module"></param>
        /// <param name="assembly"></param>
        /// <param name="references"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public ModuleBuilder Emit(Model.Module module, AssemblyBuilder assembly, IEnumerable<Assembly> references, string name, string fileName)
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

        /// <summary>
        /// Emits the types of the module into the specified moduble builder.
        /// </summary>
        /// <param name="module"></param>
        /// <param name="builder"></param>
        /// <param name="references"></param>
        public void Emit(Model.Module module, ModuleBuilder builder, IEnumerable<Assembly> references)
        {
            if (module is null)
                throw new ArgumentNullException(nameof(module));
            if (builder is null)
                throw new ArgumentNullException(nameof(builder));
            if (references is null)
                throw new ArgumentNullException(nameof(references));

            var p = new TypeInfoResolver(new DynamicTypeInfoSource(builder), new AssemblyTypeInfoSource(references));
            var c = new DynamicEmitContext(builder, p);
            var l = module.Types.SelectMany(i => c.EmitDynamicType(i)).ToList();
            var t = l.Select(i => i.FinalTypeInfo.GetTypeInfo()).ToList();
        }

    }

}
