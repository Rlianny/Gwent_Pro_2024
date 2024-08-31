using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using System;
using System.Threading;

public class DeckCreator
{
    public string Faction { get; private set; } = String.Empty;     
    public Card DeckLeader { get; private set; }        
    public List<Card> CardDeck { get; private set; } = new();       
    public int CardsTotalNumber { get; private set; }       
    public int UnityCardsTotalNumber { get; private set; }      
    public int HeroCardsTotalNumber { get; private set; }       
    public int SpecialCardsTotalNumber { get; private set; }        
    public int UnityPowerTotalNumber { get; private set; }      

    /// <summary>
    /// Constructor de la clase DeckCreator.
    /// </summary>
    /// <param name="factionName">Nombre de la facción que tendrá el mazo.</param>
    /// <param name="factionCards">Diccionario que contiene todas las cartas clasificadas por facciones.</param>
    /// <param name="allLeaders">Diccionario que conteiene todos los líderes clasificados pod facciones.</param>
    public DeckCreator(string factionName, Dictionary<string, List<Card>> factionCards, Dictionary<string, Card> allLeaders)
    {
        Faction = factionName;
        DeckLeader = allLeaders[factionName];
        CardDeck = new();
        Debug.Log("Llegan " + factionCards[Faction].Count);
        foreach(Card card in factionCards[Faction])
        {
            if(card.Type != CardTypes.Líder)
            CardDeck.Add(card);
        }
        UpdateDeckInfo();
    }

    /// <summary>
    /// Esté método crea una carta nueva a partir de otra carta.
    /// </summary>
    /// <param name="card">Carta a partir de la cual se creará la carta.</param>
    /// <returns>Carta con las mismas propiedades de la que fue recibida como parámetro.</returns>
    public void DuplicateCard(Card card)
    {
        if (card is SilverUnityCard silverCard)
        {
            if (CardActualAppearances(silverCard) < 3)
            {
                Debug.Log("Se agregara la carta");
                Card copy = new SilverUnityCard(card);
                CardDeck.Add(copy);
                UpdateDeckInfo();
            }

            else
                Debug.Log("No es posible agregar más cartas");
        }

        else
            Debug.Log("Esta carta no es duplicable");
    }

    /// <summary>
    /// Este método añade una carta al mazo.
    /// </summary>
    /// <param name="card">Carta que será añadida al mazo.</param>
    public bool AddCardToMyDeck(Card card)
    {
        if (card.Faction == Faction || card.Faction == "Neutral")
        {
            if (!(card is HeroCard && CardDeck.Contains(card)) || (card is SilverUnityCard && CardActualAppearances(card) == 0))
            {
                Debug.Log("Se agregara la carta");
                CardDeck.Add(card);
                UpdateDeckInfo();
                return true;
            }
            return false;
        }

        return false;
    }

    /// <summary>
    /// Este método elimina una carta al mazo.
    /// </summary>
    /// <param name="card">Carta que será eliminada del mazo.</param>
    public void RemoveCardToMyDeck(Card card)
    {
        foreach (Card cardInDeck in CardDeck)
        {
            if (cardInDeck.Name == card.Name)
            {
                CardDeck.Remove(cardInDeck);
                UpdateDeckInfo();
                return;
            }
        }
    }

    /// <summary>
    /// Este método actualiza la información referente al mazo.
    /// </summary>
    private void UpdateDeckInfo()
    {
        CardsTotalNumber = CardDeck.Count;
        UnityCardsTotalNumber = UnityCardsCounter();
        HeroCardsTotalNumber = HeroCardsCounter();
        SpecialCardsTotalNumber = SpecialCardsCounter();
        UnityPowerTotalNumber = TotalPowerCounter();

    }

    /// <summary>
    /// Este método cuenta las cartas de unidad presentes en el mazo.
    /// </summary>
    private int UnityCardsCounter()
    {
        int count = 0;
        foreach (Card card in CardDeck)
        {
            if (card is UnityCard)
                count++;
        }

        return count;
    }

    /// <summary>
    /// Este método cuenta las cartas de oro presentes en el mazo.
    /// </summary>
    private int HeroCardsCounter()
    {
        int count = 0;
        foreach (Card card in CardDeck)
        {
            if (card is HeroCard)
                count++;
        }

        return count;
    }

    /// <summary>
    /// Este método cuenta las cartas speciales presentes en el mazo.
    /// </summary>
    private int SpecialCardsCounter()
    {
        int count = 0;
        foreach (Card card in CardDeck)
        {
            if (card is SpecialCard)
                count++;
        }

        return count;
    }

    /// <summary>
    /// Este método cuenta la fuerza total de las unidades presentes en el mazo.
    /// </summary>
    /// /// <returns>La suma de los puntos de las unidades presentes en el mazo.</returns>
    private int TotalPowerCounter()
    {
        int power = 0;
        foreach (Card card in CardDeck)
        {
            if (card is UnityCard unityCard)
                power += unityCard.Power;
        }

        return power;
    }

    /// <summary>
    /// Este método cuenta las apariciones de una carta en el mazo.
    /// </summary>
    /// <param name="cardToCount">Carta de la cual se desea saber el número de apariciones.</param>
    /// <returns>La cantidad de veces que aparece una carta en el mazo.</returns>
    public int CardActualAppearances(Card cardToCount)
    {
        int count = 0;

        foreach (Card card in CardDeck)
        {
            if (card.Name == cardToCount.Name)
                count++;
        }
        return count;
    }
}