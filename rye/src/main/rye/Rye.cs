using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using com.rysonw.rye.frontend;

namespace com.rysonw.rye
{
    class Rye
    {
        static int Main(string[] args)
        {
            Console.WriteLine("Rye interpreter starting...");

            if (args.Length > 1)
            {
                Console.WriteLine("Usage: rye [script]");
                return 64;
            }
            else if (args.Length == 1)
            {
                try
                {
                    RunFile(args[0]);
                }
                catch (IOException ex)
                {
                    Console.Error.WriteLine(ex.Message);
                    return 1;
                }
            }
            else
            {
                Console.WriteLine("No script provided. Exiting.");
            }

            return 0;
        }

        private static void RunFile(string filePath)
        {
            var bytes = File.ReadAllBytes(filePath);
            var source = Encoding.UTF8.GetString(bytes);
            RunLines(source);
        }

        private static void RunLines(string source)
        {
            var scanner = new Scanner(source);
            List<Token> tokens = scanner.ScanTokens();

            foreach (var token in tokens)
            {
                Console.WriteLine(token);
            }
        }

        public static void Error(int line, string message)
        {
            Console.Error.WriteLine($"[line {line}] Error: {message}");
        }
    }
}
