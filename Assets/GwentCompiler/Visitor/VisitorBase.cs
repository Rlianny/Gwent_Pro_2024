using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public abstract class VisitorBase<TResult> : IVisitor<TResult>, IErrorReporter
{
    public bool hadError { get; set; }

    public void GenerateError(string message, CodeLocation errorLocation)
    {
        SemanticError newError = new SemanticError(message, errorLocation);
        Error.AllErrors.Add(newError);
        Report(newError);
        hadError = true;
    }

    public void Report(Error error)
    {
        if (CompilerOutput.compilerOutput != null)
            CompilerOutput.compilerOutput.Report(error.ToString());
        Debug.Log(error);
    }

    /// <summary>
    /// Visits an expression by dynamically invoking the appropriate Visit method with additional parameters.
    /// </summary>
    /// <param name="node">The expression to visit.</param>
    /// <param name="additionalParams">Additional parameters to pass to the Visit method.</param>
    /// <returns>The result of the Visit method or null if no matching method is found.</returns>
    public virtual TResult VisitBase(IASTNode node, params object[] additionalParams)
    {
        if (node == null) return default;

        // Combine the expression type with the types of additional parameters
        var parameterTypes = new[] { node.GetType() }
            .Concat(additionalParams.Select(p => p.GetType()))
            .ToArray();

        // Find a Visit method in this class that matches the parameter types
        var method = GetType().GetMethod("Visit", parameterTypes);

        if (method != null)
        {
            // If a matching Visit method is found, invoke it with the expression and additional parameters
            try
            {
                var parameters = new[] { node }.Concat(additionalParams).ToArray();
                return (TResult)method.Invoke(this, parameters);
            }
            catch (System.Reflection.TargetInvocationException ex)
            {
                hadError = true;
                if (ex.InnerException is RuntimeError runtimeError)
                    throw runtimeError; // Reenvía la excepción para manejarla en un nivel superior
                else
                    Console.WriteLine(ex.InnerException?.Message);
            }
        }

        // If no matching Visit method is found, return default value (null for reference types)
        return default;
    }
}