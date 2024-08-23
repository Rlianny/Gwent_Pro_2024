using System.Text.RegularExpressions; // Regex 
using System.Text; //NormalizationForm
using UnityEngine;
using System;

public class CharacterTools
{
    private static string RemoveAccentsAndPunctuations(string inputString) //
    {
        return Regex.Replace(inputString.Normalize(NormalizationForm.FormD), @"[^a-zA-z0-9 ]+", " "); 
    }

    public static string[] TxtProcessor(string inputString) 
    {
        inputString = inputString.ToLower();
        inputString = RemoveAccentsAndPunctuations(inputString); 
        string[] words = inputString.Split(' ', StringSplitOptions.RemoveEmptyEntries);

        return words;
    }

    public static string Transform(string value)
    {
        char[] guide = { '\r', '\n', '(', ')', '*', '{', '}', '´', '`' ,',','.',':'};
        foreach (char a in guide)
        {
            value = value.Replace(a, ' ');
        }
        return value;
    }
}