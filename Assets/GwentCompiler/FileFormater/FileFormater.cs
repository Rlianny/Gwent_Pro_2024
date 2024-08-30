using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class FileFormatter
{
    public static void Save(CompiledObject @object)
    {
        string fileName = "";
        if (@object is CompiledCard compiledCard) fileName += compiledCard.Name;
        if (@object is CompiledEffect compiledEffect) fileName += compiledEffect.Name;
        fileName += $"{@object.GetHashCode()}.bin";
        string combinedPath = Path.Combine("Assets/CardsCollection/Serialized", fileName);

        BinaryFormatter binaryFormatter = new();
        Stream stream = new FileStream(combinedPath, FileMode.Create, FileAccess.Write, FileShare.None);

        try
        {
            binaryFormatter.Serialize(stream, @object);
        }
        catch (Exception ex)
        {
            Debug.Log("Serialization Error");
            Debug.Log(ex.ToString());
        }
        stream.Close();
        Debug.Log("Serializing...");
    }

    public static CompiledObject Load(string path)
    {
        BinaryFormatter binaryFormatter = new();
        Stream stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.None);
        CompiledObject loaded = null;

        try
        {
            loaded = (CompiledObject)binaryFormatter.Deserialize(stream);
        }
        catch (Exception ex)
        {
            Debug.Log("Deserialization Error");
            Debug.Log(ex.ToString());
        }

        stream.Close();
        Debug.Log("Deserializing...");
        return loaded;
    }
}
