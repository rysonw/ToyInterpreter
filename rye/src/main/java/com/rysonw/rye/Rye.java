package com.rysonwinterpreter.rye;

import java.io.BufferedReader;
import java.io.IOException;
import java.io.InputStreamReader;
import java.nio.charset.Charset;
import java.nio.file.Files;
import java.nio.file.Paths;
import java.util.List;

public class Rye {
  public static void main(String[] args) throws IOException {
    System.out.println("Rye interpreter starting...");

    if (args.length > 1) {
        System.out.println("Usage: rye [script]");
        System.exit(64);
    } 
    else if (args.length == 1) {
        runFile(args[0]);
    } 
  }

  private static void runFile(string filePath) throws IOException {
    // Rye only runs code from files, no CLI
    byte[] bytes = Files.readAllBytes(Paths.get(filePath));
    runLines(bytes);
  }

  private static void runLines(String source) {
    Scanner scanner = new Scanner(source);
    List<Tokens> tokens = scanner.scanTokens();

    for (var token : tokens) {
        System.out.println(token);
    }
  }

  static void error(int line, String errorMessage) {
    report(line, "", errorMessage)
  }

  static void report() {
    System.error.println("[line " + line + "] Error" + where + ": " + message);
  }
}
