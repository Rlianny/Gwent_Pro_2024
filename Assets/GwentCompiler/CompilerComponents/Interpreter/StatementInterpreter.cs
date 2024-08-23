using UnityEngine;
using System;
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
        ExecuteBlock(block, new Environment(environment));
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
        throw new NotImplementedException();
    }
}