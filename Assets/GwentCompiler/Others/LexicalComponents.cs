using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

[Serializable]
public enum TokenTypes
{
    Identifier,
    Keyword,
    Punctuation,
    ArithmeticOperator, ComparisonOperator, LogicalOperator, AssignmentOperator, MembershipOperator, IncrementDecrementOPerator, StringOperator,
    NumericLiteral, StringLiteral, BooleanLiteral,
    Comments,
    WhiteSpaces,
    EOF,
}

[Serializable]
public enum TokenSubtypes
{
    Comma, OpenParenthesis, CloseParenthesis, OpenBracket, CloseBracket, OpenBrace, CloseBrace, Colon, Semicolon, Lambda, Dot,
    effect, card, Name, While, For, String, Number, Bool, Params, Action, Type, Faction, Power, Range, OnActivation, Effect, Selector, Source, Single, Predicate, PostAction, If, Else, EffectDescription, CharacterDescription, Quote, TriggerPlayer, Board, HandOfPlayer, FieldOfPlayer, GraveyardOfPlayer, DeckOfPlayer, Owner, Find, Push, SendBottom, Pop, Remove, Shuffle, Hand, Deck, Field, Graveyard,
    Negation, True, False, NumericLiteral, StringLiteral, Identifier, Comments, WhiteSpaces,
    Addition, Subtraction, Multiplication, Division, Potentiation, Equality, Inequality, AND, OR, GreaterThanOrEqual, LessThanOrEqual, GreaterThan, LessThan, In, PostIncrement, PostDecrement, StringConcatenation, StringConcatenationSpaced, Assignment,
    EOF,
    Print,
}

public static class LexicalComponents
{
    public static Dictionary<TokenTypes, Regex> tokenRegexPatterns = new()
    {
        { TokenTypes.Punctuation, new Regex("^\\,|^\\(|^\\)|^\\[|^\\]|^\\{|^\\}|^\\:|^\\;|^\\=>|^\\.") },
        { TokenTypes.Keyword, new Regex("^effect$?(?![a-zA-Z0-9])|^card$?(?![a-zA-Z0-9])|^Name$?(?![a-zA-Z0-9])|^while$?(?![a-zA-Z0-9])|^for$?(?![a-zA-Z0-9])|^String$?(?![a-zA-Z0-9])|^Params$?(?![a-zA-Z0-9])|^Number$?(?![a-zA-Z0-9])|^Bool$?(?![a-zA-Z0-9])|^Action$?|^Type$?(?![a-zA-Z0-9])|^Faction$?(?![a-zA-Z0-9])|^Power$?(?![a-zA-Z0-9])|^Range$?(?![a-zA-Z0-9])|^OnActivation$?(?![a-zA-Z0-9])|^Effect$?(?![a-zA-Z0-9])|^Selector$?(?![a-zA-Z0-9])|^Source$?(?![a-zA-Z0-9])|^Single$?(?![a-zA-Z0-9])|^Predicate$?(?![a-zA-Z0-9])|^PostAction$?(?![a-zA-Z0-9])|^print$?(?![a-zA-Z0-9])|^if$?(?![a-zA-Z0-9])|^else$?(?![a-zA-Z0-9])|^CharacterDescription$?(?![a-zA-Z0-9])|^EffectDescription$?(?![a-zA-Z0-9])|^Quote$?(?![a-zA-Z0-9])|^TriggerPlayer$?(?![a-zA-Z0-9])|^Board$?(?![a-zA-Z0-9])|^HandOfPlayer$?(?![a-zA-Z0-9])|^GraveyardOfPlayer$?(?![a-zA-Z0-9])|DeckOfPlayer|^Owner$?(?![a-zA-Z0-9])|^Find$?(?![a-zA-Z0-9])|^Push$?(?![a-zA-Z0-9])|^SendBottom$?(?![a-zA-Z0-9])|^Pop$?(?![a-zA-Z0-9])|^Remove$?(?![a-zA-Z0-9])|^Shuffle$?(?![a-zA-Z0-9])|^FieldOfPlayer$?(?![a-zA-Z0-9])|^Deck$?(?![a-zA-Z0-9])|^Hand$?(?![a-zA-Z0-9])|^Field$?(?![a-zA-Z0-9])|^Graveyard$?(?![a-zA-Z0-9])") },
        { TokenTypes.BooleanLiteral, new Regex("^true(?![a-zA-Z0-9])|^false(?![a-zA-Z0-9])") },
        { TokenTypes.NumericLiteral, new Regex(@"^\d+(\.\d+)?\b") },
        { TokenTypes.StringLiteral, new Regex("^\"[^\"]*\"") },
        { TokenTypes.IncrementDecrementOPerator, new Regex("^\\+\\+|^-\\-")},
        { TokenTypes.ArithmeticOperator, new Regex("^\\+|^-|^\\*|^/|^\\^")},
        { TokenTypes.ComparisonOperator, new Regex("^>=|^<=|^==|^!=|^<|^>")},
        { TokenTypes.AssignmentOperator, new Regex("^=")},
        { TokenTypes.LogicalOperator, new Regex("^&&|^\\|\\||^!")},
        { TokenTypes.MembershipOperator, new Regex("^in$?(?![a-zA-Z0-9])")},
        { TokenTypes.StringOperator, new Regex("^@@|^@") },
        { TokenTypes.Identifier, new Regex("^([a-zA-Z_]\\w*)") },
        { TokenTypes.Comments, new Regex(@"\/\/[^/\n]*") },
        { TokenTypes.WhiteSpaces, new Regex(@"\s+") },
    };

    public static Dictionary<TokenTypes, Dictionary<string, TokenSubtypes>> SubtypesCorrespondecy = new()
    {
        {
            TokenTypes.Punctuation, new Dictionary<string, TokenSubtypes>
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
                {"PostAction", TokenSubtypes.PostAction},
                {"Power", TokenSubtypes.Power},
                {"print", TokenSubtypes.Print},
                {"if", TokenSubtypes.If},
                {"else", TokenSubtypes.Else},
                {"EffectDescription", TokenSubtypes.EffectDescription},
                {"CharacterDescription", TokenSubtypes.CharacterDescription},
                {"Quote", TokenSubtypes.Quote},
                {"Type", TokenSubtypes.Type},
                {"TriggerPlayer", TokenSubtypes.TriggerPlayer},
                {"Board", TokenSubtypes.Board},
                {"HandOfPlayer", TokenSubtypes.HandOfPlayer},
                {"FieldOfPlayer", TokenSubtypes.FieldOfPlayer},
                {"GraveyardOfPlayer", TokenSubtypes.GraveyardOfPlayer},
                {"DeckOfPlayer", TokenSubtypes.DeckOfPlayer},
                {"Owner", TokenSubtypes.Owner},
                {"Find", TokenSubtypes.Find},
                {"Push", TokenSubtypes.Push},
                {"SendBottom", TokenSubtypes.SendBottom},
                {"Pop", TokenSubtypes.Pop},
                {"Remove", TokenSubtypes.Remove},
                {"Shuffle", TokenSubtypes.Shuffle},
                {"Hand", TokenSubtypes.Hand},
                {"Deck", TokenSubtypes.Deck},
                {"Graveyard", TokenSubtypes.Graveyard},
                {"Field", TokenSubtypes.Field},
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
                {"==", TokenSubtypes.Equality},
                {"!=", TokenSubtypes.Inequality}
            }
        },
        {
            TokenTypes.LogicalOperator, new Dictionary<string, TokenSubtypes>
            {
                {"&&", TokenSubtypes.AND},
                {"||", TokenSubtypes.OR},
                {"!" , TokenSubtypes.Negation}
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
