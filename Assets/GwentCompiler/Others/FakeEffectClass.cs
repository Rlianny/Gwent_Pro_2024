using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class FakeEffect
{
    public static Dictionary<string, FakeEffect> AllEffects = new();

    public List<Parameter> Parameters = new();
    public string Name { get; set; }

    public FakeEffect(string name, List<Parameter> parameters)
    {
        Name = name;
        Parameters = parameters;
    }
}

public enum ValueType
{
    String, Number, Boolean, Card, CardList,
}