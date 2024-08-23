using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;


public partial class Parser : IErrorReporter
{
    private List<Token> tokens = new();
    private int current;
    public bool hadError { get; set; }

    public Parser(List<Token> tokens)
    {
        this.tokens = tokens;
        current = 0;
    }

    public List<IStatement> Parse()
    {
        List<IStatement> statements = new();
        while (!IsAtEnd())
        {
            statements.Add(Statement());
        }
        return statements;
    }

    public List<IProgramNode> Program()
    {
        List<IProgramNode> programNodes = new();

        while(!IsAtEnd())
        {
            if(Match(TokenSubtypes.card))
            {
                programNodes.Add(Card());
                continue;
            }

            if(Match(TokenSubtypes.effect))
            {
                programNodes.Add(Effect());
                continue;
            }

            GenerateError("Invalid declaration type, expect card or effect", Peek().Location);
            Synchronize(new List<TokenSubtypes>{TokenSubtypes.card, TokenSubtypes.effect});
            break;
        }

        return programNodes;
    }

}