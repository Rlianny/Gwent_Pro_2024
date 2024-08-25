using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;


[Serializable]
public class CompiledEffect : CompiledObject
{
    public string Name {get; private set;} 
    public string TargetsId {get; private set;}
    public string ContextId {get; private set;}
    public List<Parameter> Parameters {get; private set;} 
    public BlockStmt Block {get; private set;} 

    public CompiledEffect(string name, List<Parameter> parameters, string targets, string context, BlockStmt block)
    {
        Name = name;
        TargetsId = targets;
        ContextId = context;
        Parameters = parameters;
        Block = block;
    }

    public override string ToString()
    {
        string @params = "";

        if(Parameters != null)
        {
            foreach(var param in Parameters)
            {
                @params += $"{param.Name} : {param.Type}; ";
            }

        }

        return $"{Name}, {@params}";
    }
}
