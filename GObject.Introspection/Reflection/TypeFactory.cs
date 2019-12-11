using System;
using System.Dynamic;
using System.Linq.Expressions;

namespace GObject.Introspection.Reflection
{

    /// <summary>
    /// Provides methods to create new instances of GObject types.
    /// </summary>
    public class TypeFactory : IDynamicMetaObjectProvider
    {

        public DynamicMetaObject GetMetaObject(Expression parameter)
        {
            throw new NotImplementedException();
        }

    }

}
