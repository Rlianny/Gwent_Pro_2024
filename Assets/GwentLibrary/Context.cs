using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Context
{
    public static int TriggerPlayer
    {
        get
        {
            if (GameManager.Player1.IsActive) return GameManager.Player1.PlayerID;
            else return GameManager.Player2.PlayerID;
        }
        private set { }
    }

    public static List<Card> Board
    {
        get
        {
            List<Card> toReturn = new();

            foreach (Card card in GameManager.Player1.Battlefield.GetRowFromBattlefield(RowTypes.Melee))
            {
                toReturn.Add(card);
            }

            foreach (Card card in GameManager.Player1.Battlefield.GetRowFromBattlefield(RowTypes.Ranged))
            {
                toReturn.Add(card);
            }

            foreach (Card card in GameManager.Player1.Battlefield.GetRowFromBattlefield(RowTypes.Siege))
            {
                toReturn.Add(card);
            }

            foreach (Card card in GameManager.Player2.Battlefield.GetRowFromBattlefield(RowTypes.Melee))
            {
                toReturn.Add(card);
            }

            foreach (Card card in GameManager.Player2.Battlefield.GetRowFromBattlefield(RowTypes.Ranged))
            {
                toReturn.Add(card);
            }

            foreach (Card card in GameManager.Player2.Battlefield.GetRowFromBattlefield(RowTypes.Siege))
            {
                toReturn.Add(card);
            }

            return toReturn;
        }
        private set { }
    }

    public static List<Card> Hand
    {
        get
        {
            if (GameManager.Player1.IsActive) return GameManager.Player1.PlayerHand.PlayerHand;
            else return GameManager.Player2.PlayerHand.PlayerHand;
        }
        private set { }
    }

    public static List<Card> Deck
    {
        get
        {
            if (GameManager.Player1.IsActive) return GameManager.Player1.PlayerHand.GameDeck;
            else return GameManager.Player2.PlayerHand.GameDeck;
        }
    }

    public static List<Card> Field
    {
        get
        {
            Player currentPlayer = null;
            if (GameManager.Player1.IsActive) currentPlayer = GameManager.Player1;
            else currentPlayer = GameManager.Player2;

            List<Card> toReturn = new List<Card>();

            foreach (Card card in currentPlayer.Battlefield.GetRowFromBattlefield(RowTypes.Melee))
            {
                toReturn.Add(card);
            }

            foreach (Card card in currentPlayer.Battlefield.GetRowFromBattlefield(RowTypes.Ranged))
            {
                toReturn.Add(card);
            }

            foreach (Card card in currentPlayer.Battlefield.GetRowFromBattlefield(RowTypes.Siege))
            {
                toReturn.Add(card);
            }

            return toReturn;
        }
        private set { }
    }

    public static List<Card> Graveyard
    {
        get
        {
            if (GameManager.Player1.IsActive) return GameManager.Player1.Battlefield.Graveyard;
            else return GameManager.Player2.Battlefield.Graveyard;
        }
    }

    public static List<Card> HandOfPlayer(int id)
    {
        if (GameManager.Player1.PlayerID == id) return GameManager.Player1.PlayerHand.PlayerHand;
        else return GameManager.Player2.PlayerHand.PlayerHand;
    }

    public static List<Card> FieldOfPlayer(int id)
    {
        Player currentPlayer = null;
        if (GameManager.Player1.PlayerID == id) currentPlayer = GameManager.Player1;
        else currentPlayer = GameManager.Player2;

        List<Card> toReturn = new List<Card>();

        foreach (Card card in currentPlayer.Battlefield.GetRowFromBattlefield(RowTypes.Melee))
        {
            toReturn.Add(card);
        }

        foreach (Card card in currentPlayer.Battlefield.GetRowFromBattlefield(RowTypes.Ranged))
        {
            toReturn.Add(card);
        }

        foreach (Card card in currentPlayer.Battlefield.GetRowFromBattlefield(RowTypes.Siege))
        {
            toReturn.Add(card);
        }

        return toReturn;
    }

    public static List<Card> GraveyardOfPlayer(int id)
    {
        if (GameManager.Player1.PlayerID == id) return GameManager.Player1.Battlefield.Graveyard;
        else return GameManager.Player2.Battlefield.Graveyard;
    }

    public static List<Card> DeckOfPlayer(int id)
    {
        if (GameManager.Player1.PlayerID == id) return GameManager.Player1.PlayerHand.GameDeck;
        else return GameManager.Player2.PlayerHand.GameDeck;
    }
}



