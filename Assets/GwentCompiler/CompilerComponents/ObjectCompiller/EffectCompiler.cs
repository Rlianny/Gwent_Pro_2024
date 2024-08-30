using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public partial class ObjectCompiler
{
    private CompiledEffect GetCompiledEffect(EffectDeclaration node)
    {
        Interpreter interpreter = new Interpreter();
        string effectName = null;
        if(node.Name != null && interpreter.Interpret(node.Name) is string stringName) effectName = stringName;

        List<Parameter> parameters = null;
        if(node.Params != null && interpreter.Interpret(node.Params) is List<Parameter> paramList) parameters = paramList;

        IStatement block = null;
        if(node.Action != null) block = node.Action.BlockStmt;

        if(effectName != null && block != null)
        return new CompiledEffect(effectName, parameters, node.Action.TargetsId.Value.Lexeme, node.Action.ContextId.Value.Lexeme, block);
        else throw new RuntimeError("Missing effect fields", node.EffectLocation.Location);


    }
}