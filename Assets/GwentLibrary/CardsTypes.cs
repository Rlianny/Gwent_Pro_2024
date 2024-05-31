using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using UnityEditor;

public class LeaderCard : Card
{
    public LeaderCard(string[] CardInfoArray) : base(CardInfoArray)
    {

    }
}

public abstract class UnityCard : Card
{
    public List<RowTypes> Row { get; private set; }     // Lista donde cada elemento es un string que representa la posición de la carta en el campo de battalla (M, R, S)
    public int Power { get; private set; }        // Poder (puntos) de la carta
    public string RowString { get; private set; }     // String que contiene las posibles posiciones de la carta en el campo de battalla (M, R, S)

    public UnityCard(string[] CardInfoArray) : base(CardInfoArray)
    {
        RowString = CardInfoArray[3];
        Row = Tools.GetRowTypes(CardInfoArray[3]);
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

    }
}

public class SilverUnityCard : UnityCard
{
    public int ActualPower { get; set; }      // Poder actual (puntos actuales) en el campo de batalla
    public static int PossibleAppearances = 3;
    public SilverUnityCard(string[] CardInfoArray) : base(CardInfoArray)
    {
        ActualPower = Power;
    }

    public SilverUnityCard(Card card) : base(card)
    {
        if (card is SilverUnityCard silverUnityCard)
        {
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
    public RowTypes Row { get; private set; }
    public IncreaseCard(string[] CardInfoArray) : base(CardInfoArray)
    {
        Row = Tools.GetRowTypesForSpecialCards(CardInfoArray[3]);
    }
}

public class ClearanceCard : SpecialCard
{
    public ClearanceCard(string[] CardInfoArray) : base(CardInfoArray)
    {

    }
}

public class WeatherCard : SpecialCard
{
    public RowTypes Row { get; private set; }
    public WeatherCard(string[] CardInfoArray) : base(CardInfoArray)
    {
        Row = Tools.GetRowTypesForSpecialCards(CardInfoArray[3]);
    }
}