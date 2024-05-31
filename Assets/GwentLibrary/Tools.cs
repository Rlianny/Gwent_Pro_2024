using System;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.VisualScripting;

public static class Tools
{
    public static Dictionary<int, RowTypes> RowForIndex = new()
    {
        {0, RowTypes.Melee},
        {1, RowTypes.Ranged},
        {2, RowTypes.Sigee},
    };

    public static int NumberOfSilversInRow(List<UnityCard> rowCards)
    {
        int count = 0;

        foreach(UnityCard unityCard in rowCards)
        {
            if(unityCard is SilverUnityCard)
            count++;
        }

        return count;
    }

    public static List<RowTypes> GetRowTypes(string rows)
    {
        List<RowTypes> rowTypes = new();
        for(int i = 0; i < rows.Length; i++)
        {
            switch(rows[i])
            {
                case 'M':
                rowTypes.Add(RowTypes.Melee);
                continue;

                case 'R':
                rowTypes.Add(RowTypes.Ranged);
                continue;

                case 'S':
                rowTypes.Add(RowTypes.Sigee);
                continue;
            }
        }

        return rowTypes;
    }

        public static RowTypes GetRowTypesForSpecialCards(string rows)
    {   
        for(int i = 0; i < rows.Length; i++)
        {
            switch(rows[i])
            {
                case 'M':
                return RowTypes.Melee;
                
                case 'R':
                return RowTypes.Ranged;

                case 'S':
                return RowTypes.Sigee;
            }
        }

        throw new ArgumentException("La carta tiene una fila no definido");
    }
}

public enum CardTypes
{
    Líder,
    Unidad_Héroe,
    Unidad_de_Plata,
    Carta_de_Aumento,
    Carta_de_Clima,
    Señuelo,
    Carta_de_Despeje,
}

public enum RowTypes
{
    Melee,
    Ranged, 
    Sigee,
}

