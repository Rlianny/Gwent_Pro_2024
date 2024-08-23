using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEventReport
{
    public GameEvents GameEvent;
    public Player ActivePlayer;
    public Player RivalPlayer;
    public Card Card;
    public RowTypes RowOfCard;

    public GameEventReport(GameEvents currentEvent, Player activePlayer, Player rivalPlayer, Card triggerCard, RowTypes rowOfCard)
    {
        GameEvent = currentEvent;
        ActivePlayer = activePlayer;
        RivalPlayer = rivalPlayer;
        Card = triggerCard;
        RowOfCard = rowOfCard;
    }

    public GameEventReport(GameEvents gameEvent)
    {
        GameEvent = gameEvent;
    }

    public GameEventReport(GameEvents currentEvent, Player activePlayer, Player rivalPlayer, Card triggerCard)
    {
        GameEvent = currentEvent;
        ActivePlayer = activePlayer;
        RivalPlayer = rivalPlayer;
        Card = triggerCard;
    }

}
