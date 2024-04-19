using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class LeaderCard : Card
{
    public LeaderCard(string[] CardInfoArray) : base(CardInfoArray)
    {

    }
}

public abstract class UnityCard : Card
{
    public List<string> Row { get; private set; }     // Lista donde cada elemento es un string que representa la posición de la carta en el campo de battalla (M, R, S)
    public int Power { get; private set; }        // Poder (puntos) de la carta
    public virtual bool Modificable { get; protected set; }       // Booleano que dice si la carta es afectada o no por climas, aumentos o efectos de otras cartas
    public virtual int PossibleAppearances { get; protected set; }        // Número de posibles apariciones de la carta en el mazo de un jugador
    public string RowString { get; private set; }     // String que contiene las posibles posiciones de la carta en el campo de battalla (M, R, S)

    public UnityCard(string[] CardInfoArray) : base(CardInfoArray)
    {
        RowString = CardInfoArray[3];
        Row = CardInfoArray[3].Split(' ', StringSplitOptions.RemoveEmptyEntries).ToList();
        Power = int.Parse(CardInfoArray[4]);
    }

    public UnityCard(Card card) : base(card)
    {
        if (card is UnityCard unityCard)
        {
            RowString = unityCard.RowString;
            Row = unityCard.Row;
            Power = unityCard.Power;
        }
    }
}

public class HeroCard : UnityCard
{
    public HeroCard(string[] CardInfoArray) : base(CardInfoArray)
    {
        Modificable = false;
        PossibleAppearances = 1;
    }
}

public class SilverUnityCard : UnityCard
{
    public int ActualPower { get; set; }      // Poder actual (puntos actuales) en el campo de batalla
    public SilverUnityCard(string[] CardInfoArray) : base(CardInfoArray)
    {
        Modificable = true;
        PossibleAppearances = 3;
        ActualPower = Power;
    }

    public SilverUnityCard(Card card) : base(card)
    {
        if (card is SilverUnityCard silverUnityCard)
        {
            Modificable = silverUnityCard.Modificable;
            PossibleAppearances = silverUnityCard.PossibleAppearances;
            ActualPower = silverUnityCard.Power;
        }
    }
}

public abstract class SpecialCard : Card
{
    public SpecialCard(string[] CardInfoArray) : base(CardInfoArray)
    {

    }
}

public class IncreaseCard : SpecialCard
{
    public string Row { get; private set; }       // string que representa la fila a la que esta carta afecta
    public IncreaseCard(string[] CardInfoArray) : base(CardInfoArray)
    {
        Row = CardInfoArray[3];
    }
}

public class ClearanceCard : SpecialCard
{
    public List<string> Row { get; private set; }     // Lista donde cada elemento es un string que representa la posición de la carta en el campo de battalla (M, R, S)
    public ClearanceCard(string[] CardInfoArray) : base(CardInfoArray)
    {
        Row = CardInfoArray[3].Split(' ', StringSplitOptions.RemoveEmptyEntries).ToList();
    }
}

public class WeatherCard : SpecialCard
{
    public string Row { get; private set; }       // string que representa la fila a la que esta carta afecta
    public WeatherCard(string[] CardInfoArray) : base(CardInfoArray)
    {
        Row = CardInfoArray[3];
    }
}