using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;


public class Token
{
    public string Lexeme { get; private set; }
    public TokenTypes Type { get; private set; }
    public TokenSubtypes Subtype { get; private set; }
    public CodeLocation Location { get; private set; }

    public Token(string lexeme, TokenTypes type, int row, int column)
    {
        Lexeme = lexeme;
        Type = type;
        Location = new CodeLocation(row, column);

        switch (type)
        {
            case TokenTypes.Identifier:
                Subtype = TokenSubtypes.Identifier;
                return;

            case TokenTypes.StringLiteral:
                Subtype = TokenSubtypes.StringLiteral;
                return;

            case TokenTypes.NumericLiteral:
                Subtype = TokenSubtypes.NumericLiteral;
                return;

            case TokenTypes.Comments:
                Subtype = TokenSubtypes.Comments;
                return;

            case TokenTypes.WhiteSpaces:
                Subtype = TokenSubtypes.WhiteSpaces;
                return;

            case TokenTypes.EOF:
                Subtype = TokenSubtypes.EOF;
                return;

            default:
                Subtype = LexicalComponents.SubtypesCorrespondecy[type][lexeme];
                return;
        }

    }

    public override string ToString()
    {
        //return $"Lexeme: {Lexeme}, Type: {Type}, Subtipe: {Subtype}, Row: {Location.Row}, Column: {Location.Column}";
        return Lexeme;
    }
}
