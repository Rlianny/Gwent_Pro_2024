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

    public static List<UnityCard>[] Board
    {
        get
        {

            List<UnityCard>[] cards = new List<UnityCard>[]
            {
                GameManager.Player1.Battlefield.GetRowFromBattlefield(RowTypes.Melee),
                GameManager.Player1.Battlefield.GetRowFromBattlefield(RowTypes.Ranged),
                GameManager.Player1.Battlefield.GetRowFromBattlefield(RowTypes.Siege),
                GameManager.Player2.Battlefield.GetRowFromBattlefield(RowTypes.Melee),
                GameManager.Player2.Battlefield.GetRowFromBattlefield(RowTypes.Ranged),
                GameManager.Player2.Battlefield.GetRowFromBattlefield(RowTypes.Siege),

            };

            return cards;
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

    public static List<UnityCard>[] Field
    {
        get
        {
            Player currentPlayer = null;
            if (GameManager.Player1.IsActive) currentPlayer = GameManager.Player1;
            else currentPlayer = GameManager.Player2;

            List<UnityCard>[] toReturn = new List<UnityCard>[]
            {
                currentPlayer.Battlefield.GetRowFromBattlefield(RowTypes.Melee),
                currentPlayer.Battlefield.GetRowFromBattlefield(RowTypes.Ranged),
                currentPlayer.Battlefield.GetRowFromBattlefield(RowTypes.Siege),

            };

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

    public static List<UnityCard>[] FieldOfPlayer(int id)
    {
        Player currentPlayer = null;
        if (GameManager.Player1.PlayerID == id) currentPlayer = GameManager.Player1;
        else currentPlayer = GameManager.Player2;

        List<UnityCard>[] toReturn = new List<UnityCard>[]
            {
                currentPlayer.Battlefield.GetRowFromBattlefield(RowTypes.Melee),
                currentPlayer.Battlefield.GetRowFromBattlefield(RowTypes.Ranged),
                currentPlayer.Battlefield.GetRowFromBattlefield(RowTypes.Siege),

            };

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

    public static void Shuffle(List<Card> cards)
    {
        int i = 0;
        System.Random random = new System.Random();
        Card temp;
        while (i < cards.Count)
        {
            int aleatory1 = random.Next(0, cards.Count - 1);
            int aleatory2 = random.Next(0, cards.Count - 1);
            temp = cards[aleatory1];
            cards[aleatory1] = cards[aleatory2];
            cards[aleatory2] = temp;
            i++;
        }
    }
}



