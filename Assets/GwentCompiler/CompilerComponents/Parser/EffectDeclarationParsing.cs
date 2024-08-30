using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public partial class Parser
{
    private IProgramNode Effect()
    {
        Token effectLocation = Previous();
        EffectDeclaration parsedEffect = new EffectDeclaration(effectLocation);

        Consume(TokenSubtypes.OpenBrace, "Effect must declare a body", null);

        while (!Match(TokenSubtypes.CloseBrace))
        {
            if (Match(TokenSubtypes.Name))
            {
                IEffectComponent value = GenericField();
                if (value == null)
                    GenerateError("The effect must to declare a name", Previous().Location);

                else if (!parsedEffect.SetComponent(value))
                    GenerateError("The effect name has been defined before", Previous().Location);

                continue;
            }

            if (Match(TokenSubtypes.Params))
            {
                IEffectComponent value = Params();
                if (value == null)
                    GenerateError("The effect must to declare parameters", Previous().Location);

                else if (!parsedEffect.SetComponent(value))
                    GenerateError("The effect parameters has been defined before", Previous().Location);
                continue;
            }

            if (Match(TokenSubtypes.Action))
            {
                IEffectComponent value = Action();
                if (value == null)
                    GenerateError("The effect must to declare an action", Previous().Location);

                else if (!parsedEffect.SetComponent(value))
                    GenerateError("The effect action has been defined before", Previous().Location);

                continue;
            }

            Synchronize(new List<TokenSubtypes> { TokenSubtypes.card, TokenSubtypes.effect });
            break;
        }

        return parsedEffect;
    }

    private IEffectComponent Params()
    {
        Token colon = Consume(TokenSubtypes.Colon, "Was expected ':'", null);
        Consume(TokenSubtypes.OpenBrace, "Was expected '{'", null);
        List<ParsedParam> @params = new();
        while (!Match(TokenSubtypes.CloseBrace))
        {
            ParsedParam param = ParsedEffectParam();
            if (param != null)
                @params.Add(param);
            else
            {
                Synchronize(new List<TokenSubtypes>() { TokenSubtypes.CloseBrace });
                break;
            }
        }
        Consume(TokenSubtypes.Comma, "Was expected ','", null);
        return new EffectParamsDeclaration(@params);
    }

    private ParsedParam ParsedEffectParam()
    {
        IExpression expression = Expression();
        Token token = Peek();
        Token colon = Consume(TokenSubtypes.Colon, "Was expected ':'", null);
        CodeLocation colonLocation = Previous().Location;
        if (expression is Variable var && Match(new List<TokenSubtypes>() { TokenSubtypes.Number, TokenSubtypes.String, TokenSubtypes.Bool }))
        {
            Token type = Previous();
            return new ParsedParam(var, colonLocation, type);
        }
        GenerateError("Was expected a variable", token.Location);
        Synchronize(null);
        return null;
    }

    private IEffectComponent Action()
    {
        Token actionToken = Previous();
        Token colon = Consume(TokenSubtypes.Colon, "Was expected ':'", null);
        Consume(TokenSubtypes.OpenParenthesis, "Was expected '('", null);
        Variable targetsVar = null;
        Token location = Previous();
        var targets = Expression();
        if (targets is Variable variableTargets)
        {
            targetsVar = variableTargets;
        }
        else GenerateError("Variable was expected", location.Location);
        Token comma = Consume(TokenSubtypes.Comma, "Was expected ','", null);
        Variable contextVar = null;
        location = Previous();
        var context = Expression();
        if (context is Variable variableContext)
        {
            contextVar = variableContext;
        }
        else GenerateError("Variable was expected", location.Location);
        Consume(TokenSubtypes.CloseParenthesis, "Was expected ')'", null);
        Consume(TokenSubtypes.Lambda, "Was expected '=>'", null);
        IStatement block = null;
        location = Previous();
        IStatement statement = Statement();
        if(statement != null)
        block = statement;
        else GenerateError("Block was expected", location.Location);
        if (targetsVar != null && contextVar != null && block != null)
            return new EffectAction(targetsVar, contextVar, block);
        else return null;
    }

}