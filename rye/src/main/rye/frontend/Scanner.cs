using System;
using System.Collections.Generic;
using System.Text;

namespace com.rysonw.rye.frontend
{
    public class Scanner
    {
        private readonly string source;
        private readonly List<Token> tokens = new List<Token>();
        private int start = 0;
        private int current = 0;
        private int line = 1;

        public Scanner(string source)
        {
            this.source = source ?? string.Empty;
        }

        public List<Token> ScanTokens()
        {
            while (!IsAtEnd())
            {
                start = current;
                ScanToken();
            }

            tokens.Add(new Token(TokenType.EOF, string.Empty, line));
            return tokens;
        }

        private void ScanToken()
        {
            char c = Advance();
            if (char.IsWhiteSpace(c))
            {
                if (c == '\n') line++;
                return;
            }

            if (char.IsLetter(c) || c == '_')
            {
                Identifier();
                return;
            }

            if (char.IsDigit(c))
            {
                Number();
                return;
            }

            // For any other single character, treat its lexeme as a token (e.g., punctuation)
            tokens.Add(new Token(TokenType.IDENTIFIER, c.ToString(), line));
        }

        private void Identifier()
        {
            while (!IsAtEnd() && (char.IsLetterOrDigit(Peek()) || Peek() == '_'))
            {
                Advance();
            }
            string text = source.Substring(start, current - start);
            tokens.Add(new Token(TokenType.IDENTIFIER, text, line));
        }

        private void Number()
        {
            while (!IsAtEnd() && char.IsDigit(Peek()))
            {
                Advance();
            }
            // optional fraction part
            if (!IsAtEnd() && Peek() == '.' && char.IsDigit(PeekNext()))
            {
                Advance(); // consume '.'
                while (!IsAtEnd() && char.IsDigit(Peek()))
                {
                    Advance();
                }
            }

            string text = source.Substring(start, current - start);
            tokens.Add(new Token(TokenType.NUMBER, text, line));
        }

        private bool IsAtEnd() => current >= source.Length;
        private char Advance() => source[current++];
        private char Peek() => IsAtEnd() ? '\0' : source[current];
        private char PeekNext() => (current + 1) >= source.Length ? '\0' : source[current + 1];
    }
}
