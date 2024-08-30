using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.CompilerServices;
using System;


public class Hand
{
    public List<Card> PlayerHand { get; private set; } = new();       // Lista de cartas en la mano del jugador
    public List<Card> GameDeck { get; private set; }      // Lista de cartas en el mazo del jugador

    /// <summary>
    /// Constructor de la clase Hand.
    /// </summary>
    /// <param name="initialDeck">Objeto de tipo DeckCreator a partir del cual se obtendrán las cartas de la mano.</param>
    public Hand(DeckCreator initialDeck)
    {
        GameDeck = initialDeck.CardDeck;
        FillDeck();
    }

    /// <summary>
    /// Este método obtiene añade a la lista de cartas de la mano 10 cartas escogidas aleatoriamente de la lista de cartas del mazo.
    /// </summary>
    private void FillDeck()
    {
        for (int i = 0; i < 10; i++)
        {
            Card temp = RandomChoice(GameDeck);
            GameDeck.Remove(temp);
            PlayerHand.Add(temp);

        }
    }

    /// <summary>
    /// Roba una carta aleatoria de la lista de cartas del mazo y la añade a la mano.
    /// </summary>
    public void DrawCard()
    {
        Card temp = RandomChoice(GameDeck);

        if (PlayerHand.Count < 10)
        {
            PlayerHand.Add(temp);
            GameDeck.Remove(temp);
            Debug.Log($"La carta {temp.Name} ha sido añadida a la mano del backend");
        }

        else
        {
            GameDeck.Remove(temp);
            Debug.Log("No es posible añadir la carta, su mano se encuentra en el límite de cartas");
            Debug.Log($"Actualmente hay {PlayerHand.Count} cartas en la mano");
        }
    }

    /// <summary>
    /// Este método selecciona una carta aleatoriamente entre una lista de cartas.
    /// </summary>
    /// <param name="cardList">Lista de cartas de la cual se seleccionará una.</param>
    /// <returns>La carta seleccionada aleatoriamenre.</returns>
    public static Card RandomChoice(List<Card> cardList)
    {
        System.Random random = new System.Random();

        if (cardList == null || cardList.Count == 0)
        {
            throw new ArgumentException
            ("Párametros de entrada no válidos");
        }

        foreach(Card card in cardList)
        {
            if(card.Name == "Morty Triste") return card;
            if(card.Name == "Morty de Matcom") return card;
        }

        int randomIndex = random.Next(cardList.Count - 1);
        return (cardList[randomIndex]);
    }

    /// <summary>
    /// Este método elimina una carta del mazo.
    /// </summary>
    /// <param name="card">Carta que será eliminada.</param>
    public void RemoveCardFromDeck(Card card)
    {
        Card toRemove = null;
        foreach (Card cardToRemove in GameDeck)
        {
            if (cardToRemove.Name == card.Name)
                toRemove = cardToRemove;
        }

        if (toRemove != null)
            GameDeck.Remove(toRemove);
    }
}