using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
public partial class Interpreter : VisitorBase<object>
{
    public object Visit(CardTypeDeclaration declaration)
    {
        var value = Evaluate(declaration.Value);

        if (value is string stringType)
        {
            if (stringType == "Oro" || stringType == "Plata" || stringType == "Líder") return value;
            else
            {
                throw new RuntimeError("Invalid type declaration, 'Oro', 'Plata' or 'Líder' was expected", declaration.Operator);
            }
        }

        else
        {
            throw new RuntimeError("The 'Type' must be a string value", declaration.Operator);
        }
    }

    public object Visit(NameDeclaration declaration)
    {
        var value = Evaluate(declaration.Value);

        if (value != null && value is string stringName)
        {
            return stringName;
        }
        else
        {
            throw new RuntimeError("The 'Name' must be a string value", declaration.Operator);
        }
    }

    public object Visit(CardFactionDeclaration declaration)
    {
        var value = Evaluate(declaration.Value);

        if (value is string stringFaction)
        {
            return stringFaction;
        }
        else
        {
            throw new RuntimeError("The 'Faction' must be a string value", declaration.Operator);
        }
    }

    public object Visit(CardPowerDeclaration declaration)
    {
        var value = Evaluate(declaration.Value);

        if (value is double intPower)
        {
            if (intPower > 0)
                return intPower;

            else throw new RuntimeError("The 'Power' must be a positive numeric value", declaration.Operator);
        }
        else
        {
            throw new RuntimeError("The 'Power' must to be a numeric value", declaration.Operator);
        }
    }

    public object Visit(CardEffectDescriptionDeclaration declaration)
    {
        var value = Evaluate(declaration.Value);

        if (value is string stringValue)
        {
            return stringValue;
        }
        else
        {
            throw new RuntimeError("The 'Effect Description' must be a string value", declaration.Operator);
        }
    }

    public object Visit(CardCharacterDescriptionDeclaration declaration)
    {
        var value = Evaluate(declaration.Value);

        if (value is string stringValue)
        {
            return stringValue;
        }
        else
        {
            throw new RuntimeError("The 'Character Description' must be a string value", declaration.Operator);
        }
    }

    public object Visit(CardQuoteDeclaration declaration)
    {
        var value = Evaluate(declaration.Value);

        if (value is string stringValue)
        {
            return stringValue;
        }
        else
        {
            throw new RuntimeError("The 'Quote' must be a string value", declaration.Operator);
        }
    }

    public object Visit(CardRangeDeclaration declaration)
    {
        List<string> ranges = new();

        bool melee = false, ranged = false, siege = false;

        foreach (IExpression expression in declaration.Ranges)
        {
            var value = Evaluate(expression);

            if (value is string stringValue)
            {
                if (stringValue == "Melee")
                {
                    if (melee) throw new RuntimeError("The range 'Melee' has already been declared", declaration.Operator);
                    melee = true;
                    ranges.Add(stringValue);
                    continue;
                }

                if (stringValue == "Ranged")
                {
                    if (ranged) throw new RuntimeError("The range 'Ranged' has already been declared", declaration.Operator);
                    ranged = true;
                    ranges.Add(stringValue);
                    continue;
                }

                if (stringValue == "Siege")
                {
                    if (siege) throw new RuntimeError("The range 'Siege' has already been declared", declaration.Operator);
                    siege = true;
                    ranges.Add(stringValue);
                    continue;
                }

                throw new RuntimeError("Invalid 'Range' declaration, only the ranges 'Melee', 'Siege' and 'Ranged' are accepted", declaration.Operator);
            }
            else
            {
                throw new RuntimeError("The 'Range' must be a string value", declaration.Operator);
            }
        }

        return ranges;
    }

    public object Visit(OnActivation onActivation)
    {
        List<EffectActivation> effects = new();

        foreach (var activation in onActivation.Activations)
        {
            object newActivation = Visit(activation, true);
            if (newActivation != null)
                effects.Add((EffectActivation)newActivation);
        }

        return effects;
    }

    public object Visit(ActivationData data, bool isRoot)
    {
        string effectName = null;
        Dictionary<Parameter, object> Params = null;
        string selectorSource = null;
        bool selectorSingle = false;
        EffectActivation postAction = null;
        Delegate @delegate = null;

        var name = Evaluate(data.Effect.EffectName);
        if (name is string stringName) effectName = stringName;
        else throw new RuntimeError("The 'Name' must be a string value", data.Effect.Colon);

        if (Effect.CheckEffectExistance(stringName))
        {
            Dictionary<Parameter, object> parameter = new();

            if (data.Effect.ActivationParams != null)
            {
                foreach (var activationParam in data.Effect.ActivationParams)
                {
                    object paramValue = Evaluate(activationParam.Value);
                    string paramName = activationParam.VarName.Value.Lexeme;
                    Parameter newParam = null;

                    if (paramValue is string stringValue)
                    {
                        newParam = new Parameter(paramName, ValueType.String);
                        paramValue = stringValue;
                    }

                    else if (paramValue is bool booleanValue)
                    {
                        newParam = new Parameter(paramName, ValueType.Boolean);
                        paramValue = booleanValue;
                    }

                    else if (paramValue is double numericValue)
                    {
                        newParam = new Parameter(paramName, ValueType.Number);
                        paramValue = numericValue;
                    }

                    else throw new RuntimeError("The parameter value must be string, number or boolean", activationParam.Colon);

                    if (Effect.GetCompiledEffect(stringName).Parameters.Contains(newParam)) parameter.Add(newParam, paramValue);

                    else throw new RuntimeError("Invalid parameter declaration, this effect does not contain this parameter", activationParam.VarName.Value.Location);
                }

                if (Effect.GetCompiledEffect(stringName).Parameters.Count != parameter.Count) throw new RuntimeError("Invalid parameter declaration, there are still parameters to declare", data.Effect.ActivationParams[0].Colon);
                Params = parameter;
            }

            else if (Effect.GetCompiledEffect(stringName).Parameters == null) Params = null;

            else throw new RuntimeError("Parameters must be declared", data.Effect.Colon);
        }

        else throw new RuntimeError("The effect must be declared before", data.Effect.Colon);

        if (data.Selector != null)
        {
            var source = Evaluate(data.Selector.Source.Source);

            if (source is string stringSource)
            {
                if (stringSource == "board" || stringSource == "hand" || stringSource == "otherHand" || stringSource == "deck" || stringSource == "otherDeck" || stringSource == "field" || stringSource == "otherField" || stringSource == "parent" && isRoot == false) selectorSource = stringSource;

                else throw new RuntimeError("Invalid source declaration", data.Selector.Source.Operator);
            }
            else throw new RuntimeError("The 'Source' must be a string value", data.Selector.Source.Operator);

            if(data.Selector.Single != null)
            {
                var single = Evaluate(data.Selector.Single.Single);

                if (single is bool booleanSingle) selectorSingle = booleanSingle;

                else throw new RuntimeError("The 'Single' must to be a boolean value", data.Selector.Single.Operator);
            }

            @delegate = new Delegate(data.Selector.Predicate.LambdaExpression);
        }

        else if (isRoot == false)
        {
            selectorSource = null;
            selectorSingle = false;
        }

        else throw new RuntimeError("The 'Selector' must be declared", data.Effect.Colon);

        if (data.PostAction != null)
        {
            object post = Visit(data.PostAction.LinkedEffect, false);
            if (post != null)
                postAction = (EffectActivation)post;
        }

        return new EffectActivation(effectName, Params, selectorSource, selectorSingle, @delegate, postAction);
    }
}