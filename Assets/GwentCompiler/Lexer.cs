using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class Lexer
{
    private int row;
    private List<Token> tokens = new();
    public List<Token> Tokenize(string input)
    {
        string[] lines = input.Split('\n', StringSplitOptions.RemoveEmptyEntries);

        foreach (string line in lines)
        {
            row++;
            LineTokenizer(line);
        }
        row = 0;
        return tokens;
    }
    private void LineTokenizer(string input)
    {
        int index = 1;
        while (index < input.Length)
        {
            bool Match = false;
            foreach (var token in LexicalComponents.tokenRegexPatterns)
            {
                var regex = token.Value;
                var match = regex.Match(input.Substring(index));

                if (match.Success)
                {
                    if (token.Key == TokenTypes.WhiteSpaces)
                    {
                        index += match.Length;
                        Match = match.Success;
                        break;
                    }
                    tokens.Add(new Token(match.Groups[0].Value, token.Key, row, index));
                    Match = match.Success;
                    index += match.Length;
                    break;
                }
            }
            if (!Match)
            {
                throw new Exception($"Unexpected symbol at row {row} and column {index}");
            }
        }
    }
}