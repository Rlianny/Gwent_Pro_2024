using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
public interface ICardComponent : IComponent { }

#region CardComponents
public abstract class GenericFieldComponent : ICardComponent, IEffectComponent
{
    public CodeLocation Operator { get; private set; }
    public IExpression Value { get; private set; }

    public GenericFieldComponent(IExpression value, CodeLocation @operator)
    {
        Operator = @operator;
        Value = value;
    }
}
public class CardRangeDeclaration : ICardComponent
{
    public List<IExpression> Ranges { get; private set; }
    public CodeLocation Operator { get; private set; }

    public CardRangeDeclaration(List<IExpression> value, CodeLocation @operator)
    {
        Ranges = value;
        Operator = @operator;
    }
}
public class OnActivation : ICardComponent
{
    public CodeLocation Operator { get; private set; } 
    public List<ActivationData> Activations;

    public OnActivation(List<ActivationData> data, CodeLocation @operator)
    {
        Operator = @operator;
        Activations = data;
    }
}
public class ActivationData : ICardComponent
{
    public EffectInfo Effect {get; private set;}
    public Selector Selector {get; private set;}
    public PostAction PostAction {get; private set;}

    public ActivationData(EffectInfo effect, Selector selector, PostAction postAction)
    {
        Effect = effect;
        Selector = selector;
        PostAction = postAction;
    }
}

#endregion

#region GenericFieldComponent
public class CardTypeDeclaration : GenericFieldComponent
{
    public CardTypeDeclaration(IExpression value, CodeLocation @operator) : base(value, @operator) { }
}
public class NameDeclaration : GenericFieldComponent
{
    public NameDeclaration(IExpression value, CodeLocation @operator) : base(value, @operator) { }
}
public class CardFactionDeclaration: GenericFieldComponent
{
    public CardFactionDeclaration(IExpression value, CodeLocation @operator) : base(value, @operator) { }
}
public class CardPowerDeclaration : GenericFieldComponent
{
    public CardPowerDeclaration(IExpression value, CodeLocation @operator) : base(value, @operator) { }
}
public class CardEffectDescriptionDeclaration : GenericFieldComponent
{
    public CardEffectDescriptionDeclaration(IExpression value, CodeLocation @operator) : base(value, @operator) { }
}
public class CardCharacterDescriptionDeclaration : GenericFieldComponent
{
    public CardCharacterDescriptionDeclaration(IExpression value, CodeLocation @operator) : base(value, @operator) { }
}
public class CardQuoteDeclaration : GenericFieldComponent
{
    public CardQuoteDeclaration(IExpression value, CodeLocation @operator) : base(value, @operator) { }
}

#endregion

#region OnActivationFieldComponents
public class EffectInfo
{
    public IExpression EffectName { get; private set; } 
    public List<ParsedParam> ActivationParams { get; private set; } 
    public CodeLocation Colon { get; private set; }

    public EffectInfo(IExpression effectName, List<ParsedParam> @params, CodeLocation colon)
    {
        EffectName = effectName;
        ActivationParams = @params;
        Colon = colon;
    }
}

public class Selector
{
    public SelectorSource Source { get; private set; } 
    public SelectorSingle Single { get; private set; }
    public SelectorPredicate Predicate { get; private set; } 

    public Selector(SelectorSource source, SelectorSingle single, SelectorPredicate predicate)
    {
        Source = source;
        Single = single;
        Predicate = predicate;
    }
}

public class SelectorSource
{
    public IExpression Source { get; private set; } 
    public CodeLocation Operator { get; private set; }

    public SelectorSource(IExpression source, CodeLocation colon)
    {
        Source = source;
        Operator = colon;
    }
}

public class SelectorSingle
{
    public IExpression Single { get; private set; } 
    public CodeLocation Operator { get; private set; } 

    public SelectorSingle(IExpression single, CodeLocation op)
    {
        Single = single;
        Operator = op;
    }
}

public class SelectorPredicate
{
    public LambdaExpr LambdaExpression { get; private set; }

    public SelectorPredicate(LambdaExpr expression)
    {
        LambdaExpression = expression;
    }
}

public class PostAction
{
    public ActivationData LinkedEffect { get; private set; }

    public PostAction(ActivationData linkedEffect)
    {
        LinkedEffect = linkedEffect;
    }
}

public class ParsedParam
{
    public Variable VarName { get; private set; } 
    public CodeLocation Colon { get; private set; } 
    public IExpression Value { get; private set; }
    public Token Type {get; private set;}

    public ParsedParam(Variable variable, CodeLocation colon, Token type)
    {
        VarName = variable;
        Colon = colon;
        Type = type;
    }

    public ParsedParam(Variable variable, CodeLocation colon, IExpression expression)
    {
        VarName = variable;
        Colon = colon;
        Value = expression;
    }
}

#endregion