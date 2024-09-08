using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections;

[Serializable]
public class CompiledCard : CompiledObject
{
    public string Type { get; private set; }
    public string Name { get; private set; }
    public string Faction { get; private set; }
    public List<string> Range { get; private set; }
    public string EffectDescription { get; private set; }
    public List<EffectActivation> OnActivation { get; private set; }
    public int Power { get; private set; }
    public string CharacterDescription { get; private set; }
    public string Quote { get; private set; }

    public CompiledCard(string type, string name, string faction, List<string> range, List<EffectActivation> onActivation, string effectDescription, int effectNumber, string characterDescription, string quote)
    {
        Type = type;
        Name = name;
        Faction = faction;
        Range = range;
        EffectDescription = effectDescription;
        OnActivation = onActivation;
        Power = effectNumber;
        CharacterDescription = characterDescription;
        Quote = quote;
    }
    public override string ToString()
    {
        string ranges = "";
        string onActivation = "";
        
        if(Range != null)
        foreach (string range in Range)
        {
            ranges += range + " ";
        }

        foreach (var act in OnActivation)
        {
            onActivation += $"{act.EffectName}, Parameters:";

            if (act.Parameters != null)
                foreach (var parm in act.Parameters)
                {
                    onActivation += $"{parm.Key.Name} : {parm.Value} ;";
                }

            onActivation += $"{act.SelectorSource}, {act.SelectorSingle}, ";
        }

        return $"{Type}, {Name}, {Faction}, {ranges}, {onActivation} {EffectDescription}, {Power}, {CharacterDescription}, {Quote}";
    }

}