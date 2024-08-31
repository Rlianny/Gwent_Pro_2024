using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.VisualScripting;
using System.Threading.Tasks;

public class GameManager : Subject
{
    public static bool GameOver = false;        // booleano que indica si el juego está terminado
    public static GameManager gameManager;
    public static Player Player1;       // Jugador1
    public static Player Player2;       // Jugador2

    public static GameManager AwakeManager()
    {
        return new GameManager();
    }

    private GameManager()
    {
        gameManager = this;

        Player1 = GameData.Player1;
        Player2 = GameData.Player2;

        PickPLayerAsFirst(Player1, Player2);

        if (Player1.IsActive == true)
            Debug.Log($"{Player1.PlayerName} empieza");


        if (Player2.IsActive == true)
            Debug.Log($"{Player2.PlayerName} empieza");

        NotifyObservers(new GameEventReport(GameEvents.Start, Player1, Player2));
    }

    public void PlayACard(Card card, RowTypes row)
    {
        if (Player1.IsActive == true)
            InternalPlayACard(Player1, Player2, card, row);

        else
            InternalPlayACard(Player2, Player1, card, row);

    }

    private void InternalPlayACard(Player activePlayer, Player rivalPlayer, Card card, RowTypes row)
    {
        activePlayer.DragAndDropMovement(activePlayer, rivalPlayer, card, row);
        Debug.Log($"{activePlayer.PlayerName} ha movido la carta {card.Name} a la fila {row}");

        if (card.Type == CardTypes.Señuelo)
            NotifyObservers(new GameEventReport(GameEvents.DecoyEventStart, activePlayer, rivalPlayer, card, row));

        else
        {
            NotifyObservers(new GameEventReport(GameEvents.Summon, activePlayer, rivalPlayer, card, row));
            ChangeTurn(activePlayer, rivalPlayer);
        }
    }

    public void UseLeaderSkill()
    {
        if (Player1.IsActive == true)
            InternalUseLeaderSkill(Player1, Player2);

        else
            InternalUseLeaderSkill(Player2, Player1);
    }

    private void InternalUseLeaderSkill(Player activePlayer, Player rivalPlayer)
    {
        activePlayer.UseLeaderSkill(activePlayer, rivalPlayer);
        NotifyObservers(new GameEventReport(GameEvents.Summon, activePlayer, rivalPlayer, activePlayer.PlayerLeader));
        Debug.Log($"{activePlayer.PlayerName} ha usado la habilidad del lider");
        ChangeTurn(activePlayer, rivalPlayer);
    }


    public void PassTurn()
    {
        if (Player1.IsActive == true)
            InternalPassTurn(Player1, Player2);

        else if (Player2.IsActive == true)
            InternalPassTurn(Player2, Player1);
    }

    private void InternalPassTurn(Player activePlayer, Player rivalPlayer)
    {
        activePlayer.PassTurn();

        if (rivalPlayer.HasPassed == false)
        {
            NotifyObservers(new GameEventReport(GameEvents.PassTurn, activePlayer, rivalPlayer));
            rivalPlayer.StartTurn();
        }

        else
            BothPLayersHasPassed();
    }

    public void FinishDecoyEvent(Card cardToSwap)
    {
        if (Player1.IsActive == true)
            InternalFinishDecoyEvent(Player1, Player2, cardToSwap);

        else
            InternalFinishDecoyEvent(Player2, Player1, cardToSwap);
    }

    private void InternalFinishDecoyEvent(Player activePlayer, Player rivalPlayer, Card cardToSwap)
    {
        int pos = -1;
        for (int i = 0; i < 3; i++)
        {
            foreach (UnityCard unity in activePlayer.Battlefield.GetRowFromBattlefield(Tools.RowForIndex[i]))
            {
                if (unity.Type == CardTypes.Señuelo)
                {
                    pos = i;
                }
            }
        }

        if (pos >= 0)
        {
            if (cardToSwap is UnityCard unityCard)
            {
                activePlayer.Battlefield.RemoveCardFromBattlefield(unityCard, Tools.RowForIndex[pos]);
                activePlayer.PlayerHand.PlayerHand.Add(unityCard);
                if (unityCard is SilverUnityCard silverUnityCard)
                    silverUnityCard.ActualPower = silverUnityCard.Power;
                activePlayer.Battlefield.UpdateBattlefieldInfo();
                rivalPlayer.Battlefield.UpdateBattlefieldInfo();
                NotifyObservers(new GameEventReport(GameEvents.DecoyEventFinish, activePlayer, rivalPlayer, unityCard));
                ChangeTurn(activePlayer, rivalPlayer);
            }
        }
    }

    public void AbortDecoyEvent()
    {
        if (Player1.IsActive == true)
            InternalAbortDecoyEvent(Player1, Player2);

        else
            InternalAbortDecoyEvent(Player2, Player1);
    }

    private void InternalAbortDecoyEvent(Player activePlayer, Player rivalPlayer)
    {
        activePlayer.Battlefield.UpdateBattlefieldInfo();
        rivalPlayer.Battlefield.UpdateBattlefieldInfo();
        NotifyObservers(new GameEventReport(GameEvents.DecoyEventAbort, activePlayer, rivalPlayer));
        ChangeTurn(activePlayer, rivalPlayer);
    }

    public void InvokeACard(Card card, RowTypes row)
    {
        if (Player1.IsActive == true)
            InternalInvokeACard(Player1, Player2, card, row);

        else
            InternalInvokeACard(Player2, Player1, card, row);
    }

    private void InternalInvokeACard(Player activePlayer, Player rivalPlayer, Card card, RowTypes row)
    {
        InvokeMovement(activePlayer, rivalPlayer, card, row);
        Debug.Log($"La carta {card.Name} ha sido invocada por {activePlayer.PlayerName}");
        NotifyObservers(new GameEventReport(GameEvents.Invoke, activePlayer, rivalPlayer, card, row));
    }

    private void InvokeMovement(Player activePlayer, Player rivalPlayer, Card card, RowTypes Row)
    {
        if (Row == RowTypes.Melee || Row == RowTypes.Ranged || Row == RowTypes.Siege)
        {
            if (card is IncreaseCard increaseCard)
            {
                activePlayer.Battlefield.AddCardToIncreaseColumn(increaseCard, Row);
                activePlayer.PlayerHand.RemoveCardFromDeck(card);
                activePlayer.Battlefield.UpdateBattlefieldInfo();
                rivalPlayer.Battlefield.UpdateBattlefieldInfo();
            }

            if (card is WeatherCard weatherCard)
            {
                PlayerBattlefield.AddCardToWeatherRow(weatherCard, Row);
                activePlayer.PlayerHand.RemoveCardFromDeck(card);
                activePlayer.Battlefield.UpdateBattlefieldInfo();
                rivalPlayer.Battlefield.UpdateBattlefieldInfo();
            }
        }
    }

    public void ChangeTurn(Player Player1, Player Player2)
    {
        if (Player1.HasPassed == false || Player2.HasPassed == false)
        {
            if (Player1.IsActive == true)
                InternalChangeTurn(Player1, Player2);

            else
                InternalChangeTurn(Player2, Player1);
        }

        else
        {
            throw new ArgumentException("Turno no posible");
        }
    }

    private void InternalChangeTurn(Player activePlayer, Player rivalPlayer)
    {
        if (activePlayer.HasPassed == false)
        {
            activePlayer.FinishTurn();
            rivalPlayer.StartTurn();
        }
    }

    public void PickPLayerAsFirst(Player Player1, Player Player2)
    {
        System.Random random = new System.Random();
        int choice = random.Next(2);

        if (choice == 0)
            Player1.StartTurn();

        else
            Player2.StartTurn();
    }

    public void BothPLayersHasPassed()
    {
        if (Player1.GamesWon < 2 && Player2.GamesWon < 2)
        {
            NotifyObservers(new GameEventReport(GameEvents.FinishRound, Player1, Player2));
            FinishRound(Player1, Player2);
            NotifyObservers(new GameEventReport(GameEvents.StartRound, Player1, Player2));
        }
    }

    public void FinishRound(Player Player1, Player Player2)
    {
        SetRoundWinner(Player1, Player2);

        Player1.GetNewBattlefield();
        Player2.GetNewBattlefield();

        if (!GameOver)
        {
            Player1.PlayerHand.DrawCard();
            Player2.PlayerHand.DrawCard();

            Player1.PlayerHand.DrawCard();
            Player2.PlayerHand.DrawCard();
            NotifyObservers(new GameEventReport(GameEvents.DrawCard, Player1, Player2));
        }
    }

    private void SetRoundWinner(Player Player1, Player Player2)
    {
        if (Player1.Battlefield.TotalScore > Player2.Battlefield.TotalScore)
            InternalSetRoundWinner(Player1, Player2);

        else if (Player2.Battlefield.TotalScore > Player1.Battlefield.TotalScore)
            InternalSetRoundWinner(Player2, Player1);

        else
        {
            Player1.AddVictory();
            Player2.AddVictory();

            if (Player1.GamesWon == 2 || Player2.GamesWon == 2)
            {
                NotifyObservers(new GameEventReport(GameEvents.FinishGame, Player1, Player2));
                GameOver = true;
            }

            else
            {
                Player1.NewRound();
                Player2.NewRound();

                PickPLayerAsFirst(Player1, Player2);
                NotifyObservers(new GameEventReport(GameEvents.Start, Player1, Player2));
            }
        }
    }

    private void InternalSetRoundWinner(Player winner, Player looser)
    {
        winner.AddVictory();

        if (winner.GamesWon == 2)
        {
            NotifyObservers(new GameEventReport(GameEvents.FinishGame, winner, looser));
            GameOver = true;
        }

        else
        {
            winner.NewRound();
            looser.NewRound();
            winner.StartTurn();
        }
    }

}
public enum GameEvents
{
    Start,
    Summon,
    Invoke,
    PassTurn,
    DecoyEventStart,
    DecoyEventFinish,
    DecoyEventAbort,
    FinishRound,
    StartRound,
    FinishGame,
    DrawCard,
}


