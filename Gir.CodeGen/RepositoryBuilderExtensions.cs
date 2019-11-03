using System;

using Microsoft.CodeAnalysis.Editing;
using Microsoft.Extensions.DependencyInjection;

namespace Gir.CodeGen
{

    /// <summary>
    /// Provides extension methods for usage with the <see cref="RepositoryBuilder"/>.
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
            services.AddTransient<Func<SyntaxGenerator, IRepositoryBuilder>>(p => s => new RepositoryBuilder(s, p.GetServices<ISyntaxNodeGenerator>(), p.GetServices<IProcessor>()));
            services.AddTransient<RepositoryBuilderFactory>();

            // register available node generators
            foreach (var t in typeof(RepositoryBuilder).Assembly.GetTypes())
                if (t.IsClass && !t.IsAbstract && typeof(ISyntaxNodeGenerator).IsAssignableFrom(t))
                    services.AddScoped(typeof(ISyntaxNodeGenerator), t);

            // register available processors
            foreach (var t in typeof(RepositoryBuilder).Assembly.GetTypes())
                if (t.IsClass && !t.IsAbstract && typeof(IProcessor).IsAssignableFrom(t))
                    services.AddScoped(typeof(IProcessor), t);

            return services;
        }

    }

}
