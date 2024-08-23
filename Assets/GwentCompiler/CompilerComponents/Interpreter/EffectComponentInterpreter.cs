using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public partial class Interpreter : VisitorBase<object>
{
    public object Visit(EffectParamsDeclaration paramsDeclaration)
    {
        List<Parameter> parameters = new();

        foreach(var parsedParam in paramsDeclaration.Parameters)
        {
            if(parsedParam.Type == null) throw new RuntimeError("The parameter type must be 'Number', 'Bool' or 'String'", parsedParam.VarName.Value.Location);
            
            switch(parsedParam.Type.Subtype)
            {
                case TokenSubtypes.String:
                parameters.Add(new Parameter(parsedParam.VarName.Value.Lexeme, ValueType.String));
                continue;

                case TokenSubtypes.Number:
                parameters.Add(new Parameter(parsedParam.VarName.Value.Lexeme, ValueType.Number));
                continue;

                case TokenSubtypes.Bool:
                parameters.Add(new Parameter(parsedParam.VarName.Value.Lexeme, ValueType.Boolean));
                continue;

                default: throw new RuntimeError("The parameter type must be 'Number', 'Bool' or 'String'", parsedParam.VarName.Value.Location);
            }
        }

        return parameters;
    }

    public object Visit(EffectAction action)
    {
        throw new NotImplementedException();
    }
}