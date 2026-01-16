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

            tokens.Add(new Token(TokenType.EOF, string.Empty, null, line));
            return tokens;
        }

        private void ScanToken()
        {
            char c = Advance();

            switch (c)
            {
                // Single
                case '(': AddToken(TokenType.LEFT_PAREN); break;
                case ')': AddToken(TokenType.RIGHT_PAREN); break;
                case '{': AddToken(TokenType.LEFT_BRACE); break;
                case '}': AddToken(TokenType.RIGHT_BRACE); break;
                case ',': AddToken(TokenType.COMMA); break;
                case '.': AddToken(TokenType.DOT); break;
                case '-': AddToken(TokenType.MINUS); break;
                case '+': AddToken(TokenType.PLUS); break;
                case ';': AddToken(TokenType.SEMICOLON); break;
                case '*': AddToken(TokenType.STAR); break;

                // Double
                case '!': AddToken(match('=') ? TokenType.BANG_EQUAL : TokenType.BANG); break;
                case '=': AddToken(match('=') ? TokenType.EQUAL_EQUAL : TokenType.EQUAL); break;
                case '<': AddToken(match('=') ? TokenType.LESS_EQUAL : TokenType.LESS); break;
                case '>': AddToken(match('=') ? TokenType.GREATER_EQUAL : TokenType.GREATER); break;
                case '/':
                    if (match('/'))
                    {
                        // Find the position for end of comment
                        while (Peek() != '\n' && !IsAtEnd())
                        {
                            Advance();
                        }
                    }
                    else
                    {
                        AddToken(TokenType.SLASH);
                    }
                    break;

                // Whitespace (IGNORE)
                case ' ':
                case '\r':
                case '\t':
                    break;
                case '\n':
                    line++;
                    break;

                // Literals
                case '"': CheckString(); break;
                
                default:

                    if (IsDigit(c))
                    {
                        Number();
                    }
                    else
                    {
                        Rye.Error(line, "Unexpected Character.");
                    }

                    Rye.Error(line, "Unexpected error reading token...");
                    break;
            }
        }

        private bool match(char expected)
        {
            if (IsAtEnd()) { return false; }
            if (source[current] != expected) { return false; }
            current++;
            return true;
        }

        private void AddToken(TokenType type) {
            AddToken(type, null);
        }

        private void AddToken(TokenType type, Object literal) {
            Console.WriteLine($"Start: {start}; Current: {current}");
            String text = source.Substring(start, current - start);
            tokens.Add(new Token(type, text, literal, line));
        }

        private void CheckString()
        {
            while (Peek() != '"' && !IsAtEnd())
            {
                if (Peek() == '\n') line++;
                Advance();
            }

            if (IsAtEnd())
            {
                Rye.Error(line, "Unterminated String");
                return;
            }

            Advance();

            int length = current - start - 2;
            string value = length > 0 ? source.Substring(start + 1, length) : string.Empty;
            AddToken(TokenType.STRING, value);
        }

        private bool IsDigit(char c) {
            return c >= '0' && c <= '9';
        }

        private void Number()
        {
            while (IsDigit(Peek())) Advance();

            if (Peek() == '.' && IsDigit(PeekNext()))
            {
                Advance(); // consume '.'
                while (IsDigit(Peek())) Advance();
            }

            string text = source.Substring(start, current - start);
            if (double.TryParse(text, System.Globalization.NumberStyles.Number,
                                System.Globalization.CultureInfo.InvariantCulture,
                                out double value))
            {
                AddToken(TokenType.NUMBER, value);
            }
            else
            {
                Rye.Error(line, $"Invalid number literal: {text}");
            }
        }

        private bool IsAtEnd() => current >= source.Length;
        private char Advance() => source[current++];
        private char Peek() => IsAtEnd() ? '\0' : source[current]; 
        private char PeekNext() => (current + 1) >= source.Length ? '\0' : source[current + 1];
    }
}
