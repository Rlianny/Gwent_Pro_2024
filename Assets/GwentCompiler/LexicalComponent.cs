using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;

public enum TokenTypes
{
    Identifier,
    Keyword,
    Separators,
    ArithmeticOperator, ComparisonOperator, LogicalOperator, AssignmentOperator, MembershipOperator, IncrementDecrementOPerator, StringOperator,
    NumericLiteral, StringLiteral, BooleanLiteral,
    Comments,
    WhiteSpaces,
}

public enum TokenSubtypes
{
    Comma, OpenParenthesis, CloseParenthesis, OpenBracket, CloseBracket, OpenBrace, CloseBrace, Colon, Semicolon, Lambda, Dot,
    effect, card, Name, While, For, String, Number, Bool, Params, Action, Type, Faction, Power, Range, OnActivation, Effect, Selector, Source, Single, Predicate, PostAction,
    True, False, NumericLiteral, StringLiteral, Identifier, Comments, WhiteSpaces,
    Addition, Subtraction, Multiplication, Division, Potentiation, Equality, AND, OR, GreaterThanOrEqual, LessThanOrEqual, GreaterThan, LessThan, In, PostIncrement, PostDecrement, StringConcatenation, StringConcatenationSpaced, Assignment,
}

public static class LexicalComponents
{
    public static Dictionary<TokenTypes, Regex> tokenRegexPatterns = new()
    {
        { TokenTypes.Separators, new Regex("^\\,|^\\(|^\\)|^\\[|^\\]|^\\{|^\\}|^\\:|^\\;|^\\=>|^\\.") },
        { TokenTypes.Keyword, new Regex("^effect$?(?![a-zA-Z0-9])|^card$?(?![a-zA-Z0-9])|^Name$?(?![a-zA-Z0-9])|^while$?(?![a-zA-Z0-9])|^for$?(?![a-zA-Z0-9])|^String$?(?![a-zA-Z0-9])|^Params$?(?![a-zA-Z0-9])|^Number$?(?![a-zA-Z0-9])|^Bool$?(?![a-zA-Z0-9])|^Action$?|^Type$?(?![a-zA-Z0-9])|^Faction$?(?![a-zA-Z0-9])|^Power$?(?![a-zA-Z0-9])|^Range$?(?![a-zA-Z0-9])|^OnActivation$?(?![a-zA-Z0-9])|^Effect$?(?![a-zA-Z0-9])|^Selector$?(?![a-zA-Z0-9])|^Source$?(?![a-zA-Z0-9])|^Single$?(?![a-zA-Z0-9])|^Predicate$?(?![a-zA-Z0-9])|^PostAction$(?![a-zA-Z0-9])") },
        { TokenTypes.BooleanLiteral, new Regex("^true$|^false$") },
        { TokenTypes.NumericLiteral, new Regex("^-?\\+?\\d+(\\.\\d+)(?![a-zA-Z0-9])") },
        { TokenTypes.StringLiteral, new Regex("^\"[^\"]*\"") },
        { TokenTypes.ArithmeticOperator, new Regex("^\\+|^-|^\\*|^/|^\\^")},
        { TokenTypes.ComparisonOperator, new Regex("^>=|^<=|^==|^<|^>")},
        { TokenTypes.LogicalOperator, new Regex("^&&|^\\|\\|")},
        { TokenTypes.AssignmentOperator, new Regex("^=")},
        { TokenTypes.MembershipOperator, new Regex("^in$?(?![a-zA-Z0-9])")},
        { TokenTypes.IncrementDecrementOPerator, new Regex("^\\+\\+| ^\\--")},
        { TokenTypes.StringOperator, new Regex("^@@|^@") },
        { TokenTypes.Identifier, new Regex("^([a-zA-Z_]\\w*)") },
        { TokenTypes.WhiteSpaces, new Regex(@"\s+") },
        { TokenTypes.Comments, new Regex(@"\/\/[^/\n]*") },
    };

    public static Dictionary<TokenTypes, Dictionary<string, TokenSubtypes>> SubtypesCorrespondecy = new()
    {
        {
            TokenTypes.Separators, new Dictionary<string, TokenSubtypes>
            {
                {",", TokenSubtypes.Comma},
                {"(", TokenSubtypes.OpenParenthesis},
                {")", TokenSubtypes.CloseParenthesis},
                {"[", TokenSubtypes.OpenBracket},
                {"]", TokenSubtypes.CloseBracket},
                {"{", TokenSubtypes.OpenBrace},
                {"}", TokenSubtypes.CloseBrace},
                {":", TokenSubtypes.Colon},
                {";", TokenSubtypes.Semicolon},
                {"=>", TokenSubtypes.Lambda},
                {".", TokenSubtypes.Dot},
            }
        },
        {
            TokenTypes.Keyword, new Dictionary<string, TokenSubtypes>
            {
                {"effect", TokenSubtypes.effect},
                {"card", TokenSubtypes.card},
                {"Name", TokenSubtypes.Name},
                {"while", TokenSubtypes.While},
                {"for", TokenSubtypes.For},
                {"String", TokenSubtypes.String},
                {"Number", TokenSubtypes.Number},
                {"Bool", TokenSubtypes.Bool},
                {"Params", TokenSubtypes.Params},
                {"Action", TokenSubtypes.Action},
                {"Faction", TokenSubtypes.Faction},
                {"Range", TokenSubtypes.Range},
                {"OnActivation", TokenSubtypes.OnActivation},
                {"Effect", TokenSubtypes.Effect},
                {"Selector", TokenSubtypes.Selector},
                {"Source", TokenSubtypes.Source},
                {"Single", TokenSubtypes.Single},
                {"Predicate", TokenSubtypes.Predicate},
                {"PostAction", TokenSubtypes.PostAction}
            }
        },
        {
            TokenTypes.BooleanLiteral, new Dictionary<string, TokenSubtypes>
            {
                {"true", TokenSubtypes.True},
                {"false", TokenSubtypes.False},
            }
        },
        {
            TokenTypes.ArithmeticOperator, new Dictionary<string, TokenSubtypes>
            {
                {"+", TokenSubtypes.Addition},
                {"-", TokenSubtypes.Subtraction},
                {"*", TokenSubtypes.Multiplication},
                {"/", TokenSubtypes.Division},
                {"^", TokenSubtypes.Potentiation},
            }
        },
        {
            TokenTypes.ComparisonOperator, new Dictionary<string, TokenSubtypes>
            {
                {">=", TokenSubtypes.GreaterThanOrEqual},
                {"<=", TokenSubtypes.LessThanOrEqual},
                {"<", TokenSubtypes.LessThan},
                {">", TokenSubtypes.GreaterThan},
            }
        },
        {
            TokenTypes.LogicalOperator, new Dictionary<string, TokenSubtypes>
            {
                {"&&", TokenSubtypes.AND},
                {"||", TokenSubtypes.OR},
            }
        },
        {
            TokenTypes.AssignmentOperator, new Dictionary<string, TokenSubtypes>
            {
                {"=", TokenSubtypes.Assignment},
            }
        },
        {
            TokenTypes.MembershipOperator, new Dictionary<string, TokenSubtypes>
            {
                {"in", TokenSubtypes.In},
            }
        },
        {
            TokenTypes.IncrementDecrementOPerator, new Dictionary<string, TokenSubtypes>
            {
                {"++", TokenSubtypes.PostIncrement},
                {"--", TokenSubtypes.PostDecrement},
            }
        },
        {
            TokenTypes.StringOperator, new Dictionary<string, TokenSubtypes>
            {
                {"@", TokenSubtypes.StringConcatenation},
                {"@@", TokenSubtypes.StringConcatenationSpaced},
            }
        },
    };


}