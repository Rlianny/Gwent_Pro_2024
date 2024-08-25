using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public interface IStatement : IASTNode
{
   
}

public class ExpressionStmt : IStatement
{
    public IExpression Expression {get; private set;}

    public ExpressionStmt(IExpression expression)
    {
        Expression = expression;
    }
}

public class PrintStmt : IStatement
{
    public IExpression Expression {get; private set;}

    public PrintStmt(IExpression expression)
    {
        Expression = expression;
    }
}

public class BlockStmt : IStatement
{
    public List<IStatement> Statements {get; private set;}
    public BlockStmt(List<IStatement> statements)
    {
        Statements = statements;
    }
}

public class IfStmt : IStatement
{
    public IExpression Condition {get; private set;}
    public IStatement ThenBranch {get; private set;}
    public IStatement ElseBranch {get; private set;}

    public IfStmt(IExpression condition, IStatement thenBranch, IStatement elseBranch)
    {
        Condition = condition;
        ThenBranch = thenBranch;
        ElseBranch = elseBranch;
    }
}

public class WhileStmt : IStatement
{
    public IExpression Condition {get; private set;}
    public IStatement Body {get; private set;}

    public WhileStmt(IExpression condition, IStatement body)
    {
        Condition = condition;
        Body = body;
    }
}

public class ForStmt : IStatement
{
    public Variable Variable {get; private set;}
    public IExpression Collection {get; private set;}
    public BlockStmt Body {get; private set;}

    public ForStmt(Variable variable, IExpression collection, List<IStatement> body)
    {
        Variable = variable;
        Collection = collection;
        Body = new BlockStmt(body);
    }
}

