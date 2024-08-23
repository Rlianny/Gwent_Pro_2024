using UnityEngine;
using System;
using System.Collections.Generic;
using Unity.VisualScripting.Dependencies.NCalc;
public interface IExpression : IASTNode { }

public abstract class BinaryExpression : IExpression
{
    public IExpression Left { get; private set; }
    public IExpression Right { get; private set; }
    public Token Operator { get; private set; }

    public BinaryExpression(IExpression left, Token op, IExpression right)
    {
        Left = left;
        Right = right;
        Operator = op;
    }
}

public abstract class UnaryExpression : IExpression
{
    public IExpression Right { get; private set; }
    public Token Operator { get; private set; }

    public UnaryExpression(Token op, IExpression right)
    {
        Right = right;
        Operator = op;
    }
}

public class GroupExpression : IExpression
{
    public IExpression Expression { get; private set; }

    public GroupExpression(IExpression expr)
    {
        Expression = expr;
    }
}

public abstract class Atom : IExpression
{
    public Token Value { get; private set; }
    public Atom(Token value)
    {
        Value = value;
    }
}

public class Variable : IExpression
{
    public Token Value { get; private set; }
    public Variable(Token value)
    {
        Value = value;
    }
}

public class AssignmentExpr : IExpression
{
    public Variable Name { get; private set; }
    public IExpression Value { get; private set; }

    public AssignmentExpr(Variable name, IExpression value)
    {
        Name = name;
        Value = value;
    }
}

public class IncrementOrDecrementOperationExpr : IExpression
{
    public Variable Name { get; private set; }
    public Token Operation { get; private set; }

    public IncrementOrDecrementOperationExpr(Token op, Variable right)
    {
        Name = right;
        Operation = op;
    }
}

public abstract class ContextAccessExpr : IExpression
{
    public Token Dot { get; private set; }
    public Variable Variable { get; private set; }
    public Token Access { get; private set; }
    public IExpression Args { get; private set; }
    public bool SintacticSugar { get; private set; }

    public ContextAccessExpr(Variable variable, Token dot, Token access, IExpression args, bool sugar)
    {
        Dot = dot;
        Variable = variable;
        Access = access;
        Args = args;
        SintacticSugar = sugar;
    }
}

public abstract class ContextMethodsExpr : IExpression
{
    public IExpression AccessExpression { get; private set; }
    public Token Method { get; private set; }
    public IExpression Args { get; private set; }

    public ContextMethodsExpr(IExpression contextAccessExpr, Token method, IExpression args)
    {
        AccessExpression = contextAccessExpr;
        Method = method;
        Args = args;
    }
}

public class LambdaExpr : IExpression
{
    public Variable Variable { get; private set; }
    public Token Lambda { get; private set; }
    public IExpression Filter { get; private set; }

    public LambdaExpr(Variable variable, Token lambda, IExpression expression)
    {
        Variable = variable;
        Lambda = lambda;
        Filter = expression;
    }
}

#region BinaryExpressionSubtypes
public abstract class ArithmeticBinaryExpression : BinaryExpression
{
    public ArithmeticBinaryExpression(IExpression left, Token op, IExpression right) : base(left, op, right) { }
}

public abstract class BooleanOperationBinaryExpression : BinaryExpression
{
    public BooleanOperationBinaryExpression(IExpression left, Token op, IExpression right) : base(left, op, right) { }
}

public abstract class StringOperationBinaryExpression : BinaryExpression
{
    public StringOperationBinaryExpression(IExpression left, Token op, IExpression right) : base(left, op, right) { }
}

public class AndExpr : BooleanOperationBinaryExpression
{
    public AndExpr(IExpression left, Token op, IExpression right) : base(left, op, right) { }
}

public class OrExpr : BooleanOperationBinaryExpression
{
    public OrExpr(IExpression left, Token op, IExpression right) : base(left, op, right) { }
}

public class EqualityExpr : BooleanOperationBinaryExpression
{
    public EqualityExpr(IExpression left, Token op, IExpression right) : base(left, op, right) { }
}

public class InequalityExpr : BooleanOperationBinaryExpression
{
    public InequalityExpr(IExpression left, Token op, IExpression right) : base(left, op, right) { }
}

public class StringConcatenationExpr : StringOperationBinaryExpression
{
    public StringConcatenationExpr(IExpression left, Token op, IExpression right) : base(left, op, right) { }
}

public class StringConcatenationSpacedExpr : StringOperationBinaryExpression
{
    public StringConcatenationSpacedExpr(IExpression left, Token op, IExpression right) : base(left, op, right) { }
}

public class GreaterThanExpr : BooleanOperationBinaryExpression
{
    public GreaterThanExpr(IExpression left, Token op, IExpression right) : base(left, op, right) { }
}

public class GreaterThanOrEqualExpr : BooleanOperationBinaryExpression
{
    public GreaterThanOrEqualExpr(IExpression left, Token op, IExpression right) : base(left, op, right) { }
}

public class LessThanExpr : BooleanOperationBinaryExpression
{
    public LessThanExpr(IExpression left, Token op, IExpression right) : base(left, op, right) { }
}

public class LessThanOrEqualExpr : BooleanOperationBinaryExpression
{
    public LessThanOrEqualExpr(IExpression left, Token op, IExpression right) : base(left, op, right) { }
}

public class AdditionExpr : ArithmeticBinaryExpression
{
    public AdditionExpr(IExpression left, Token op, IExpression right) : base(left, op, right) { }
}

public class SubtractionExpr : ArithmeticBinaryExpression
{
    public SubtractionExpr(IExpression left, Token op, IExpression right) : base(left, op, right) { }
}

public class MultiplicationExpr : ArithmeticBinaryExpression
{
    public MultiplicationExpr(IExpression left, Token op, IExpression right) : base(left, op, right) { }
}

public class DivisionExpr : ArithmeticBinaryExpression
{
    public DivisionExpr(IExpression left, Token op, IExpression right) : base(left, op, right) { }
}

public class PowerExpr : ArithmeticBinaryExpression
{
    public PowerExpr(IExpression left, Token op, IExpression right) : base(left, op, right) { }
}

#endregion

#region UnaryExpressionSubtypes
public class NegatedExpr : UnaryExpression
{
    public NegatedExpr(Token op, IExpression right) : base(op, right) { }
}

public class Subtraction : UnaryExpression
{
    public Subtraction(Token op, IExpression right) : base(op, right) { }
}

#endregion

#region AtomicExpressionSubtypes
public class NumericLiteral : Atom
{
    public NumericLiteral(Token value) : base(value) { }
}

public class StringLiteral : Atom
{
    public StringLiteral(Token value) : base(value) { }
}

public class BooleanLiteral : Atom
{
    public BooleanLiteral(Token value) : base(value) { }
}

# endregion

#region ContextAccessExpressionSubtypes

public class CardPropertyAccessExpr : ContextAccessExpr
{
    public CardPropertyAccessExpr(Variable variable, Token dot, Token access, IExpression args, bool sugar) : base(variable, dot, access, args, sugar) { }
}

public class TriggerPlayerAccessExpr : ContextAccessExpr
{
    public TriggerPlayerAccessExpr(Variable variable, Token dot, Token access, IExpression args, bool sugar) : base(variable, dot, access, args, sugar) { }
}

public class BoardAccessExpr : ContextAccessExpr
{
    public BoardAccessExpr(Variable variable, Token dot, Token access, IExpression args, bool sugar) : base(variable, dot, access, args, sugar) { }
}

public class HandOfPlayerAccessExpr : ContextAccessExpr
{
    public HandOfPlayerAccessExpr(Variable variable, Token dot, Token access, IExpression args, bool sugar) : base(variable, dot, access, args, sugar) { }
}

public class FieldOfPlayerAccessExpr : ContextAccessExpr
{
    public FieldOfPlayerAccessExpr(Variable variable, Token dot, Token access, IExpression args, bool sugar) : base(variable, dot, access, args, sugar) { }
}

public class GraveyardOfPlayerAccessExpr : ContextAccessExpr
{
    public GraveyardOfPlayerAccessExpr(Variable variable, Token dot, Token access, IExpression args, bool sugar) : base(variable, dot, access, args, sugar) { }
}

public class DeckOfPlayerAccessExpr : ContextAccessExpr
{
    public DeckOfPlayerAccessExpr(Variable variable, Token dot, Token access, IExpression args, bool sugar) : base(variable, dot, access, args, sugar) { }
}

public class CardOwnerAccessExpr : ContextAccessExpr
{
    public CardOwnerAccessExpr(Variable variable, Token dot, Token access, IExpression args, bool sugar) : base(variable, dot, access, args, sugar) { }
}

#endregion

#region ContextMethodExpressionSubtypes

public class FindMethodExpr : ContextMethodsExpr
{
    public FindMethodExpr(IExpression access, Token method, IExpression args) : base(access, method, args) { }
}

public class PushMethodExpr : ContextMethodsExpr
{
    public PushMethodExpr(IExpression access, Token method, IExpression args) : base(access, method, args) { }
}

public class SendBottomMethodExpr : ContextMethodsExpr
{
    public SendBottomMethodExpr(IExpression access, Token method, IExpression args) : base(access, method, args) { }
}

public class PopMethodExpr : ContextMethodsExpr
{
    public PopMethodExpr(IExpression access, Token method, IExpression args) : base(access, method, args) { }
}

public class RemoveMethodExpr : ContextMethodsExpr
{
    public RemoveMethodExpr(IExpression access, Token method, IExpression args) : base(access, method, args) { }
}

public class ShuffleMethodExpr : ContextMethodsExpr
{
    public ShuffleMethodExpr(IExpression access, Token method, IExpression args) : base(access, method, args) { }
}

#endregion