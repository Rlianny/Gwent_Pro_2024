using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Token
{
    public string Lexeme { get; private set; }
    public TokenTypes Type { get; private set; }
    public int Row { get; private set; }
    public int Column { get; private set; }
    public TokenSubtypes Subtype { get; private set; }

    public Token(string lexeme, TokenTypes type, int row, int column)
    {
        Lexeme = lexeme;
        Type = type;
        Row = row;
        Column = column;

        switch(type)
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

            default:
            Subtype = LexicalComponents.SubtypesCorrespondecy[type][lexeme];
            return;
        }

    }
}