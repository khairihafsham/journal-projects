using System;
using System.IO;
using Antlr4.Runtime;
using Antlr4.Runtime.Tree;

namespace AdvParsing
{
    class Program
    {
        static void Main(string[] args)
        {
            string path = Path.GetFullPath(args[0]);
            string text = System.IO.File.ReadAllText(path);

            AntlrInputStream inputStream = new AntlrInputStream(text);
            PrinterLexer lexer = new PrinterLexer(inputStream);
            CommonTokenStream commonTokenStream = new CommonTokenStream(lexer);
            PrinterParser parser = new PrinterParser(commonTokenStream);

            ErrorListener errListener = new ErrorListener();
            parser.RemoveErrorListeners();
            parser.AddErrorListener(errListener);

            parser.BuildParseTree = true;
            IParseTree tree = parser.stat();

            if (errListener.HasError)
            {
                foreach (string msg in errListener.ErrorMessages)
                {
                    Console.WriteLine(msg);
                }

                return;
            }

            ParseTreeWalker walker = new ParseTreeWalker();
            Listener listener = new Listener();
            walker.Walk(listener, tree);
        }
    }
}
