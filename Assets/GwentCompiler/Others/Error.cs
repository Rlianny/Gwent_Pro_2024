using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;


// clase que representa los tipos de error que pueden ocurrir en el proceso de compilación
public class Error
{
    public static List<Error> AllErrors = new();  // todos los errores detectados en tiempo de compilación
    public string Argument { get; private set; }        // argumento del error
    public CodeLocation ErrorLocation { get; private set; }     // ubicacion del error en el código fuente

    public Error(string message, CodeLocation errorLocation)
    {
        Argument = message;
        ErrorLocation = errorLocation;
    }

    public override string ToString()
    {
        return $"Error: {Argument} in row {ErrorLocation.Row}, column {ErrorLocation.Column}.";
    }
}

public class LexicalError : Error
{
    public LexicalError(string message, CodeLocation location) : base(message, location) { }
}
public class ParseError : Error
{
    public ParseError(string message, CodeLocation location) : base(message, location) { }
}
public class SemanticError : Error
{
    public SemanticError(string message, CodeLocation location) : base(message, location) { }
}



// interface que implementan los objetos que son capaces de detectar y reportar errores
public interface IErrorReporter
{
    protected static int lastColumn { get; set; }
    protected static int lastRow { get; set; }
    public bool hadError { get; set; }

    /// <summary>
    /// Este método genera un nuevo error.
    /// </summary>
    /// <remarks>
    /// Cada tipo que implemente IErrorReporter tendrá este método y lo implementará de manera diferente.
    /// </remarks>
    /// <param name="message"> Argumento para el tipo de error generado.</param>
    /// <param name="errorLocation"> Ubicación del error en el código fuente.</param>
    /// <returns>devuelve el token anterior al movimiento en la lista de Tokens.</returns>
    public void GenerateError(string message, CodeLocation errorLocation);

    /// <summary>
    /// Este método implementa la función de mostrar un error al usuario.
    /// </summary>
    public void Report(Error error);
}

public class RuntimeError : Exception
{
    public CodeLocation CodeLocation { get; private set; }
    public RuntimeError(string message, CodeLocation location) : base($"Error: {message} in row {location.Row}, column {location.Column}.")
    {
        CodeLocation = location;
    }
}
