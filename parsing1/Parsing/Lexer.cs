using System;
using System.Text;

namespace Parsing
{
    public class Lexer
    {
        public string Input { get; }
        public bool EOF { get; set; } = false;
        protected char Char { get; set; }
        protected int Pointer { get; set; }

        public Lexer(string input)
        {
            this.Input = input;
            this.Char = input[this.Pointer];
        }

        public void Consume()
        {
            this.Pointer++;

            if (this.Pointer >= this.Input.Length)
            {
                this.EOF = true;
            }
            else
            {
                this.Char = this.Input[this.Pointer];
            }
        }

        public void Reset()
        {
            this.Pointer = 0;
            this.Char = this.Input[this.Pointer];
            this.EOF = false;
        }

        public void Match(char x)
        {
            if (this.Char == x)
            {
                this.Consume();
            }
            else
            {
                throw new Exception($"Expecting {x}; got {this.Char}");
            }
        }

        public Token NextToken()
        {
            while (this.EOF == false)
            {
                switch (this.Char)
                {
                    case ' ':
                    case '\t':
                    case '\n':
                    case '\r':
                        this.WS();
                        continue;

                    case '+':
                        this.Consume();
                        return new Token(TokenType.PLUS, "+");

                    case '(':
                        this.Consume();
                        return new Token(TokenType.LBRACKET, "(");

                    case ')':
                        this.Consume();
                        return new Token(TokenType.RBRACKET, ")");

                    default:
                        if (this.IsLETTER())
                        {
                            return this.NAME();
                        }

                        if (this.IsDIGIT())
                        {
                            return this.INT();
                        }

                        throw new Exception($"Invalid character: {this.Char}");
                }
            }

            return null;
        }

        protected bool IsLETTER()
        {
            return this.Char >= 'a'  && this.Char <= 'z' || this.Char >= 'A' && this.Char <= 'Z';
        }

        protected bool IsDIGIT()
        {
            return this.Char >= '0'  && this.Char <= '9';
        }

        protected Token INT()
        {
            StringBuilder buf = new StringBuilder();
            do
            {
                buf.Append(this.Char);
                this.Consume();
            } while (this.IsDIGIT());

            return new Token(TokenType.INT, buf.ToString());
        }

        protected Token NAME()
        {
            StringBuilder buf = new StringBuilder();
            do
            {
                buf.Append(this.Char);
                this.Consume();
            } while (this.IsLETTER());

            return new Token(TokenType.NAME, buf.ToString());
        }

        protected void WS()
        {
            while ( this.Char ==' ' || this.Char == '\t' || this.Char == '\n' || this.Char == '\r' )
            {
                this.Consume();

                if (this.EOF)
                {
                    break;
                }
            }
        }
    }
}
