using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class EffectActivation
{
    public string EffectName { get; private set; }
    public Dictionary<Parameter, object> Parameters { get; private set; } = new();
    public string SelectorSource { get; private set; }
    public bool SelectorSingle { get; private set; }
    public Delegate SelectorPredicate { get; private set; }
    public EffectActivation PostAction { get; private set; }

    public EffectActivation(string name, Dictionary<Parameter, object> @params, string source, bool single, EffectActivation postAction)
    {
        EffectName = name;
        Parameters = @params;
        SelectorSource = source;
        SelectorSingle = single;
        // Predicate
        PostAction = postAction;
    }

    public void Execute()
    {

    }
}