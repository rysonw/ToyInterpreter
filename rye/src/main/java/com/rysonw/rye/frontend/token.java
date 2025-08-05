package com.rysonw.rye.frontend;

public class Token {
    final TokenType type;
    final String lex;
    final int line;

    public Token(TokenType type, String lex, int line) {
        this.type = type;
        this.lex = lex;
        this.line = line;
    }

}