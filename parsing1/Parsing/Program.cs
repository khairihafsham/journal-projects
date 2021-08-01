using System;
using System.IO;

namespace Parsing
{
    class Program
    {
        static void Main(string[] args)
        {
            string path = Path.GetFullPath(args[0]);
            string text = System.IO.File.ReadAllText(path);
            Lexer lexer = new Lexer(text);

            if (args.Length > 1 && args[1] == "-tokens")
            {
                PrintToken(args, lexer);
            }
            else
            {
                Parser parser = new Parser(lexer, 2);
                parser.Eval();
            }
        }

        public static void PrintToken(string[] args, Lexer lexer)
        {
            Token token = lexer.NextToken();
            while (token != null)
            {
                Console.WriteLine(token);
                token = lexer.NextToken();
            }

            lexer.Reset();
        }
    }
}
