using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;

using GObject.Introspection.Model;

namespace GObject.Introspection.Emit
{

    class MethodMemberEmitter : MemberEmitter
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        public MethodMemberEmitter(DynamicEmitContext context) :
            base(context)
        {

        }

        System.Type[] GetMethodParameterTypes(TypeBuilder type, MethodMember method)
        {
            return new System.Type[0];
        }

        MethodAttributes GetMethodAttributes(TypeBuilder type, MethodMember method)
        {
            var a = MethodAttributes.PrivateScope;

            switch (method.Visibility)
            {
                case Visibility.Public:
                    a |= MethodAttributes.Public;
                    break;
                case Visibility.Private:
                    a |= MethodAttributes.Private;
                    break;
                case Visibility.Internal:
                    a |= MethodAttributes.Family;
                    break;
            }

            if (method.Modifiers.HasFlag(MemberModifier.Static))
                a |= MethodAttributes.Static;

            return a;
        }

        /// <summary>
        /// Emits the dynamic member.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="member"></param>
        /// <returns></returns>
        public override IEnumerable<MemberInfo> EmitDynamicMember(TypeBuilder type, Member member)
        {
            return EmitDynamicMember(type, (MethodMember)member);
        }

        /// <summary>
        /// Emits the dynamic member.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="method"></param>
        /// <returns></returns>
        IEnumerable<MemberInfo> EmitDynamicMember(TypeBuilder type, MethodMember method)
        {
            // handle constructors explicitely
            if (method is ConstructorMember ctor)
            {
                foreach (var i in EmitDynamicConstructorMember(type, ctor))
                    yield return i;

                yield break;
            }

            var returnType = method.ReturnType != null ? Context.ResolveTypeInfo(method.ReturnType.Type) : null;
            var methodBuilder = type.DefineMethod(method.Name, GetMethodAttributes(type, method), returnType, GetMethodParameterTypes(type, method));
            methodBuilder.GetILGenerator().Emit(OpCodes.Nop);
            yield return methodBuilder;
        }

        /// <summary>
        /// Emits a constructor method.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="ctor"></param>
        /// <returns></returns>
        IEnumerable<MemberInfo> EmitDynamicConstructorMember(TypeBuilder type, ConstructorMember ctor)
        {
            var ctorBuilder = type.DefineConstructor(GetMethodAttributes(type, ctor), CallingConventions.HasThis, GetMethodParameterTypes(type, ctor));
            ctorBuilder.GetILGenerator().Emit(OpCodes.Nop);
            yield return ctorBuilder;
        }

    }

}
