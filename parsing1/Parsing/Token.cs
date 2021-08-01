namespace Parsing
{
    public enum TokenType
    {
        EOF = 1,
        NAME = 2,
        PLUS = 3,
        LBRACKET = 4,
        RBRACKET = 5,
        INT = 6
    }

    public class Token
    {
        public TokenType Type { get; set; }
        public string Text { get; set; }

        public Token(TokenType type, string text)
        {
            this.Type = type;
            this.Text = text;
        }

        public override string ToString()
        {
            return $"<{this.Text}, {this.Type}>";
        }
    }
}
