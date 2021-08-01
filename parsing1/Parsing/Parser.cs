using System;

namespace Parsing
{
    public class Parser
    {
        protected Lexer Lexer { get; set; }

        /// <summary>
        /// The size will be controlled by N indirectly
        /// </summary>
        protected Token[] LookAhead { get; set; }

        /// <summary>
        /// The N in LL(N)
        /// </summary>
        protected int N { get; set; }

        /// <summary>
        /// A circular index. It counts how many Tokens left to fill in the LookAhead List
        /// </summary>
        protected int Pointer { get; set; }

        public Parser(Lexer lexer, int n)
        {
            this.Lexer = lexer;
            this.N = n;
            this.LookAhead = new Token[n];

            for (int i = 0; i < this.N; i++)
            {
                this.Consume();
            }
        }

        public void Match(string text)
        {
            if (this.LT(1).Type == TokenType.NAME && this.LT(1).Text == text)
            {
                this.Consume();
            }
            else
            {
                throw new Exception($"Expecting {text}; got {this.LA(1)}");
            }
        }

        public string Match(TokenType type)
        {
            Token token = this.LT(1);
            if (token.Type == type)
            {
                this.Consume();

                return token.Text;
            }
            else
            {
                throw new Exception($"Expecting {type}; got {this.LA(1)}");
            }
        }

        public void Consume()
        {
            this.LookAhead[this.Pointer] = this.Lexer.NextToken();
            this.Pointer = (this.Pointer + 1) % this.N;
        }

        public Token LT(int i)
        {
            return this.LookAhead[(int) ((this.Pointer + i -1) % this.N)];
        }

        public TokenType LA(int i)
        {
            return this.LT(i).Type;
        }

        public void Eval()
        {
            while (this.LT(1) != null)
            {
                this.Print();
            }
        }

        public void Print()
        {
            this.Match("print");
            this.Match(TokenType.LBRACKET);
            int value = this.Expr();
            this.Match(TokenType.RBRACKET);

            Console.WriteLine(value);
        }

        public int Expr()
        {
            if (this.LA(1) == TokenType.INT && this.LA(2) == TokenType.PLUS)
            {
                string leftText = this.Match(TokenType.INT);
                this.Match(TokenType.PLUS);
                string rightText = this.Match(TokenType.INT);

                return Convert.ToInt32(leftText) + Convert.ToInt32(rightText);
            }

            if (this.LA(1) == TokenType.INT)
            {
                int value = Convert.ToInt32(this.LT(1).Text);
                this.Match(TokenType.INT);

                return value;
            }

            throw new Exception("Unknown expression");
        }
    }
}
