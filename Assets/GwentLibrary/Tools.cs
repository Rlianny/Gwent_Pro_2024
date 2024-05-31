using System;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.VisualScripting;

public static class Tools
{
    /// <summary>
    /// Este método cuenta la cantidad de cartas en el campo de batalla de un jugador.
    /// </summary>
    /// <param name="battlefield">El campo de batalla del que se contarán las cartas.</param>
    /// <returns>La cantidad de cartas en el campo de un jugador.</returns>
    public static int NumberOfUnitysOnBattlefield(List<UnityCard>[] battlefield)
    {
        int count = 0;

        foreach (List<UnityCard> cardList in battlefield)
        {
            count += cardList.Count;
        }

        return count;
    }

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

