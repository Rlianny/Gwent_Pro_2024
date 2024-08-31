using UnityEngine;
using System;
using System.Collections.Generic;

public partial class Interpreter : VisitorBase<object>
{
    public object Visit(AndExpr expr)
    {
        var left = Evaluate(expr.Left);
        var right = Evaluate(expr.Right);

        if (left is bool leftBool && right is bool rightBool) return leftBool && rightBool;
        else throw new RuntimeError("The operands must be boolean values", expr.Operator.Location);
    }

    public object Visit(OrExpr expr)
    {
        var left = Evaluate(expr.Left);
        var right = Evaluate(expr.Right);

        if (left is bool leftBool && right is bool rightBool) return leftBool || rightBool;
        else throw new RuntimeError("The operands must be boolean values", expr.Operator.Location);
    }

    public object Visit(EqualityExpr expr)
    {
        var left = Evaluate(expr.Left);
        var right = Evaluate(expr.Right);

        return left?.Equals(right);
    }

    public object Visit(InequalityExpr expr)
    {
        var left = Evaluate(expr.Left);
        var right = Evaluate(expr.Right);

        return !left?.Equals(right);
    }

    public object Visit(StringConcatenationExpr expr)
    {
        var left = Evaluate(expr.Left);
        var right = Evaluate(expr.Right);

        if (left is string leftString && right is string rightString) return leftString + rightString;
        else throw new RuntimeError("The operands must be string values", expr.Operator.Location);
    }

    public object Visit(StringConcatenationSpacedExpr expr)
    {
        var left = Evaluate(expr.Left);
        var right = Evaluate(expr.Right);

        if (left is string leftString && right is string rightString) return leftString + " " + rightString;
        else throw new RuntimeError("The operands must be string values", expr.Operator.Location);
    }

    public object Visit(GreaterThanExpr expr)
    {
        var left = Evaluate(expr.Left);
        var right = Evaluate(expr.Right);

        if (left is double leftInt && right is double rightInt) return leftInt > rightInt;
        else throw new RuntimeError("The operands must be numeric values", expr.Operator.Location);
    }

    public object Visit(GreaterThanOrEqualExpr expr)
    {
        var left = Evaluate(expr.Left);
        var right = Evaluate(expr.Right);

        if (left is double leftInt && right is double rightInt) return leftInt >= rightInt;
        else throw new RuntimeError("The operands must be numeric values", expr.Operator.Location);
    }

    public object Visit(LessThanExpr expr)
    {
        var left = Evaluate(expr.Left);
        var right = Evaluate(expr.Right);

        if (left is double leftInt && right is double rightInt) return leftInt < rightInt;
        else throw new RuntimeError("The operands must be numeric values", expr.Operator.Location);
    }

    public object Visit(LessThanOrEqualExpr expr)
    {
        var left = Evaluate(expr.Left);
        var right = Evaluate(expr.Right);

        if (left is double leftInt && right is double rightInt) return leftInt <= rightInt;
        else throw new RuntimeError("The operands must be numeric values", expr.Operator.Location);
    }

    public object Visit(AdditionExpr expr)
    {
        var left = Evaluate(expr.Left);
        var right = Evaluate(expr.Right);

        if (left is double leftInt && right is double rightInt) return leftInt + rightInt;
        else throw new RuntimeError("The operands must be numeric values", expr.Operator.Location);
    }

    public object Visit(SubtractionExpr expr)
    {
        var left = Evaluate(expr.Left);
        var right = Evaluate(expr.Right);

        if (left is double leftInt && right is double rightInt) return leftInt - rightInt;
        else throw new RuntimeError("The operands must be numeric values", expr.Operator.Location);
    }

    public object Visit(MultiplicationExpr expr)
    {
        var left = Evaluate(expr.Left);
        var right = Evaluate(expr.Right);

        if (left is double leftInt && right is double rightInt) return leftInt * rightInt;
        else throw new RuntimeError("The operands must be numeric values", expr.Operator.Location);
    }

    public object Visit(DivisionExpr expr)
    {
        var left = Evaluate(expr.Left);
        var right = Evaluate(expr.Right);

        if (left is double leftInt && right is double rightInt) return leftInt / rightInt;
        else throw new RuntimeError("The operands must be numeric values", expr.Operator.Location);
    }

    public object Visit(PowerExpr expr)
    {
        var left = Evaluate(expr.Left);
        var right = Evaluate(expr.Right);

        if (left is double leftInt && right is double rightInt) return Math.Pow(leftInt, rightInt);
        else throw new RuntimeError("The operands must be numeric values", expr.Operator.Location);
    }

    public object Visit(NegatedExpr expr)
    {
        var right = Evaluate(expr.Right);

        if (right is bool rightBool) return !rightBool;
        else throw new RuntimeError("The operand must be boolean value", expr.Operator.Location);
    }

    public object Visit(Subtraction subtraction)
    {
        var right = Evaluate(subtraction.Right);

        if (right is double rightInt) return -rightInt;
        else throw new RuntimeError("The operand must be a numeric value", subtraction.Operator.Location);
    }

    public object Visit(NumericLiteral literal)
    {
        if (literal.Value == null) return null;
        return double.Parse(literal.Value.Lexeme);
    }

    public object Visit(StringLiteral literal)
    {
        if (literal.Value == null) return null;
        return Stringify(literal.Value.Lexeme);
    }

    public object Visit(BooleanLiteral literal)
    {
        if (literal.Value == null) return null;
        return bool.Parse(literal.Value.Lexeme);
    }

    public object Visit(Variable variable)
    {
        return Environment.Get(variable.Value);
    }

    public object Visit(GroupExpression group)
    {
        return Evaluate(group.Expression);
    }

    public object Visit(AssignmentExpr expr)
    {
        var value = Evaluate(expr.Value);
        if (value == null || expr.Name == null) return null;

        if (expr.Name is Variable variable)
        {
            switch (expr.Assignment.Subtype)
            {
                case TokenSubtypes.Assignment:
                    Environment.Assign(variable.Value.Lexeme, value);

                    break;

                case TokenSubtypes.IncrementAssignment:
                    if (value is double doubleValue && Environment.Get(variable.Value) is double doubleVarValue)
                        Environment.Assign(variable.Value.Lexeme, doubleVarValue += doubleValue);
                    else throw new RuntimeError("The operands must be numeric values", expr.Assignment.Location);

                    break;

                case TokenSubtypes.DecrementAssignment:
                    if (value is double doubleValue1 && Environment.Get(variable.Value) is double doubleVarValue1)
                        Environment.Assign(variable.Value.Lexeme, doubleVarValue1 += doubleValue1);
                    else throw new RuntimeError("The operands must be numeric values", expr.Assignment.Location);

                    break;

                case TokenSubtypes.MultiplicativeAssignment:
                    if (value is double doubleValue2 && Environment.Get(variable.Value) is double doubleVarValue2)
                        Environment.Assign(variable.Value.Lexeme, doubleVarValue2 *= doubleValue2);
                    else throw new RuntimeError("The operands must be numeric values", expr.Assignment.Location);

                    break;

                case TokenSubtypes.DivisiveAssignment:
                    if (value is double doubleValue3 && Environment.Get(variable.Value) is double doubleVarValue3)
                        if (doubleVarValue3 != 0)
                            Environment.Assign(variable.Value.Lexeme, doubleVarValue3 /= doubleValue3);
                        else throw new RuntimeError("The value must be non cero", expr.Assignment.Location);

                    break;
            }
        }

        if (expr.Name is CardPropertyAccessExpr cardProperty)
        {
            Card card = (Card)Environment.Get(cardProperty.Variable.Value);

            switch (cardProperty.Access.Subtype)
            {
                case TokenSubtypes.Power:

                    if (value is double || value is int)
                    {
                        int power = ParsePowerToInt(value.ToString());

                        if (card is SilverUnityCard silverUnityCard)
                        {
                            switch (expr.Assignment.Subtype)
                            {
                                case TokenSubtypes.Assignment:
                                    silverUnityCard.ActualPower = power;
                                    break;

                                case TokenSubtypes.IncrementAssignment:
                                    silverUnityCard.ActualPower += power;
                                    break;

                                case TokenSubtypes.DecrementAssignment:
                                    silverUnityCard.ActualPower -= power;
                                    break;

                                case TokenSubtypes.DivisiveAssignment:
                                    silverUnityCard.ActualPower /= power;
                                    break;

                                case TokenSubtypes.MultiplicativeAssignment:
                                    silverUnityCard.ActualPower *= power;
                                    break;
                            }

                            if (silverUnityCard.ActualPower < 0)
                            {
                                silverUnityCard.ActualPower = 0;
                            }
                        }

                    }
                    else throw new RuntimeError("The assigned value must be numeric value", cardProperty.Access.Location);
                    break;
            }
        }

        return value;
    }

    public object Visit(IncrementOrDecrementOperationExpr expr)
    {
        if (expr.Name == null) return null;
        var value = Environment.Get(expr.Name.Value);

        if (value is double valueInt)
        {
            if (expr.Operation.Subtype == TokenSubtypes.PostDecrement) valueInt--;
            else valueInt++;
            Environment.Assign(expr.Name.Value.Lexeme, valueInt);
            return valueInt;
        }
        else throw new RuntimeError("The operand must be a numeric value", expr.Operation.Location);
    }

    public object Visit(TriggerPlayerAccessExpr expr)
    {
        return Context.TriggerPlayer;
    }

    public object Visit(BoardAccessExpr expr)
    {
        return Context.Board;
    }

    public object Visit(HandOfPlayerAccessExpr expr)
    {
        if (!expr.SintacticSugar)
        {
            var id = Evaluate(expr.Args);
            if (id is double doubleId)
            {
                return Context.HandOfPlayer((int)doubleId);
            }
            else throw new RuntimeError("Expect a numeric value as player ID", expr.Dot.Location);
        }
        else return Context.Hand;
    }

    public object Visit(FieldOfPlayerAccessExpr expr)
    {
        if (!expr.SintacticSugar)
        {
            var id = Evaluate(expr.Args);
            if (id is double doubleId)
            {
                return Context.FieldOfPlayer((int)doubleId);
            }
            else throw new RuntimeError("Expect a numeric value as player ID", expr.Dot.Location);
        }
        else return Context.Field;
    }

    public object Visit(GraveyardOfPlayerAccessExpr expr)
    {
        if (!expr.SintacticSugar)
        {
            var id = Evaluate(expr.Args);
            if (id is double doubleId)
            {
                return Context.GraveyardOfPlayer((int)doubleId);
            }
            else throw new RuntimeError("Expect a numeric value as player ID", expr.Dot.Location);
        }
        else return Context.Graveyard;
    }

    public object Visit(DeckOfPlayerAccessExpr expr)
    {
        if (!expr.SintacticSugar)
        {
            var id = Evaluate(expr.Args);
            if (id is double doubleId)
            {
                return Context.DeckOfPlayer((int)doubleId);
            }
            else throw new RuntimeError("Expect a numeric value as player ID", expr.Dot.Location);
        }
        else return Context.Deck;
    }

    public object Visit(FindMethodExpr expr)
    {
        var value = Evaluate(expr.AccessExpression);
        if (expr.Args is LambdaExpr lambdaExpr)
        {
            Delegate @delegate = new Delegate(lambdaExpr);
            List<Card> toReturn = new();
            if (value is List<Card> cardList)
            {
                foreach (Card card in cardList)
                {
                    if (@delegate.Invoke(card)) toReturn.Add(card);
                }
                return toReturn;
            }
            else if (value is List<UnityCard>[] cardArray)
            {
                foreach (var list in cardArray)
                {
                    foreach (UnityCard card in list)
                    {
                        if (@delegate.Invoke(card)) toReturn.Add(card);
                    }
                }
                return toReturn;
            }
            else throw new RuntimeError($"The method '{expr.Method.Lexeme}' is only accessible from card lists", expr.Method.Location);
        }
        else throw new RuntimeError($"The method '{expr.Method.Lexeme}' only accept as argument lambda expression", expr.Method.Location);
    }

    public object Visit(PushMethodExpr expr)
    {
        var value = Evaluate(expr.AccessExpression);
        if (value is List<Card> cardList)
        {
            var arg = Evaluate(expr.Args);
            if (arg is Card card)
            {
                cardList.Insert(0, card);
                return cardList;
            }
            else throw new RuntimeError("Expect a card as method argument", expr.Method.Location);
        }
        else if (value is List<UnityCard>[] cardArray) return new List<Card>();
        else throw new RuntimeError($"The method '{expr.Method.Lexeme}' is only accessible from card lists", expr.Method.Location);
    }

    public object Visit(SendBottomMethodExpr expr)
    {
        var value = Evaluate(expr.AccessExpression);
        if (value is List<Card> cardList)
        {
            var arg = Evaluate(expr.Args);
            if (arg is Card card)
            {
                cardList.Add(card);
                return cardList;
            }
            else throw new RuntimeError("Expect a card as method argument", expr.Method.Location);
        }
        else if (value is List<Card>[] cardArray) return new List<Card>();
        else throw new RuntimeError($"The method '{expr.Method.Lexeme}' is only accessible from card lists", expr.Method.Location);
    }

    public object Visit(PopMethodExpr expr)
    {
        var value = Evaluate(expr.AccessExpression);
        if (value is List<Card> cardList)
        {
            if (expr.Args == null)
            {
                if (cardList.Count > 0)
                {
                    Card card = cardList[0];
                    cardList.RemoveAt(0);
                    return card;
                }
                else return null;
            }
            else throw new RuntimeError($"No overload for method '{expr.Method.Lexeme}' takes 1 arguments", expr.Method.Location);
        }
        else if (value is List<UnityCard>[] cardArray) return new List<Card>();
        else throw new RuntimeError($"The method '{expr.Method.Lexeme}' is only accessible from card lists", expr.Method.Location);
    }

    public object Visit(RemoveMethodExpr expr)
    {
        var value = Evaluate(expr.AccessExpression);
        if (value is List<Card> cardList)
        {
            var arg = Evaluate(expr.Args);
            if (arg is Card card)
            {
                return cardList.Remove(card);
            }
            else throw new RuntimeError("Expect a card as method argument", expr.Method.Location);
        }
        else if (value is List<UnityCard>[] cardArray)
        {
            var arg = Evaluate(expr.Args);
            bool answer = false;
            if (arg is Card card)
            {
                if (card is UnityCard unityCard)
                    foreach (var list in cardArray)
                    {
                        if (list.Remove(unityCard)) answer = true;
                    }
                return answer;
            }
            else throw new RuntimeError("Expect a card as method argument", expr.Method.Location);
        }
        else throw new RuntimeError($"The method '{expr.Method.Lexeme}' is only accessible from card lists", expr.Method.Location);
    }

    public object Visit(ShuffleMethodExpr expr)
    {
        var value = Evaluate(expr.AccessExpression);
        if (value is List<Card> cardList)
        {
            Context.Shuffle(cardList);
            return cardList;
        }
        else if (value is List<UnityCard>[] cardArray) return new List<Card>();
        else throw new RuntimeError($"The method '{expr.Method.Lexeme}' is only accessible from card lists", expr.Method.Location);
    }

    public object Visit(CardPropertyAccessExpr expr)
    {
        var variable = Environment.Get(expr.Variable.Value);
        if (variable is Card card)
        {
            switch (expr.Access.Subtype)
            {
                case TokenSubtypes.Type:
                    return card.Type;

                case TokenSubtypes.Name:
                    return card.Name;

                case TokenSubtypes.Faction:
                    return card.Faction;

                case TokenSubtypes.Power:
                    if (card is HeroCard heroCard) return (double)heroCard.Power;
                    else if (card is SilverUnityCard silverUnityCard) return (double)silverUnityCard.ActualPower;
                    else throw new RuntimeError("This card does not have an accessible 'power' property", expr.Access.Location);

                default:
                    throw new RuntimeError($"Card does not contain a definition for the '{expr.Access.Lexeme}' access type", expr.Access.Location);
            }
        }
        throw new RuntimeError($"The variable '{expr.Variable.Value.Lexeme}' must be a Card", expr.Variable.Value.Location);

    }
}