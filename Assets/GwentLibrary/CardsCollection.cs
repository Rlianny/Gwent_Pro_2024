using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Dynamic;
using System;
using System.Linq;

public class CardsCollection
{
    public int NumberOfCards { get; private set; } // Cantidad de cartas existentes
    public List<Card> Collection { get; private set; } = new(); // Lista que contiene todas las cartas del juego
    public Dictionary<string, List<Card>> AllFactions { get; private set; } = new(); // Diccionario que tiene como key los nombres de las facciones y como value las cartas pertenecientes a la facción (no incluye al líder)
    public Dictionary<string, Card> AllLeaders { get; private set; } = new();  // Diccionario que tiene como key los nombres de las facciones y como value a la carta líder de la facción
    public static Dictionary<string, Card> AllCardsName = new();

    /// <summary>
    /// Este método es el constructor de la clase CardsCollection, que representa el contenedor de todas las cartas existentes en el juego.
    /// </summary>
    /// <param name="cardInfoArray">Lista donde cada posición es un array que contiene la información de la carta.</param>
    public CardsCollection(List<string[]> cardInfoArray, List<CompiledObject> compileds)
    {
        AllCardsName = new();
        NumberOfCards = cardInfoArray.Count + compileds.Count;

        int count = 0;

        foreach (string[] infoArray in cardInfoArray)
        {
            Card card = TypeCreator(infoArray[0], infoArray);
            count++;
            CardBorn(card);
        }

        foreach (var compiledObj in compileds)
        {
            if (compiledObj is CompiledCard compiledCard)
            {
                Card card = TypeCreator(compiledCard);
                count++;
                CardBorn(card);
            }
        }

        Debug.Log($"Han sido cargadas {count} cartas");

        if (NumberOfCards == count)
            Debug.Log("Todas las cartas han sido cargadas");
    }

    private void CardBorn(Card card)
    {
        Collection.Add(card);

        if (!AllCardsName.ContainsKey(card.Name))
            AllCardsName.Add(card.Name, card);

        Debug.Log($"{card.Name} ha entrado al campo de batalla");

        if (card.Type != CardTypes.Líder)        // si la carta no es el Líder de una facción 
        {
            if (!AllFactions.Keys.Contains(card.Faction))        //  si la facción no estaba como llave del diccionario la agregamos y agregamos la carta 
            {
                List<Card> Temp = new();
                AllFactions.Add(card.Faction, Temp);
                Temp.Add(card);
            }
            else        // si la facción ya estaba establecida como llave del diccionario solo añadimos la carta
            {
                AllFactions[card.Faction].Add(card);
            }
        }
        else
        {
            if (!AllLeaders.ContainsKey(card.Faction))
                AllLeaders.Add(card.Faction, card);
        }
    }

    /// <summary>
    /// Este método elige el tipo de carta que será creado en correspondecia con el tipo declarado en la base de datos.
    /// </summary>
    /// <param name="TipeLetter">String que representa el tipo de carta.</param>
    /// <param name="CardInfoArray">Array que contiene toda la información de la carta.</param>
    /// <returns>Instancia de objeto que hereda de carta.</returns>
    private static Card TypeCreator(string TipeLetter, string[] CardInfoArray)
    {
        switch (TipeLetter)
        {
            case "L":
                return new LeaderCard(CardInfoArray);

            case "O":
                return new HeroCard(CardInfoArray);

            case "P":
                return new SilverUnityCard(CardInfoArray);

            case "A":
                return new IncreaseCard(CardInfoArray);

            case "C":
                return new WeatherCard(CardInfoArray);

            case "S":
                return new HeroCard(CardInfoArray);

            case "D":
                return new ClearanceCard(CardInfoArray);

            default: throw new ArgumentException("La carta tiene un tipo no definido");
        }
    }

    private static Card TypeCreator(CompiledCard compiledCard)
    {
        switch (compiledCard.Type)
        {
            case "Líder":
                return new LeaderCard(compiledCard);

            case "Oro":
                return new HeroCard(compiledCard);

            case "Plata":
                return new SilverUnityCard(compiledCard);

            default: throw new ArgumentException("La carta tiene un tipo no definido");
        }
    }
}