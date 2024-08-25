using UnityEngine;
using System;
using System.IO;
using System.Text;
using System.Runtime.Serialization.Formatters.Binary;


[Serializable]
public abstract class CompiledObject
{
    public void Save()
    {
        FileFormatter.Save(this);
    }
}