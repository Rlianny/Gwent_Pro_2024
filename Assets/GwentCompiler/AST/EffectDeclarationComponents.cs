using UnityEngine;
using System;
using System.Collections.Generic;
public interface IEffectComponent : IComponent {}

public class EffectParamsDeclaration : IEffectComponent
{
    public List<ParsedParam> Parameters {get; private set;}
    public EffectParamsDeclaration(List<ParsedParam> parameters)
    {
        Parameters = parameters;
    }
}

public class EffectAction : IEffectComponent
{
    public Variable TargetsId {get; private set;}
    public Variable ContextId {get; private set;}
    public IStatement BlockStmt {get; private set;}

    public EffectAction(Variable targets, Variable context, IStatement block)
    {
        TargetsId = targets;
        ContextId = context;
        BlockStmt = block;
    }
}
