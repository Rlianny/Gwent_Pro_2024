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
        //Init();

        if (!string.IsNullOrEmpty(query) && Content.FilesRoot.Length != 0)
        {
            SearchResult result = Engine.Query(query, Content);
            Debug.Log(result.ItemsArray[0].Score);

            if (result.ItemsArray[0].Score < 0.03) return "Morty Oculto";
            return result.ItemsArray[0].Title;
        }

        // SearchItem ToChange = new SearchItem("Error", "Realice una nueva búsqueda", (float)0.05);
        // SearchItem[] Change = new SearchItem[] { ToChange };
        return null;

    }
}
