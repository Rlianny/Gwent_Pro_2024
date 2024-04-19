using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData : MonoBehaviour
{
    public static GameData gameData;
    public static string Player1Name;
    public static DeckCreator Player1InitialDeck;
    public static Player Player1;

    public static string Player2Name;
    public static DeckCreator Player2InitialDeck;
    public static Player Player2;

    public static bool IsReady = false;


    private void Awake()
    {
        if (gameData == null)
        {
            gameData = this;
            DontDestroyOnLoad(gameObject);
        }

        else if (gameData != this)
            Destroy(gameObject);
    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void CreatePlayer()
    {
        if (Player2Name == null)
        {
            Player1InitialDeck = CreatingDeck.actualDeck;
            Player1 = new Player(Player1Name, Player1InitialDeck.Faction, Player1InitialDeck);
        }

        else
        {
            Player2InitialDeck = CreatingDeck.actualDeck;
            Player2 = new Player(Player2Name, Player2InitialDeck.Faction, Player2InitialDeck);
            IsReady = true;
        }
    }

    public void RestartData()
    {
        gameData = null;
        
        Player1Name = null;
        Player1InitialDeck = null;
        Player1 = null;

        Player2Name = null;
        Player2InitialDeck = null;
        Player2 = null;

        IsReady = false;
    }

}
