using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Security.Cryptography;
using System;
using System.Diagnostics;
using System.ComponentModel.Design;
using UnityEditor;
using System.Data;

public abstract class Effect
{
    public static Dictionary<int, Effect> EffectDictionary { get; private set; } = new()
    {
        {1, new IncreaseMeleeRowEffect()},
        {2, new IncreaseRangedRowEffect()},
        {3, new IncreaseSigeeRowEffect()},
        {4, new WeatherMeleeRowEffect()},
        {5, new WeatherRangedRowEffect()},
        {6, new WeatherSigeeRowEffect()},
        {7, new DrawEffect()},
        {8, new DeleteTheLeastEffect()},
        {9, new DeleteTheMostEfect()},
        {10, new CloseBondEffect()},
        {11, new ClearARowEffect()},
        {12, new ClearanceEffect()},
        {13, new AverageEffect()},
        {14, new DecoyEffect()},
        {15, new SupremacyEffect()},
        {16, new CancelEffect()},
        {17, new VoidEffect()},
    };

    /// <summary>
    /// Este método manipula el campo de batalla realizando el efecto de la carta.
    /// </summary>
    /// <param name="ActivePlayer">El jugador activo en el momento de la llamada del método.</param>
    /// <param name="RivalPlayer">El jugador rival al momento de la llamada del método.</param>
    /// <param name="card">La carta desde la cual se activa el efecto.</param>
    public abstract void TakeEffect(Player ActivePlayer, Player RivalPlayer, Card card);
}

public abstract class IncreaseEffect : Effect
{

}

/// <summary>
/// Aumenta en 1 el poder de cada Unidad de plata en la fila Melee del jugador activo al momento de la llamada del método.
/// </summary>
public class IncreaseMeleeRowEffect : IncreaseEffect
{
    public override void TakeEffect(Player ActivePlayer, Player RivalPlayer, Card card)
    {
        Card toInvoke = null;

        if (ActivePlayer.Battlefield.IncreaseColumn[0] == null)
        {
            foreach (Card toFind in ActivePlayer.PlayerHand.GameDeck)
            {
                if (toFind is IncreaseCard increaseCard && increaseCard.Row == RowTypes.Melee)
                {
                    toInvoke = increaseCard;
                }
            }

        }

        if (toInvoke != null)
        {
            GameManager.gameManager.InvokeACard(toInvoke, RowTypes.Melee);
            return;
        }
    }
}

/// <summary>
/// Aumenta en 1 el poder de cada Unidad de plata en la fila Ranged del jugador activo al momento de la llamada del método.
/// </summary>
public class IncreaseRangedRowEffect : IncreaseEffect
{
    public override void TakeEffect(Player ActivePlayer, Player RivalPlayer, Card card)
    {
        Card toInvoke = null;

        if (ActivePlayer.Battlefield.IncreaseColumn[1] == null)
        {
            foreach (Card toFind in ActivePlayer.PlayerHand.GameDeck)
            {
                if (toFind is IncreaseCard increaseCard && increaseCard.Row == RowTypes.Ranged)
                {
                    toInvoke = increaseCard;
                }
            }

        }

        if (toInvoke != null)
        {
            GameManager.gameManager.InvokeACard(toInvoke, RowTypes.Ranged);
            return;
        }
    }
}

/// <summary>
/// Aumenta en 1 el poder de cada Unidad de plata en la fila Sigee del jugador activo al momento de la llamada del método.
/// </summary>
public class IncreaseSigeeRowEffect : IncreaseEffect
{
    public override void TakeEffect(Player ActivePlayer, Player RivalPlayer, Card card)
    {
        Card toInvoke = null;

        if (ActivePlayer.Battlefield.IncreaseColumn[2] == null)
        {
            foreach (Card toFind in ActivePlayer.PlayerHand.GameDeck)
            {
                if (toFind is IncreaseCard increaseCard && increaseCard.Row == RowTypes.Sigee)
                {
                    toInvoke = increaseCard;
                }
            }

        }

        if (toInvoke != null)
        {
            GameManager.gameManager.InvokeACard(toInvoke, RowTypes.Sigee);
            return;
        }
    }
}

public abstract class WeatherEffect : Effect
{

}

/// <summary>
/// Disminuye a 1 el poder de cada Unidad de plata en la fila Melee de ambos jugadores.
/// </summary>
public class WeatherMeleeRowEffect : WeatherEffect
{
    public override void TakeEffect(Player ActivePlayer, Player RivalPlayer, Card card)
    {
        Card toInvoke = null;

        if (PlayerBattlefield.WeatherRow[0] == null)
        {
            foreach (Card toFind in ActivePlayer.PlayerHand.GameDeck)
            {
                if (toFind is WeatherCard weatherCard && weatherCard.Row == RowTypes.Melee)
                {
                    toInvoke = weatherCard;
                }
            }
        }

        if (toInvoke != null)
        {
            GameManager.gameManager.InvokeACard(toInvoke, RowTypes.Melee);
            return;
        }

    }
}

/// <summary>
/// Disminuye a 1 el poder de cada Unidad de plata en la fila Ranged de ambos jugadores.
/// </summary>
public class WeatherRangedRowEffect : WeatherEffect
{
    public override void TakeEffect(Player ActivePlayer, Player RivalPlayer, Card card)
    {
        Card toInvoke = null;

        if (PlayerBattlefield.WeatherRow[1] == null)
        {
            foreach (Card toFind in ActivePlayer.PlayerHand.GameDeck)
            {
                if (toFind is WeatherCard weatherCard && weatherCard.Row == RowTypes.Ranged)
                {
                    toInvoke = weatherCard;
                }
            }
        }

        if (toInvoke != null)
        {
            GameManager.gameManager.InvokeACard(toInvoke, RowTypes.Ranged);
            return;
        }
    }
}

/// <summary>
/// Disminuye a 1 el poder de cada Unidad de plata en la fila Sigee de ambos jugadores.
/// </summary>
public class WeatherSigeeRowEffect : WeatherEffect
{
    public override void TakeEffect(Player ActivePlayer, Player RivalPlayer, Card card)
    {
        Card toInvoke = null;

        if (PlayerBattlefield.WeatherRow[2] == null)
        {
            foreach (Card toFind in ActivePlayer.PlayerHand.GameDeck)
            {
                if (toFind is WeatherCard weatherCard && weatherCard.Row == RowTypes.Sigee)
                {
                    toInvoke = weatherCard;
                }
            }

        }

        if (toInvoke != null)
        {
            GameManager.gameManager.InvokeACard(toInvoke, RowTypes.Sigee);
            return;
        }
    }
}

/// <summary>
/// Roba una carta del mazo del jugador activo al momento de la llamada del método y la añade a la mano de dicho jugador.
/// </summary>
public class DrawEffect : Effect
{
    public override void TakeEffect(Player ActivePlayer, Player RivalPlayer, Card card)
    {
        ActivePlayer.PlayerHand.DrawCard();
    }
}

public abstract class DeleteEffect : Effect
{

}

/// <summary>
/// Elimina la Unidad de plata con menos poder del campo del jugador rival al momento de la llamada del método. 
/// <remarks> En caso de que el poder mínimo sea el mismo para varias Unidades de plata, solo se eliminará una de ellas. </remarks>
/// </summary>
public class DeleteTheLeastEffect : DeleteEffect
{
    public override void TakeEffect(Player ActivePlayer, Player RivalPlayer, Card card)
    {
        int minPower = int.MaxValue;
        int minPowerIndex = -1;
        int pos = -1;
        SilverUnityCard cardToDelete = null;

        for (int i = 0; i < RivalPlayer.Battlefield.Battlefield.Length; i++)
        {
            foreach (var unity in RivalPlayer.Battlefield.Battlefield[i])
            {
                if (unity is SilverUnityCard silverUnityCard && silverUnityCard.ActualPower < minPower)
                {
                    cardToDelete = silverUnityCard;
                    minPower = silverUnityCard.ActualPower;
                    minPowerIndex = i;
                    pos = 1;
                }
            }
        }

        if (pos != -1)
        {
            if (pos == 0)
            {
                if (cardToDelete != null)
                {
                    ActivePlayer.Battlefield.Battlefield[minPowerIndex].Remove(cardToDelete);
                }
            }
            else
            {
                if (cardToDelete != null)
                {
                    RivalPlayer.Battlefield.Battlefield[minPowerIndex].Remove(cardToDelete);
                }
            }

        }
    }
}

/// <summary>
/// Elimina la Unidad de plata con más poder de todo el campo de batalla. 
/// <remarks> En caso de que el poder máximo sea el mismo para varias Unidades de plata, solo se eliminará una de ellas. </remarks>
/// </summary>
public class DeleteTheMostEfect : DeleteEffect
{
    public override void TakeEffect(Player ActivePlayer, Player RivalPlayer, Card card)
    {
        int maxPower = int.MinValue;
        int maxPowerIndex = -1;
        int pos = -1;
        SilverUnityCard cardToDelete = null;

        for (int i = 0; i < RivalPlayer.Battlefield.Battlefield.Length; i++)
        {
            foreach (var unity in RivalPlayer.Battlefield.Battlefield[i])
            {
                if (unity is SilverUnityCard silverUnityCard && silverUnityCard.ActualPower > maxPower)
                {
                    cardToDelete = silverUnityCard;
                    maxPower = silverUnityCard.ActualPower;
                    maxPowerIndex = i;
                    pos = 0;
                }
            }
        }

        for (int i = 0; i < ActivePlayer.Battlefield.Battlefield.Length; i++)
        {
            foreach (var unity in ActivePlayer.Battlefield.Battlefield[i])
            {
                if (unity is SilverUnityCard silverUnityCard && silverUnityCard.ActualPower > maxPower)
                {
                    cardToDelete = silverUnityCard;
                    maxPower = silverUnityCard.ActualPower;
                    maxPowerIndex = i;
                    pos = 1;
                }
            }
        }

        if (pos != -1)
        {
            if (pos == 0)
            {
                if (cardToDelete != null)
                {
                    RivalPlayer.Battlefield.Battlefield[maxPowerIndex].Remove(cardToDelete);
                }
            }
            else
            {
                if (cardToDelete != null)
                {
                    ActivePlayer.Battlefield.Battlefield[maxPowerIndex].Remove(cardToDelete);
                }
            }

        }
    }
}

/// <summary>
/// Multiplica por n el poder de cada carta igual a la carta que llama al método, siendo n la cantidad de apariciones de dicha carta en el campo del jugador activo al momento de la llamada del método.
/// </summary>
public class CloseBondEffect : Effect
{
    public override void TakeEffect(Player ActivePlayer, Player RivalPlayer, Card card)
    {
        int n = 0;

        foreach (var cardList in ActivePlayer.Battlefield.Battlefield)
        {
            if (cardList.Count > 0)
            {
                foreach (var unityCard in cardList)
                {
                    if (unityCard.Name == card.Name)
                        n++;
                }
            }
        }

        foreach (var cardList in ActivePlayer.Battlefield.Battlefield)
        {
            if (cardList.Count > 0)
            {
                foreach (var unityCard in cardList)
                {
                    if (unityCard.Name == card.Name && unityCard is SilverUnityCard silverUnityCard)
                        silverUnityCard.ActualPower *= n;
                }
            }
        }

    }

}

/// <summary>
/// Elimina todas las Unidades de plata de la fila con menos unidades del campo. 
/// <remarks> En caso de que el número mínimo de unidades en una fila sea el mismo para varias filas, solo se limpiará una fila. </remarks>
/// </summary>
public class ClearARowEffect : Effect
{
    public override void TakeEffect(Player ActivePlayer, Player RivalPlayer, Card card)
    {
        List<UnityCard> listToClear = null;
        int minCount = int.MaxValue;


        foreach (List<UnityCard> cardList in RivalPlayer.Battlefield.Battlefield)
        {
            if (cardList.Count > 0 && cardList.Count < minCount && Tools.NumberOfSilversInRow(cardList) > 0)
            {
                minCount = cardList.Count;
                listToClear = cardList;
            }
        }

        foreach (List<UnityCard> cardList in ActivePlayer.Battlefield.Battlefield)
        {
            if (cardList.Count > 0 && cardList.Count < minCount && Tools.NumberOfSilversInRow(cardList) > 0)
            {
                minCount = cardList.Count;
                listToClear = cardList;
            }
        }

        if (listToClear != null)
        {
            for (int i = listToClear.Count - 1; i >= 0; i--)
            {
                if (listToClear[i] is SilverUnityCard)
                {
                    listToClear.RemoveAt(i);
                }
            }
        }
    }
}

/// <summary>
/// Elimina todos los climas del campo de batalla y devuelve a cada Unidad de plata su poder original.
/// </summary>
public class ClearanceEffect : Effect
{
    public override void TakeEffect(Player ActivePlayer, Player RivalPlayer, Card card)
    {
        PlayerBattlefield.ClearWeatherRow();

        for (int i = 0; i < 3; i++)
        {
            foreach (UnityCard unityCard in ActivePlayer.Battlefield.Battlefield[i])
            {
                if (unityCard is SilverUnityCard silverUnityCard)
                    silverUnityCard.ActualPower = silverUnityCard.Power;
            }

            foreach (UnityCard unityCard in RivalPlayer.Battlefield.Battlefield[i])
            {
                if (unityCard is SilverUnityCard silverUnityCard)
                    silverUnityCard.ActualPower = silverUnityCard.Power;
            }
        }
    }
}

/// <summary>
/// Calcula el promedio entre los poderes de todas las unidades del campo de batalla e iguala el poder actual de las Unidades de plata de todo el campo a ese promedio.
/// </summary>
public class AverageEffect : Effect
{
    public override void TakeEffect(Player ActivePlayer, Player RivalPlayer, Card card)
    {
        int average;
        int cardsInField = Tools.NumberOfUnitysOnBattlefield(ActivePlayer.Battlefield.Battlefield) + Tools.NumberOfUnitysOnBattlefield(RivalPlayer.Battlefield.Battlefield);
        average = (ActivePlayer.Battlefield.TotalScore + RivalPlayer.Battlefield.TotalScore) / cardsInField;

        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < ActivePlayer.Battlefield.Battlefield[i].Count; j++)
            {
                if (ActivePlayer.Battlefield.Battlefield[i][j] is SilverUnityCard silverUnityCard)
                    silverUnityCard.ActualPower = average;
            }

            for (int k = 0; k < RivalPlayer.Battlefield.Battlefield[i].Count; k++)
            {
                if (RivalPlayer.Battlefield.Battlefield[i][k] is SilverUnityCard silverUnityCard)
                    silverUnityCard.ActualPower = average;
            }
        }
    }
}

public class DecoyEffect : Effect
{
    public override void TakeEffect(Player ActivePlayer, Player RivalPlayer, Card card)
    {

    }
}

/// <summary>
/// Reduce a 1 el poder de todas las Unidades de plata en el campo del jugador rival al momento de la llamada del método.
/// </summary>
public class SupremacyEffect : Effect
{
    public override void TakeEffect(Player ActivePlayer, Player RivalPlayer, Card card)
    {
        for (int i = 0; i < 3; i++)
        {
            foreach (UnityCard unityCard in RivalPlayer.Battlefield.Battlefield[i])
            {
                if (unityCard is SilverUnityCard silverUnityCard)
                    silverUnityCard.ActualPower = 1;
            }
        }
    }
}

/// <summary>
/// Cancela el efecto de uno o ambos líderes.
/// </summary>
public class CancelEffect : Effect
{
    public override void TakeEffect(Player ActivePlayer, Player RivalPlayer, Card card)
    {
        if (ActivePlayer.PlayerLeader.EffectNumber == 16 && RivalPlayer.PlayerLeader.EffectNumber == 16)
        {
            ActivePlayer.PlayerLeader.CancellCardEffect();
            RivalPlayer.PlayerLeader.CancellCardEffect();
        }

        else
        {
            RivalPlayer.PlayerLeader.CancellCardEffect();
        }
    }
}

public class VoidEffect : Effect
{
    public override void TakeEffect(Player ActivePlayer, Player RivalPlayer, Card card)
    {

    }
}

