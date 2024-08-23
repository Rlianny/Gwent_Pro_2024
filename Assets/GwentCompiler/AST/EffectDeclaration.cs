using UnityEngine;
using System;

public class EffectDeclaration: IProgramNode
{
    public NameDeclaration Name { get; private set; }
    public EffectParamsDeclaration Params {get; private set;}
    public EffectAction Action { get; private set;}
    public Token EffectLocation { get; private set; }

    public EffectDeclaration(Token effectToken) 
    {
        EffectLocation = effectToken;
    }

    public bool SetComponent(IEffectComponent component)
    {
        if (component == null) return true;
        
        switch (component.GetType().Name)
        {
            case "NameDeclaration":

                if (Name == null)
                {
                    Name = (NameDeclaration)component;
                    return true;
                }
                return false;

            case "EffectParamsDeclaration":

                if (Params == null)
                {
                    Params = (EffectParamsDeclaration)component;
                    return true;
                }
                return false;

            case "EffectAction":

                if (Action == null)
                {
                    Action = (EffectAction)component;
                    return true;
                }
                return false;
        }
        return false;
    }
}