using UnityEngine;
using System;

[Serializable]
public class Parameter
{
    public string Name { get; private set; } 
    public ValueType Type { get; private set; } 

    public Parameter(string name, ValueType type)
    {
        Name = name;
        Type = type;
    }
    public override bool Equals(object obj)
    {
        if (obj is Parameter param)
        {
            if (param.Name == this.Name && param.Type == this.Type) return true;
            else return false;
        }

        else return false;
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }
}