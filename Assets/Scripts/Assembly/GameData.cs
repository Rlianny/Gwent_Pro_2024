using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameData
{
    public static string Player1Name;
    public static DeckCreator Player1InitialDeck;
    public static Player Player1;

    public static string Player2Name;
    public static DeckCreator Player2InitialDeck;
    public static Player Player2;

    public static bool IsReady = false;

    public static void CreatePlayer()
    {
        if (Player2Name == null)
        {
            Player1InitialDeck = CreatingDeck.actualDeck;
            Player1 = new Player(Player1Name, Player1InitialDeck.Faction, Player1InitialDeck);
            Player1.PlayerLeader.SetOwner(Player1.PlayerID);
            foreach (Card card in Player1.PlayerHand.GameDeck)
            {
                card.SetOwner(Player1.PlayerID);
            }
        }

        else
        {
            Player2InitialDeck = CreatingDeck.actualDeck;
            Player2 = new Player(Player2Name, Player2InitialDeck.Faction, Player2InitialDeck);
            Player2.PlayerLeader.SetOwner(Player2.PlayerID);
            foreach (Card card in Player2.PlayerHand.GameDeck)
            {
                card.SetOwner(Player2.PlayerID);
            }
            IsReady = true;
        }
    }

    public static void RestartData()
    {
        Player1Name = null;
        Player1InitialDeck = null;
        Player1 = null;

        Player2Name = null;
        Player2InitialDeck = null;
        Player2 = null;

        IsReady = false;
    }

}
