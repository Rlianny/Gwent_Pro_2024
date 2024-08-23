
using UnityEngine;
using System;

public static class ASTPrinter
{
    public static void Print(IExpression expr, int idt = 0)
    {
        string space = "";
        for (int i = 0; i < 2 * idt; i++)
            space += " ";

        if (expr is BinaryExpression bin)
        {
            idt++;
            System.Console.WriteLine(space + bin.Operator.ToString());
            Print(bin.Left, idt);
            Print(bin.Right, idt);
        }

        else if (expr is UnaryExpression un)
        {
            idt++;
            System.Console.WriteLine(space + un.Operator.ToString());
            Print(un.Right, idt);
        }

        else if (expr is Atom atom)
        {
            System.Console.WriteLine("   " + space + atom.Value.ToString());
        }

        else if (expr is GroupExpression group)
        {
            idt++;
            Print(group.Expression, idt);
        }

        else if (expr is ContextAccessExpr accessExpr)
        {
            idt++;
            System.Console.WriteLine(space + ".");
            Console.WriteLine(accessExpr.Variable.Value.Lexeme);
            Console.WriteLine(accessExpr.Access.Lexeme);
        }

        else if (expr is ContextMethodsExpr call)
        {
            System.Console.WriteLine(space + "Call");
            Print(call.AccessExpression);
            Print(call.Args);
        }
    }

}
