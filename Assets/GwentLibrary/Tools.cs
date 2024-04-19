using System.Collections.Generic;
using System.Diagnostics;

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
}