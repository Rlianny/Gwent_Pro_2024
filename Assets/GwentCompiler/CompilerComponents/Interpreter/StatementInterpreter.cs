using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
public partial class Interpreter : VisitorBase<object>
{
    public object Visit(ExpressionStmt expressionStatement)
    {
        Evaluate(expressionStatement.Expression);
        return null;
    }

    public object Visit(PrintStmt printStatement)
    {
        object value = Evaluate(printStatement.Expression);
        System.Console.WriteLine(Stringify(value));
        return null;
    }

    public object Visit(BlockStmt block)
    {
        ExecuteBlock(block, new Environment(Environment));
        return null;
    }

    public object Visit(IfStmt ifStatement)
    {
        if (IsTruthy(Evaluate(ifStatement.Condition))) Execute(ifStatement.ThenBranch);
        else if (ifStatement.ElseBranch != null) Execute(ifStatement.ElseBranch);
        return null;
    }

    public object Visit(WhileStmt whileStmt)
    {
        while (IsTruthy(Evaluate(whileStmt.Condition)))
        {
            Execute(whileStmt.Body);
        }

        return null;
    }

    public object Visit(ForStmt forStmt)
    {
        Environment previous = this.Environment;
        try
        {
            var collection = Evaluate(forStmt.Collection);

            if (collection is IEnumerable<object> enumerable)
            {
                var list = enumerable.ToList();
                foreach (var item in list)
                {
                    Environment.Assign(forStmt.Variable.Value.Lexeme, item);
                    Execute(forStmt.Body);
                }
            }
        }

        finally
        {
            this.Environment = previous;
        }
        
        return null;
    }
}