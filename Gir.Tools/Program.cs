using System;
using System.CommandLine;
using System.IO;

namespace Gir.Tools
{

    public static class Program
    {

        public static int Main(string[] args)
        {
            var cmd = new RootCommand()
            {
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
                    Argument = new Argument<double?>()
                },
                new Option(
                    "--reference",
                    "References to include")
                {
                    Argument = new Argument<FileInfo[]>()
                },
                new Option(
                    "--repositories",
                    "References to include")
                {
                    Argument = new Argument<FileInfo[]>()
                }
            };

            switch (args[0])
            {
                case "build":
                    throw new NotImplementedException();
                default:
                    throw new NotImplementedException();
            }

        }

    }

}
