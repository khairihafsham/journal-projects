using System;
using Antlr4.Runtime.Tree;

namespace AdvParsing
{
    public class Listener :  PrinterBaseListener
    {
        protected ParseTreeProperty<int> ExprValue = new ParseTreeProperty<int>();

        public int GetAst(IParseTree ctx)
        {
            return this.ExprValue.Get(ctx);
        }

        protected void SetAst(IParseTree ctx, int value)
        {
            this.ExprValue.Put(ctx, value);
        }

        public override void ExitAdd(PrinterParser.AddContext ctx)
        {
            this.SetAst(ctx, this.GetAst(ctx.GetChild(0)) + this.GetAst(ctx.GetChild(2)));
        }

        public override void ExitInt(PrinterParser.IntContext ctx)
        {
            this.SetAst(ctx, Convert.ToInt32(ctx.GetText()));
        }

        public override void ExitPrint(PrinterParser.PrintContext ctx)
        {
            Console.WriteLine(this.GetAst(ctx.expr()));
        }
    }
}
