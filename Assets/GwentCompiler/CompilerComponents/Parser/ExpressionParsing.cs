using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public partial class Parser
{
    #region Expressions Parsing

    // *Expression* -> Assignment;
    private IExpression Expression()
    {
        return Assignment();
    }

    // *Assignment* -> LogicOperation ("=" Assignment);
    private IExpression Assignment()
    {
        IExpression expr = LogicOperation();

        if (Match(TokenSubtypes.Assignment))
        {
            Token equal = Previous();
            IExpression value = Assignment();
            if (expr is Variable variable)
            {
                return new AssignmentExpr(variable, value);
            }
            // if(expr is CardPropertyAccessExpr cardProperty)
            // {
            //     return new AssignmentExpr(cardProperty, value);
            // }

            GenerateError("Assignment objective non valid.", equal.Location);
        }

        return expr;
    }

    // *LogicOperation* -> Equality ("And" Equality| "Or" Equality)*;
    private IExpression LogicOperation()
    {
        IExpression expr = Equality();

        while (Match(new List<TokenSubtypes>() { TokenSubtypes.AND, TokenSubtypes.OR }))
        {
            Token op = Previous();
            IExpression right = Equality();

            switch (op.Subtype)
            {
                case TokenSubtypes.AND:
                    expr = new AndExpr(expr, op, right);
                    break;

                case TokenSubtypes.OR:
                    expr = new OrExpr(expr, op, right);
                    break;
            }
        }

        return expr;
    }

    // *Equality* = StringOperation ("!=" StringOperation | "==" StingOperation)*;
    private IExpression Equality()
    {
        IExpression expr = StringOperation();

        while (Match(new List<TokenSubtypes>() { TokenSubtypes.Inequality, TokenSubtypes.Equality }))
        {
            Token op = Previous();
            IExpression right = StringOperation();

            switch (op.Subtype)
            {
                case TokenSubtypes.Equality:
                    expr = new EqualityExpr(expr, op, right);
                    break;

                case TokenSubtypes.Inequality:
                    expr = new InequalityExpr(expr, op, right);
                    break;
            }
        }

        return expr;
    }

    // *StringOperation* -> Comparison ("@" Comparison | "@@" Comparison)*;

    private IExpression StringOperation()
    {
        IExpression expr = Comparison();

        while (Match(new List<TokenSubtypes>() { TokenSubtypes.StringConcatenation, TokenSubtypes.StringConcatenationSpaced }))
        {
            Token op = Previous();
            IExpression right = Comparison();

            switch (op.Subtype)
            {
                case TokenSubtypes.StringConcatenation:
                    expr = new StringConcatenationExpr(expr, op, right);
                    break;

                case TokenSubtypes.StringConcatenationSpaced:
                    expr = new StringConcatenationSpacedExpr(expr, op, right);
                    break;
            }
        }

        return expr;
    }

    // *Comparison* -> Term (">" Term | ">=" Term | "<" Term | "<=" Term)*;

    private IExpression Comparison()
    {
        IExpression expr = Term();

        while (Match(new List<TokenSubtypes>() { TokenSubtypes.GreaterThan, TokenSubtypes.GreaterThanOrEqual, TokenSubtypes.LessThan, TokenSubtypes.LessThanOrEqual }))
        {
            Token op = Previous();
            IExpression right = Term();

            switch (op.Subtype)
            {
                case TokenSubtypes.GreaterThan:
                    expr = new GreaterThanExpr(expr, op, right);
                    break;

                case TokenSubtypes.GreaterThanOrEqual:
                    expr = new GreaterThanOrEqualExpr(expr, op, right);
                    break;

                case TokenSubtypes.LessThan:
                    expr = new LessThanExpr(expr, op, right);
                    break;

                case TokenSubtypes.LessThanOrEqual:
                    expr = new LessThanOrEqualExpr(expr, op, right);
                    break;
            }
        }

        return expr;
    }

    // *Term* -> Factor ("+" Factor | "-" Factor)*;
    private IExpression Term()
    {
        IExpression expr = Factor();

        while (Match(new List<TokenSubtypes>() { TokenSubtypes.Addition, TokenSubtypes.Subtraction }))
        {
            Token op = Previous();
            IExpression right = Factor();

            switch (op.Subtype)
            {
                case TokenSubtypes.Addition:
                    expr = new AdditionExpr(expr, op, right);
                    break;

                case TokenSubtypes.Subtraction:
                    expr = new SubtractionExpr(expr, op, right);
                    break;
            }
        }

        return expr;
    }

    // *Factor* -> Power ("*" Power | "/" Power)*;
    private IExpression Factor()
    {
        IExpression expr = Power();

        while (Match(new List<TokenSubtypes>() { TokenSubtypes.Multiplication, TokenSubtypes.Division }))
        {
            Token op = Previous();
            IExpression right = Power();

            switch (op.Subtype)
            {
                case TokenSubtypes.Multiplication:
                    expr = new MultiplicationExpr(expr, op, right);
                    break;

                case TokenSubtypes.Division:
                    expr = new DivisionExpr(expr, op, right);
                    break;
            }
        }

        return expr;
    }

    // *Power* -> Unary ("^" Power)*;
    private IExpression Power()
    {
        IExpression expr = Unary();

        while (Match(TokenSubtypes.Potentiation))
        {
            Token op = Previous();
            IExpression right = Power();
            expr = new PowerExpr(expr, op, right);
        }

        return expr;
    }

    // *Unary* -> ("-" | "!") Primary;
    private IExpression Unary()
    {
        if (Match(new List<TokenSubtypes>() { TokenSubtypes.Subtraction, TokenSubtypes.Negation }))
        {
            Token op = Previous();
            IExpression right = Primary();

            switch (op.Subtype)
            {
                case TokenSubtypes.Subtraction:
                    return new Subtraction(op, right);

                case TokenSubtypes.Negation:
                    return new NegatedExpr(op, right);
            }
        }

        return Primary();
    }

    // *Primary* -> BoolLiteral | NumericLiteral | StringLiteral | Access | MethodCalled;
    private IExpression Primary()
    {
        if (Match(TokenSubtypes.True) || Match(TokenSubtypes.False)) return new BooleanLiteral(Previous());

        if (Match(TokenSubtypes.NumericLiteral)) return new NumericLiteral(Previous());

        if (Match(TokenSubtypes.StringLiteral)) return new StringLiteral(Previous());

        if (Match(TokenSubtypes.OpenParenthesis))
        {
            Token open = Previous();
            IExpression expr = Expression();
            Token close = Consume(TokenSubtypes.CloseParenthesis, ") expected after expression.", null);

            if (close == null) return null;
            expr = new GroupExpression(expr);

            if (expr is GroupExpression group && group.Expression is Variable var)
            {
                if (Match(TokenSubtypes.Lambda))
                {
                    Token lambda = Previous();
                    IExpression filter = Expression();
                    return new LambdaExpr(var, lambda, filter);
                }

                else
                {
                    GenerateError("Invalid group expression", open.Location);
                }
            }
            return expr;
        }

        if (Match(TokenSubtypes.Identifier))
        {
            Variable var = new Variable(Previous());

            if (Match(new List<TokenSubtypes>() { TokenSubtypes.PostIncrement, TokenSubtypes.PostDecrement }))
            {
                Token op = Previous();
                return new IncrementOrDecrementOperationExpr(op, var);
            }

            else if (Match(TokenSubtypes.Dot))
            {
                Token dot = Previous();

                if (Match(new List<TokenSubtypes>() { TokenSubtypes.TriggerPlayer, TokenSubtypes.Board, TokenSubtypes.HandOfPlayer, TokenSubtypes.FieldOfPlayer, TokenSubtypes.GraveyardOfPlayer, TokenSubtypes.DeckOfPlayer, TokenSubtypes.Owner, TokenSubtypes.Type, TokenSubtypes.Name, TokenSubtypes.Faction, TokenSubtypes.Power, TokenSubtypes.Hand, TokenSubtypes.Deck, TokenSubtypes.Graveyard, TokenSubtypes.Field }))
                {
                    Token access = Previous();
                    IExpression args = null;


                    if (access.Subtype == TokenSubtypes.HandOfPlayer || access.Subtype == TokenSubtypes.FieldOfPlayer || access.Subtype == TokenSubtypes.GraveyardOfPlayer || access.Subtype == TokenSubtypes.DeckOfPlayer)
                    {
                        if (Match(TokenSubtypes.OpenParenthesis))
                        {
                            args = Expression();
                            Consume(TokenSubtypes.CloseParenthesis, "Expect ')' after methods arguments", null);
                        }
                    }

                    ContextAccessExpr expr = null;

                    switch (access.Subtype)
                    {
                        case TokenSubtypes.TriggerPlayer:
                            expr = new TriggerPlayerAccessExpr(var, dot, access, args, false);
                            break;

                        case TokenSubtypes.Board:
                            expr = new BoardAccessExpr(var, dot, access, args, false);
                            break;

                        case TokenSubtypes.HandOfPlayer:
                            expr = new HandOfPlayerAccessExpr(var, dot, access, args, false);
                            break;

                        case TokenSubtypes.FieldOfPlayer:
                            expr = new FieldOfPlayerAccessExpr(var, dot, access, args, false);
                            break;

                        case TokenSubtypes.GraveyardOfPlayer:
                            expr = new GraveyardOfPlayerAccessExpr(var, dot, access, args, false);
                            break;

                        case TokenSubtypes.DeckOfPlayer:
                            expr = new DeckOfPlayerAccessExpr(var, dot, access, args, false);
                            break;

                        case TokenSubtypes.Owner:
                            expr = new CardOwnerAccessExpr(var, dot, access, args, false);
                            break;

                        case TokenSubtypes.Hand:
                            expr = new HandOfPlayerAccessExpr(var, dot, access, args, true);
                            break;

                        case TokenSubtypes.Deck:
                            expr = new DeckOfPlayerAccessExpr(var, dot, access, args, true);
                            break;

                        case TokenSubtypes.Graveyard:
                            expr = new GraveyardOfPlayerAccessExpr(var, dot, access, args, true);
                            break;

                        case TokenSubtypes.Field:
                            expr = new FieldOfPlayerAccessExpr(var, dot, access, args, true);
                            break;

                        case TokenSubtypes.Type:
                            expr = new CardPropertyAccessExpr(var, dot, access, args, false);
                            break;

                        case TokenSubtypes.Name:
                            expr = new CardPropertyAccessExpr(var, dot, access, args, false);
                            break;

                        case TokenSubtypes.Faction:
                            expr = new CardPropertyAccessExpr(var, dot, access, args, false);
                            break;

                        case TokenSubtypes.Power:
                            expr = new CardPropertyAccessExpr(var, dot, access, args, false);
                            break;
                    }

                    if (Match(TokenSubtypes.Dot))
                    {
                        return MethodCall(expr);
                    }

                    return expr;
                }


                return var;
                //else GenerateError("Expression expected", Peek().Location); // Si ninguno de los casos coincide, significa que estamos sentados sobre un token que no puede iniciar una expresión
                //Synchronize(new List<TokenSubtypes> { TokenSubtypes.Semicolon, TokenSubtypes.Comma, TokenSubtypes.CloseBrace});

            }

            return var;
        }

        else GenerateError("Expression expected", Peek().Location); // Si ninguno de los casos coincide, significa que estamos sentados sobre un token que no puede iniciar una expresión
        return null;
    }

    private IExpression MethodCall(IExpression access)
    {

        if (Check(TokenSubtypes.Dot))
        {
            GenerateError("Invalid Access", Peek().Location);
            return null;
        }

        if (Match(TokenSubtypes.Find))
        {
            Token method = Previous();
            Consume(TokenSubtypes.OpenParenthesis, "Expected '('", new List<TokenSubtypes> { TokenSubtypes.CloseParenthesis });
            IExpression args = Expression();
            Consume(TokenSubtypes.CloseParenthesis, "Expected ')'", null);
            return new FindMethodExpr(access, method, args);
        }

        if (Match(TokenSubtypes.Push))
        {
            Token method = Previous();
            Consume(TokenSubtypes.OpenParenthesis, "Expected '('", new List<TokenSubtypes> { TokenSubtypes.CloseParenthesis });
            IExpression args = Expression();
            Consume(TokenSubtypes.CloseParenthesis, "Expected ')'", null);
            return new PushMethodExpr(access, method, args);
        }

        if (Match(TokenSubtypes.SendBottom))
        {
            Token method = Previous();
            Consume(TokenSubtypes.OpenParenthesis, "Expected '('", new List<TokenSubtypes> { TokenSubtypes.CloseParenthesis });
            IExpression args = Expression();
            Consume(TokenSubtypes.CloseParenthesis, "Expected ')'", null);
            return new SendBottomMethodExpr(access, method, args);
        }

        if (Match(TokenSubtypes.Pop))
        {
            Token method = Previous();
            Consume(TokenSubtypes.OpenParenthesis, "Expected '('", new List<TokenSubtypes> { TokenSubtypes.CloseParenthesis });
            Consume(TokenSubtypes.CloseParenthesis, "Expected ')'", null);
            return new PopMethodExpr(access, method, null);
        }

        if (Match(TokenSubtypes.Remove))
        {
            Token method = Previous();
            Consume(TokenSubtypes.OpenParenthesis, "Expected '('", new List<TokenSubtypes> { TokenSubtypes.CloseParenthesis });
            IExpression args = Expression();
            Consume(TokenSubtypes.CloseParenthesis, "Expected ')'", null);
            return new RemoveMethodExpr(access, method, args);
        }

        if (Match(TokenSubtypes.Shuffle))
        {
            Token method = Previous();
            Consume(TokenSubtypes.OpenParenthesis, "Expected '('", new List<TokenSubtypes> { TokenSubtypes.CloseParenthesis });
            Consume(TokenSubtypes.CloseParenthesis, "Expected ')'", null);
            return new ShuffleMethodExpr(access, method, null);
        }

        else GenerateError("Expression expected", Peek().Location);
        return null;
    }

    #endregion
}