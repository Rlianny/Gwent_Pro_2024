using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public partial class Parser
{
    private Token Consume(TokenSubtypes subtype, string message, List<TokenSubtypes> limit)
    {
        if (Check(subtype)) return Advance();

        GenerateError(message, new CodeLocation(Previous().Location.Row, Previous().Location.Column + Previous().Lexeme.Length));
        Synchronize(limit);
        return null;
    }

    private bool Match(TokenSubtypes type)
    {

        if (Check(type))
        {
            Advance();
            return true;
        }

        return false;
    }

    private bool Match(List<TokenSubtypes> Types)
    {
        foreach (TokenSubtypes subtype in Types)
        {
            if (Check(subtype))
            {
                Advance();
                return true;
            }
        }

        return false;

    }

    /// <summary>
    /// Este método verifica si el subtipo del Token actual en la lista de tokens coincide con un subtipo recibido como parámetro.
    /// </summary>
    /// <param name="tokenSubtype">subtipo con el que se desea comparar el subtipo del Token actual.</param>
    /// <returns>devuelve true si el subtipo coincide, false en el caso contrario.</returns>
    private bool Check(TokenSubtypes tokenSubtype)
    {
        if (IsAtEnd()) return false;
        return Peek().Subtype == tokenSubtype;
    }

    private bool Check(List<TokenSubtypes> Types)
    {
        foreach (TokenSubtypes subtype in Types)
        {
            if (Check(subtype))
            {
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// Este método se mueve al token siguiente en la lista de Tokens y lo devuelve.
    /// </summary>
    /// <remarks>
    /// En caso de que la lista de tokens haya llegado a su fin, el método no se moverá al token siguiente.
    /// </remarks>
    /// <returns>devuelve el token anterior al movimiento en la lista de Tokens.</returns>
    private Token Advance()
    {
        if (!IsAtEnd()) current++;
        return Previous();
    }

    /// <summary>
    /// Este método comprueba si se ha llegado al final de la lista de tokens.
    /// </summary>
    /// <returns>devuelve true si se ha llegado al final de la lista de tokens, false en el caso contrario.</returns>

    private bool IsAtEnd()
    {
        return Peek().Type == TokenTypes.EOF;
    }

    /// <summary>
    /// Este método devuelve el token actual de la lista de tokens.
    /// </summary>
    /// <returns>devuelve el token actual en la lista de Tokens.</returns>
    private Token Peek()
    {
        return tokens[current];
    }

    /// <summary>
    /// Este método devuelve el token anterior de la lista de tokens.
    /// </summary>
    /// <returns>devuelve el token anterior en la lista de Tokens.</returns>
    private Token Previous()
    {
        return tokens[current - 1];
    }

    public void GenerateError(string message, CodeLocation errorLocation)
    {
        ParseError newError = new ParseError(message, errorLocation);
        Error.AllErrors.Add(newError);
        Report(newError);
        hadError = true;
    }

    public void Report(Error error)
    {
        Debug.Log(error);
    }

    private void Synchronize(List<TokenSubtypes> synchronizer)
    {
        if (synchronizer == null) return;
        Advance();


        while (!IsAtEnd())
        {
            if (Check(synchronizer)) return;
            Advance();
        }
    }
}