package com.rysonw.rye.frontend;

public class Token {
    // Class fields, constructors, and methods go here
    final TokenType type;
    final String lex;
    final int line;

    // Example constructor
    public Token(TokenType type, String lex, int line) {
        this.type = type;
        this.lex = lex;
        this.line = line;
    }

    // Example getter
    public String getType() {
        return type;
    }

    // Example setter
    public void setType(String type) {
        this.type = type;
    }
}