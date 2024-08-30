using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public partial class Parser
{
    private IProgramNode Card()
    {
        Token cardLocation = Previous();
        CardDeclaration parsedCard = new(cardLocation);

        Consume(TokenSubtypes.OpenBrace, "Card must declare a body", null);
        while (!Match(TokenSubtypes.CloseBrace))
        {
            if (Match(TokenSubtypes.Type))
            {
                ICardComponent value = GenericField();
                if (value == null)
                    GenerateError("The card must to declare a type", Previous().Location);
        
                else if (!parsedCard.SetComponent(value))
                    GenerateError("The card type has been defined before", Previous().Location);
                    
                continue;
            }

            if (Match(TokenSubtypes.Name))
            {
                ICardComponent value = GenericField();
                if (value == null)
                    GenerateError("The card must to declare a name", Previous().Location);

                else if (!parsedCard.SetComponent(value))
                    GenerateError("The card name has been defined before", Previous().Location);

                continue;
            }

            if (Match(TokenSubtypes.Faction))
            {
                ICardComponent value = GenericField();
                if (value == null)
                    GenerateError("The card must to declare a faction", Previous().Location);
                    
                else if (!parsedCard.SetComponent(value))
                    GenerateError("The card faction has been defined before", Previous().Location);

                continue;
            }

            if (Match(TokenSubtypes.Range))
            {
                ICardComponent value = CardRange();
                if (value == null)
                    GenerateError("The card must to declare a range", Previous().Location);
        
                else if (!parsedCard.SetComponent(value))
                    GenerateError("The card range has been defined before", Previous().Location);
                   
                continue;
            }

            if (Match(TokenSubtypes.Power))
            {
                ICardComponent value = GenericField();
                if (value == null)
                    GenerateError("The card must to declare a power", Previous().Location);
                    
                else if (!parsedCard.SetComponent(value))
                    GenerateError("The card power has been defined before", Previous().Location);
                    
                continue;
            }

            if (Match(TokenSubtypes.OnActivation))
            {
                ICardComponent value = OnActivation();
                if (value == null)
                    GenerateError("The card must to declare a OnActivation field", Previous().Location);

                else if (!parsedCard.SetComponent(value))
                    GenerateError("The card OnActivation field has been defined before", Previous().Location);
                    
                continue;
            }

            if (Match(TokenSubtypes.EffectDescription))
            {
                ICardComponent value = GenericField();
                if (value == null)
                    GenerateError("The card must to declare a effect description", Previous().Location);
                    
                else if (!parsedCard.SetComponent(value))
                    GenerateError("The effect description has been defined before", Previous().Location);
        
                continue;
            }

            if (Match(TokenSubtypes.CharacterDescription))
            {
                ICardComponent value = GenericField();
                if (value == null)
                    GenerateError("The card must to declare a character description", Previous().Location);
                   
                else if (!parsedCard.SetComponent(value))
                    GenerateError("The character description has been defined before", Previous().Location);
                    
                continue;
            }

            if (Match(TokenSubtypes.Quote))
            {
                ICardComponent value = GenericField();
                if (value == null)
                    GenerateError("The card must to declare a quote", Previous().Location);
                   
                else if (!parsedCard.SetComponent(value))
                    GenerateError("The card quote has been defined before", Previous().Location);
                    
                continue;
            }

            Synchronize(new List<TokenSubtypes> { TokenSubtypes.card, TokenSubtypes.effect });
            break;
        }

        return parsedCard;

    }

    private GenericFieldComponent GenericField()
    {
        var component = Previous();
        Token colon = Consume(TokenSubtypes.Colon, "Expected ':'", new List<TokenSubtypes> { TokenSubtypes.Comma });
        CodeLocation colonLocation = Previous().Location;
        IExpression value = Expression();

        if (value == null) return null;

        Consume(TokenSubtypes.Comma, "Expected ',' after field declaration", null);

        switch (component.Subtype)
        {
            case TokenSubtypes.Name:
                return new NameDeclaration(value, colonLocation);
            case TokenSubtypes.Faction:
                return new CardFactionDeclaration(value, colonLocation);
            case TokenSubtypes.Power:
                return new CardPowerDeclaration(value, colonLocation);
            case TokenSubtypes.EffectDescription:
                return new CardEffectDescriptionDeclaration(value, colonLocation);
            case TokenSubtypes.CharacterDescription:
                return new CardCharacterDescriptionDeclaration(value, colonLocation);
            case TokenSubtypes.Quote:
                return new CardQuoteDeclaration(value, colonLocation);
            case TokenSubtypes.Type:
                return new CardTypeDeclaration(value, colonLocation);
        }

        return null;
    }

    private ICardComponent CardRange()
    {
        List<IExpression> ranges = new();
        Token colon = Consume(TokenSubtypes.Colon, "Expected ':'", null);
        CodeLocation colonLocation = Previous().Location;
        Consume(TokenSubtypes.OpenBracket, "Expect '['", new List<TokenSubtypes> { TokenSubtypes.CloseBracket });
        while (!Match(TokenSubtypes.CloseBracket))
        {
            IExpression expression = Expression();
            if (expression != null)
            ranges.Add(expression);
            else GenerateError("Invalid range", colonLocation);
            if (!Check(TokenSubtypes.CloseBracket))
            {
                Consume(TokenSubtypes.Comma, "Expect ','", new List<TokenSubtypes> { TokenSubtypes.CloseBracket });
            }
        }
        Consume(TokenSubtypes.Comma, "Expect ','", null);
        return new CardRangeDeclaration(ranges, colonLocation);
    }

    private ICardComponent OnActivation()
    {
        Token colon = Consume(TokenSubtypes.Colon, "Expected ':'", null);
        CodeLocation colonLocation = Previous().Location;
        Token openBracket = Consume(TokenSubtypes.OpenBracket, "Expect '['", null);
        List<ActivationData> data = new();
        while (!Match(TokenSubtypes.CloseBracket))
        {
            ActivationData activationData = ActivationData();
            if(activationData != null)
                data.Add(activationData);
            else break;
        }
        return new OnActivation(data, colonLocation);

    }

    private ActivationData ActivationData()
    {
        var openBrace = Consume(TokenSubtypes.OpenBrace, "Was expected '{'", null);
        EffectInfo effectInfo = EffectInfo();
        if (effectInfo == null) return null;
        Selector selector = Selector();
        PostAction postAction = PostAction();
        var CloseBrace = Consume(TokenSubtypes.CloseBrace, "Was expected '}'", null);

        return new ActivationData(effectInfo, selector, postAction);
    }

    private EffectInfo EffectInfo()
    {
        if(!Check(new List<TokenSubtypes>() {TokenSubtypes.Effect, TokenSubtypes.Type}))
        {
            GenerateError("Effect information declaration expected", Previous().Location);
            Synchronize(new List<TokenSubtypes>() {TokenSubtypes.Comma});
            return null;
        }
        Advance();
        Consume(TokenSubtypes.Colon, "Was expected ':'", null);
        CodeLocation colonLocation = Previous().Location;
        IExpression name = null;
        List<ParsedParam> @params = new();
        if (Match(TokenSubtypes.OpenBrace))
        {
            var tokenName = Consume(TokenSubtypes.Name, "Effect name expected", new List<TokenSubtypes> { TokenSubtypes.Comma });
            Consume(TokenSubtypes.Colon, "Was expected ':'", null);
            name = Expression();
            Consume(TokenSubtypes.Comma, "Was expected ','", null);
            while (!Match(TokenSubtypes.CloseBrace))
            {
                ParsedParam param = ParsedParam();
                if(param != null)
                @params.Add(param);
                else 
                {
                    Synchronize(new List<TokenSubtypes>() { TokenSubtypes.CloseBrace });
                    break;
                }
            }
            Consume(TokenSubtypes.Comma, "Was expected ','", null);
        }
        else 
        {
            name = Expression();
            Consume(TokenSubtypes.Comma, "Was expected ','", null);
            while (Check(TokenSubtypes.Identifier))
            {
                ParsedParam param = ParsedParam();
                if(param != null)
                @params.Add(param);
                else 
                {
                    Synchronize(new List<TokenSubtypes>() { TokenSubtypes.CloseBrace });
                    break;
                }
            }
        }

        return new EffectInfo(name, @params, colonLocation);

    }
    
    private ParsedParam ParsedParam()
    {
        Token token = Peek();
        var variable = Expression();
        if (variable is Variable var)
        {
            Token colon = Consume(TokenSubtypes.Colon, "Was expected ':'", null);
            CodeLocation colonLocation = Previous().Location;
            IExpression value = Expression();
            Token comma = Consume(TokenSubtypes.Comma, "Was expected ','", null);
            return new ParsedParam(var, colonLocation, value);
        }
        GenerateError("Was expected a variable", token.Location);
        Synchronize(new List<TokenSubtypes>() { TokenSubtypes.CloseBrace });
        return null;
    }

    private Selector Selector()
    {
        var selector = Consume(TokenSubtypes.Selector, "Effect selector declaration expected", new List<TokenSubtypes>(){TokenSubtypes.CloseBracket});
        if (selector == null) return null;
        Token colon = Consume(TokenSubtypes.Colon, "Was expected ':'", null);
        Token openBrace = Consume(TokenSubtypes.OpenBrace, "Was expected '{'", null);
        SelectorSource source = Source();
        SelectorSingle single = null;
        if (Match(TokenSubtypes.Single))
        {
            single = Single();
        }
        SelectorPredicate predicate = Predicate();
        Token openBrace1 = Consume(TokenSubtypes.CloseBrace, "Was expected '}'", null);
        Token comma = Consume(TokenSubtypes.Comma, "Was expect ','", null);
        return new Selector(source, single, predicate);
    }

    private SelectorSource Source()
    {
        Consume(TokenSubtypes.Source, "Source declaration expected", new List<TokenSubtypes> { TokenSubtypes.CloseBrace });
        Token colon = Consume(TokenSubtypes.Colon, "Was expected ':'", null);
        CodeLocation colonLocation = Previous().Location;
        IExpression source = Expression();
        Consume(TokenSubtypes.Comma, "Was expected ','", null);
        return new SelectorSource(source, colonLocation);
    }

    public SelectorSingle Single()
    {
        Token colon = Consume(TokenSubtypes.Colon, "Was expected ':'", null);
        CodeLocation colonLocation = Previous().Location;
        IExpression single = Expression();
        var comma = Consume(TokenSubtypes.Comma, "Was expected ','", null);
        return new SelectorSingle(single, colonLocation);
    }

    public SelectorPredicate Predicate()
    {
        Consume(TokenSubtypes.Predicate, "Predicate declaration expected", new List<TokenSubtypes> { TokenSubtypes.CloseBrace });
        Token colon = Consume(TokenSubtypes.Colon, "Was expected ':'", null);
        CodeLocation colonLocation = Previous().Location;
        var var = Expression();
        if (var is LambdaExpr lambdaExpr)
        {
            return new SelectorPredicate(lambdaExpr);
        }
        GenerateError("Was expect a lambda expression", colonLocation);
        Synchronize(new List<TokenSubtypes> { TokenSubtypes.CloseBrace });
        return null;
    }

    private PostAction PostAction()
    {
        if (Match(TokenSubtypes.PostAction))
        {
            Token postActionColon = Consume(TokenSubtypes.Colon, "Was expected ':'", null);
            Consume(TokenSubtypes.OpenBrace, "Was expected '{'", null);
            Consume(TokenSubtypes.Type, "Effect name declaration expected", new List<TokenSubtypes> { TokenSubtypes.CloseBrace });
            Token colon = Consume(TokenSubtypes.Colon, "Was expected ':'", null);
            CodeLocation colonLocation = Previous().Location;
            IExpression name = Expression();
            Consume(TokenSubtypes.Comma, "Was expected ','", null);
            List<ParsedParam> @params = new();
            while (Check(TokenSubtypes.Identifier))
            {
                ParsedParam param = ParsedParam();
                if(param != null)
                @params.Add(param);
                else 
                {
                    Synchronize(new List<TokenSubtypes>() { TokenSubtypes.CloseBrace });
                    break;
                }
            }
            EffectInfo info = new EffectInfo(name, @params, colonLocation);
            Selector selector = null;
            if (Peek().Subtype == TokenSubtypes.Selector)
            {
                selector = Selector();
            }
            PostAction postAction = PostAction();
            Consume(TokenSubtypes.CloseBrace, "Was expected '}'", null);
            return new PostAction(new ActivationData(info, selector, postAction));
        }
        else return null;
    }


}