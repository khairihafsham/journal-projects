using System.Collections.Generic;
using System.IO;
using Antlr4.Runtime;

namespace AdvParsing
{
    public class ErrorListener : BaseErrorListener
    {
        public List<string> ErrorMessages { get; set; } = new List<string>();
        public bool HasError => this.ErrorMessages.Count > 0;

        public override void SyntaxError(TextWriter output, IRecognizer recognizer, IToken offendingSymbol, int line,
                                         int charPositionInLIne, string msg, RecognitionException e)
        {
            this.ErrorMessages.Add($"line {line}:{charPositionInLIne} {msg}");
        }
    }
}
