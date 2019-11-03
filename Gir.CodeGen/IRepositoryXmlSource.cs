using System.Xml.Linq;

namespace Gir.CodeGen
{

    /// <summary>
    /// Provides repository XML files.
    /// </summary>
    public interface IRepositoryXmlSource
    {

        /// <summary>
        /// Retrieves the element associated with the namespace at the specified version.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="version"></param>
        /// <returns></returns>
        XElement? ResolveNamespace(string name, string version);

    }

}
