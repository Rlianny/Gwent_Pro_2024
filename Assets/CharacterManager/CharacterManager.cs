﻿using UnityEngine;
using System;

public static class CharacterManager
{
    static DataFolder Content;
    public static bool Initied = false;
    public static void Init()
    {
        if (!Initied)
        {
            Initied = true;

            Content = new DataFolder("/home/lianny/$/home/lianny/Gwent/GwentPro/Assets/Resources/CharacterManagerSource");
        }
    }
    public static string Query(string query)
    {
        Init();

        if (!string.IsNullOrEmpty(query) && Content.FilesRoot.Length != 0)
        {
            SearchResult result = Engine.Query(query, Content);
            //Debug.Log(result.ItemsArray[0].Score);

            if (result.ItemsArray[0].Score < 0.03) return "Morty Oculto";

            int count = 0;

            if (query.StartsWith("Morty"))
            {
                while (!result.ItemsArray[count].Title.StartsWith("Morty"))
                {
                    count++;
                }
            }

            return result.ItemsArray[count].Title;
        }

        return null;

    }
}
