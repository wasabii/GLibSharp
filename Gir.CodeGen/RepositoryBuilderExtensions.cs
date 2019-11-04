using System;

using Microsoft.CodeAnalysis.Editing;
using Microsoft.Extensions.DependencyInjection;

namespace Gir.CodeGen
{

    /// <summary>
    /// Provides extension methods for usage with the <see cref="SyntaxBuilder"/>.
    /// </summary>
    public static class RepositoryBuilderExtensions
    {

        /// <summary>
        /// Adds the GirSharp CodeGen dependencies to the container.
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddGirCodeGen(this IServiceCollection services)
        {
            services.AddTransient<Func<SyntaxGenerator, ISyntaxBuilder>>(p => s => new SyntaxBuilder(s, p.GetServices<ISyntaxNodeBuilder>()));
            services.AddTransient<RepositoryBuilderFactory>();

            // register available builders
            foreach (var t in typeof(SyntaxBuilder).Assembly.GetTypes())
                if (t.IsClass && !t.IsAbstract && typeof(ISyntaxNodeBuilder).IsAssignableFrom(t))
                    services.AddScoped(typeof(ISyntaxNodeBuilder), t);

            return services;
        }

    }

}
