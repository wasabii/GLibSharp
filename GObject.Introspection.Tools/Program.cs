using System.CommandLine;
using System.CommandLine.Invocation;

namespace GObject.Introspection.Tools
{

    public static class Program
    {

        public static int Main(string[] args)
        {
            var codegen = new Command(
                "codegen",
                "Generates a .NET code binding for a GIR exposed API")
            {
                new Option(
                    new string[] { "--module", "-m" },
                    "Modules to include")
                {
                    Argument = new Argument<string[]>(defaultValue: () => new string[0])
                },
                new Option(
                    new string[] { "--import", "-i" },
                    "GObject repository files to import")
                {
                    Argument = new Argument<string[]>()
                },
                new Option(
                    new string[] { "--export", "-e" },
                    "GObject namespaces to export")
                {
                    Argument = new Argument<string[]>()
                },
                new Option(
                    "--lang",
                    "Language of code to output")
                {
                    Argument = new Argument<string>(defaultValue: () => "C#")
                },
                new Option(
                    "--lang-version",
                    "Version of the language to output")
                {
                    Argument = new Argument<double?>(defaultValue: () => null)
                },
                new Option(
                    new string[] { "--output", "-o" },
                    "Destination to place generated file")
                {
                    Argument = new Argument<string>()
                }
            };

            codegen.Handler = new CodeGen();

            var cmd = new RootCommand() { codegen };
            return cmd.Invoke(args, new SystemConsole());
        }

    }

}
