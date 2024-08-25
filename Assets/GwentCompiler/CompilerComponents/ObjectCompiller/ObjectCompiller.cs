using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public partial class ObjectCompiler : VisitorBase<object>
{
    private List<IProgramNode> nodes;
    public ObjectCompiler(List<IProgramNode> nodes)
    {
        this.nodes = nodes;
    }

    public List<CompiledObject> CompileObjects()
    {
        List<CompiledObject> compiledObjects = new();

        foreach (var node in nodes)
        {
            if (node is CardDeclaration cardDeclaration)
            {
                try
                {
                    compiledObjects.Add(GetCompiledCard(cardDeclaration));
                }
                catch (RuntimeError ex)
                {
                    hadError = true;
                    Debug.Log(ex.Message);
                }
            }
            if (node is EffectDeclaration effectDeclaration)
            {
                try
                {
                    compiledObjects.Add(GetCompiledEffect(effectDeclaration));
                }
                catch (RuntimeError ex)
                {
                    hadError = true;
                    Debug.Log(ex.Message);
                }
            }
        }

        return compiledObjects;
    }
}