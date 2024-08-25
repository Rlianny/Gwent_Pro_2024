using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public partial class Interpreter : VisitorBase<object>
{
    public Environment Environment = new Environment();

    #region Principal Methods

    public void Interpret(List<IStatement> statements)
    {
        try
        {
            foreach (var statement in statements)
            {
                Execute(statement);
            }
        }
        catch (RuntimeError ex)
        {
            GenerateError(ex.Message, ex.CodeLocation);
        }
    }

    public object Interpret(IComponent component)
    {
        try
        {
            return VisitBase(component);
        }
        catch (RuntimeError ex)
        {
            GenerateError(ex.Message, ex.CodeLocation);
        }
        return null;
    }

    public object Evaluate(IExpression expression)
    {
        return VisitBase(expression);
    }

    public void Execute(IStatement statement)
    {
        VisitBase(statement);
    }

    private void ExecuteBlock(BlockStmt block, Environment environment)
    {
        Environment previous = this.Environment;
        try
        {
            this.Environment = environment;
            foreach (var stmt in block.Statements)
            {
                Execute(stmt);
            }
        }
        finally
        {
            this.Environment = previous;
        }
    }
    #endregion

    #region Auxiliary Methods

    private string Stringify(object obj)
    {
        if (obj == null) return "null";

        string text = obj.ToString();
        if (text == null) return "null";

        if (text.EndsWith(".0")) text = text[..(text.Length - 2)];

        if (text[0] == '"' && text[text.Length - 1] == '"') text = text[1..(text.Length - 1)];

        return text;
    }

    private bool IsTruthy(object obj)
    {
        if (obj == null) return false;

        if (bool.TryParse(obj.ToString(), out bool value)) return value;
        return true;
    }

    #endregion
}