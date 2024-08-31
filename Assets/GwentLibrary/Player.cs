using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Dynamic;
using System.Net.WebSockets;
using System;
using Unity.VisualScripting;

public class Player
{
    private static int IDSelector;
    public int PlayerID;
    public string PlayerName { get; private set; } 
    public string PlayerFaction { get; private set; }      
    public Card PlayerLeader { get; private set; }     
    public Hand PlayerHand { get; private set; }        
    public PlayerBattlefield Battlefield { get; private set; }      
    public bool HasPassed { get; private set; }    
    public int GamesWon { get; private set; }       
    public bool IsActive { get; private set; } 

    /// <summary>
    /// Constructor de la clase Player.
    /// </summary>
    /// <param name="name">Nombre del jugador.</param>
    /// <param name="faction">Facción a la que pertenecen las cartas en el mazo.</param>
    /// <param name="initialDeck">Mazo elegido por el jugador.</param>
    public Player(string name, string faction, DeckCreator initialDeck)
    {
        PlayerName = name;
        PlayerFaction = faction;
        PlayerLeader = initialDeck.DeckLeader;
        PlayerHand = new Hand(initialDeck);
        Battlefield = new();
        HasPassed = false;
        GamesWon = 0;
        IsActive = false;
        PlayerID = GetNewID();
    }

    private int GetNewID()
    {
        return IDSelector++;
    }

    /// <summary>
    /// Este método activa el efecto del líder de la facción.
    /// </summary>
    /// <param name="activePlayer">El jugador activo al momento de la llamada del método.</param>
    /// <param name="rivalPlayer">El jugador rival al momento de la llamada del método.</param>
    public void UseLeaderSkill(Player activePlayer, Player rivalPlayer)
    {
        PlayerLeader.ActivateEffect(activePlayer, rivalPlayer, PlayerLeader);
        activePlayer.Battlefield.UpdateBattlefieldInfo();
        rivalPlayer.Battlefield.UpdateBattlefieldInfo();
    }

    /// <summary>
    /// Este método se llama cuando el jugador elige pasar turno.
    /// </summary>
    public void PassTurn()
    {
        HasPassed = true;
        IsActive = false;
    }

    /// <summary>
    /// Este método añade una victoria al jugador.
    /// </summary>
    public void AddVictory()
    {
        GamesWon = GamesWon + 1;
    }

    /// <summary>
    /// Este método se llama para iniciar el turno del jugador.
    /// </summary>
    public void StartTurn()
    {
        IsActive = true;
    }

    /// <summary>
    /// Este método se llama para finalizar el turno del jugador.
    /// </summary>
    public void FinishTurn()
    {
        IsActive = false;
    }

    /// <summary>
    /// Este método se llama para iniciar una nueva ronda.
    /// </summary>
    public void NewRound()
    {
        HasPassed = false;
    }

    /// <summary>
    /// Este método maneja el movimiento de una carta hacia un lugar del tablero.
    /// </summary>
    /// <param name="activePlayer">El jugador activo al momento de la llamada del método.</param>
    /// <param name="rivalPlayer">El jugador rival al momento de la llamada del método.</param>
    /// <param name="card">Carta que será manipulada.</param>
    /// <param name="Row">Lugar al que se moverá la carta.</param>
    public void DragAndDropMovement(Player activePlayer, Player rivalPlayer, Card card, RowTypes row)
    {
        if (row == RowTypes.Melee || row == RowTypes.Ranged || row == RowTypes.Siege)
        {
            if (card is UnityCard unityCard)
            {
                Battlefield.AddCardToBattlefiel(unityCard, row);
                RemoveCardAndActivateEffect(activePlayer, rivalPlayer, unityCard);
            }

            if (card is IncreaseCard increaseCard)
            {
                Battlefield.AddCardToIncreaseColumn(increaseCard, row);
                RemoveCardAndActivateEffect(activePlayer, rivalPlayer, increaseCard);
            }

            if (card is WeatherCard weatherCard)
            {
                PlayerBattlefield.AddCardToWeatherRow(weatherCard, row);
                RemoveCardAndActivateEffect(activePlayer, rivalPlayer, weatherCard);
            }

            if (card is ClearanceCard clearanceCard)
            {
                RemoveCardAndActivateEffect(activePlayer, rivalPlayer, clearanceCard);
            }
        }

        else
            throw new ArgumentException("Parámetro de entrada Row incorrecto");
    }

    private void RemoveCardAndActivateEffect(Player activePlayer, Player rivalPlayer, Card card)
    {
        PlayerHand.PlayerHand.Remove(card);

        activePlayer.Battlefield.UpdateBattlefieldInfo();
        rivalPlayer.Battlefield.UpdateBattlefieldInfo();

        card.ActivateEffect(activePlayer, rivalPlayer, card);
        Debug.Log("Se ha hecho el efecto");

        activePlayer.Battlefield.UpdateBattlefieldInfo();
        rivalPlayer.Battlefield.UpdateBattlefieldInfo();
    }

    /// <summary>
    /// Este método limpia todo el campo de batalla del jugador.
    /// </summary>
    public void GetNewBattlefield()
    {
        Battlefield.ClearBattelfield();
        Battlefield.UpdateBattlefieldInfo();
    }
}


