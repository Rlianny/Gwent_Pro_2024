using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

[Serializable]
public class EffectActivation
{
    public string EffectName { get; private set; }
    public Dictionary<Parameter, object> Parameters { get; private set; } = new();
    public string SelectorSource { get; private set; }
    public bool SelectorSingle { get; private set; }
    public Delegate SelectorPredicate { get; private set; }
    public EffectActivation PostAction { get; private set; }

    public EffectActivation(string name, Dictionary<Parameter, object> @params, string source, bool single, Delegate predicate, EffectActivation postAction)
    {
        EffectName = name;
        Parameters = @params;
        SelectorSource = source;
        SelectorSingle = single;
        SelectorPredicate = predicate;
        PostAction = postAction;
    }
}

public class Delegate
{
    public Variable Identifier { get; private set; }
    public IExpression Filter { get; private set; }
    private CodeLocation LambdaLocation { get; }

    public Delegate(LambdaExpr lambdaExpr)
    {
        Identifier = lambdaExpr.Variable;
        Filter = lambdaExpr.Filter;
        LambdaLocation = lambdaExpr.Lambda.Location;
    }

    public bool Invoke(Card card)
    {
        Interpreter interpreter = new();

        interpreter.Environment.Assign(Identifier.Value.Lexeme, card);

        var value = interpreter.Evaluate(Filter);

        if (value is bool boolValue) return boolValue;
        else throw new RuntimeError("Invalid Filter", LambdaLocation);
    }
}