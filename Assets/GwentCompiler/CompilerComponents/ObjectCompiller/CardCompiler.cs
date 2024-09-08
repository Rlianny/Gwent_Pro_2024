using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
public partial class ObjectCompiler
{
    private CompiledCard GetCompiledCard(CardDeclaration node)
    {
        Interpreter interpreter = new Interpreter();

        string cardName = null;
        if (node.Name != null && interpreter.Interpret(node.Name) is string stringName) cardName = stringName;

        string cardType = null;
        if (node.Type != null && interpreter.Interpret(node.Type) is string stringType) cardType = stringType;

        string cardFaction = null;
        if (node.Faction != null && interpreter.Interpret(node.Faction) is string stringFaction) cardFaction = stringFaction;

        List<string> range = null;
        if (node.Range != null && interpreter.Interpret(node.Range) is List<string> rangeList) range = rangeList;

        List<EffectActivation> onAct = null;
        if (node.OnActivationField == null) Debug.Log("Caso 1");
        if (interpreter.Interpret(node.OnActivationField) is not List<EffectActivation>)
        {
            if (interpreter.Interpret(node.OnActivationField) == null) Debug.Log(null);
        }
        if (node.OnActivationField != null && interpreter.Interpret(node.OnActivationField) is List<EffectActivation> activations) onAct = activations;

        string effectDescription = null;
        if (node.EffectDescription != null && interpreter.Interpret(node.EffectDescription) is string stringEffectDescription) effectDescription = stringEffectDescription;

        string characterDescription = null;
        if (node.CharacterDescription != null && interpreter.Interpret(node.CharacterDescription) is string stringCharacterDescription) characterDescription = stringCharacterDescription;

        string quote = null;
        if (node.Quote != null && interpreter.Interpret(node.Quote) is string stringQuote) quote = stringQuote;

        double? power = null;
        if (node.Power != null && interpreter.Interpret(node.Power) is double intPower) power = intPower;

        // if(cardType != null && cardName != null && cardFaction != null && range != null && onAct != null && effectDescription != null && power != null && characterDescription != null && quote != null) 
        // return new CompiledCard(cardType, "Morty " + cardName, cardFaction, range, onAct, effectDescription, (int)power, characterDescription, quote);

        if (cardType != null)
        {
            if (cardName != null)
            {
                if (cardFaction != null)
                {
                    if (range != null || cardType == "Líder")
                    {
                        if (onAct != null)
                        {
                            if (effectDescription != null)
                            {
                                if (power != null || cardType == "Líder")
                                {
                                    if (power == null) power = 0;
                                    if (characterDescription != null)
                                    {
                                        if (quote != null)
                                        {
                                            return new CompiledCard(cardType, "Morty " + cardName, cardFaction, range, onAct, effectDescription, (int)power, characterDescription, quote);
                                        }
                                        else throw new RuntimeError("Missing card quote field", node.CardLocation.Location);
                                    }
                                    else throw new RuntimeError("Missing card character description field", node.CardLocation.Location);
                                }
                                else throw new RuntimeError("Missing card power field", node.CardLocation.Location);
                            }
                            else throw new RuntimeError("Missing card effect description field", node.CardLocation.Location);
                        }
                        else throw new RuntimeError("Missing card OnActivation field", node.CardLocation.Location);
                    }
                    else throw new RuntimeError("Missing card range field", node.CardLocation.Location);
                }
                else throw new RuntimeError("Missing card faction field", node.CardLocation.Location);
            }
            else throw new RuntimeError("Missing card name field", node.CardLocation.Location);
        }
        else throw new RuntimeError("Missing card type field", node.CardLocation.Location);

    }

}