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

    void Start()
    {
        gameManager = this;

        Player1 = GameData.Player1;
        Player2 = GameData.Player2;

        PickPLayerAsFirst(Player1, Player2);

        if (Player1.IsActive == true)
        {
            Debug.Log($"{Player1.PlayerName} empieza");
        }

        if (Player2.IsActive == true)
        {
            Debug.Log($"{Player2.PlayerName} empieza");
        }

        NotifyObservers(GameEvents.Start);

    }

    void Update()
    {

    }

    public void PlayACard(Card card, string row)
    {
        if (Player1.IsActive == true)
        {
            Player1.DragAndDropMovement(Player1, Player2, card, row);
            Debug.Log($"{Player1.PlayerName} ha movido la carta {card.Name} a la fila {row}");

            if (card.Type == "Señuelo")
            {
                NotifyObservers(GameEvents.DecoyEventStart);
            }

            else
            {
                NotifyObservers(GameEvents.Summon, card);
                ChangeTurn(Player1, Player2);
            }
        }


        else
        {
            Player2.DragAndDropMovement(Player2, Player1, card, row);
            Debug.Log($"{Player2.PlayerName} ha movido la carta {card.Name} a la fila {row}");

            if (card.Type == "Señuelo")
            {
                NotifyObservers(GameEvents.DecoyEventStart);
            }

            else
            {
                NotifyObservers(GameEvents.Summon, card);
                ChangeTurn(Player2, Player1);
            }
        }
    }

    public void UseLeaderSkill()
    {
        if (Player1.IsActive == true)
        {
            Player1.UseLeaderSkill(Player1, Player2);
            NotifyObservers(GameEvents.Summon, Player1.PlayerLeader);
            Debug.Log($"{Player1.PlayerName} ha usado la habilidad del lider");
            ChangeTurn(Player1, Player2);
        }

        else
        {
            Player2.UseLeaderSkill(Player2, Player1);
            NotifyObservers(GameEvents.Summon, Player2.PlayerLeader);
            Debug.Log($"{Player2.PlayerName} ha usado la habilidad del lider");
            ChangeTurn(Player2, Player1);
        }

    }

    public void PassTurn()
    {
        if (Player1.IsActive == true)
        {
            Player1.PassTurn();

            if (Player2.HasPassed == false)
            {
                NotifyObservers(GameEvents.PassTurn);
                Player2.StartTurn();
            }

            else
                BothPLayersHasPassed();
        }

        else if (Player2.IsActive == true)
        {
            Player2.PassTurn();

            if (Player1.HasPassed == false)
            {
                NotifyObservers(GameEvents.PassTurn);
                Player1.StartTurn();
            }

            else
                BothPLayersHasPassed();
        }
    }

    public void FinishDecoyEvent(Card cardToSwap)
    {
        if (Player1.IsActive == true)
        {
            int pos = -1;
            for (int i = 0; i < 3; i++)
            {
                foreach (UnityCard unity in Player1.Battlefield.Battlefield[i])
                {
                    if (unity.Type == "Señuelo")
                    {
                        pos = i;
                    }
                }
            }

            if (pos >= 0)
            {
                if (cardToSwap is UnityCard unityCard)
                {
                    Player1.Battlefield.Battlefield[pos].Remove(unityCard);
                    Player1.PlayerHand.PlayerHand.Add(unityCard);
                    if (unityCard is SilverUnityCard silverUnityCard)
                        silverUnityCard.ActualPower = silverUnityCard.Power;
                    Player1.Battlefield.UpdateBattlefieldInfo();
                    Player2.Battlefield.UpdateBattlefieldInfo();
                    NotifyObservers(GameEvents.DecoyEventFinish, unityCard);
                    ChangeTurn(Player1, Player2);
                }
            }
        }

        else
        {
            int pos = -1;
            for (int i = 0; i < 3; i++)
            {
                foreach (UnityCard unity in Player2.Battlefield.Battlefield[i])
                {
                    if (unity.Type == "Señuelo")
                    {
                        pos = i;
                    }
                }
            }

            if (pos >= 0)
            {
                if (cardToSwap is UnityCard unityCard)
                {
                    Player2.Battlefield.Battlefield[pos].Remove(unityCard);
                    Player2.PlayerHand.PlayerHand.Add(unityCard);
                    if (unityCard is SilverUnityCard silverUnityCard)
                        silverUnityCard.ActualPower = silverUnityCard.Power;
                    Player1.Battlefield.UpdateBattlefieldInfo();
                    Player2.Battlefield.UpdateBattlefieldInfo();
                    NotifyObservers(GameEvents.DecoyEventFinish, unityCard);
                    ChangeTurn(Player2, Player1);
                }
            }
        }
    }

    public void AbortDecoyEvent()
    {
        if (Player1.IsActive == true)
        {
            Player1.Battlefield.UpdateBattlefieldInfo();
            Player2.Battlefield.UpdateBattlefieldInfo();
            NotifyObservers(GameEvents.DecoyEventAbort);
            ChangeTurn(Player1, Player2);
        }

        else
        {
            Player1.Battlefield.UpdateBattlefieldInfo();
            Player2.Battlefield.UpdateBattlefieldInfo();
            NotifyObservers(GameEvents.DecoyEventAbort);
            ChangeTurn(Player2, Player1);
        }
    }

    public void InvokeACard(Card card, string row)
    {
        if (Player1.IsActive == true)
        {
            InvokeMovement(Player1, Player2, card, row);
            Debug.Log($"La carta {card.Name} ha sido invocada por {Player1.PlayerName}");
            NotifyObservers(GameEvents.Invoke, card);
            //ChangeTurn(Player1, Player2);
        }

        else
        {
            InvokeMovement(Player2, Player1, card, row);
            Debug.Log($"La carta {card.Name} ha sido invocada por {Player2.PlayerName}");
            NotifyObservers(GameEvents.Invoke, card);
        }
    }

    public void InvokeMovement(Player activePlayer, Player rivalPlayer, Card card, string Row)
    {
        if (Row == "M" || Row == "R" || Row == "S")
        {
            if (card is IncreaseCard increaseCard)
            {
                activePlayer.Battlefield.IncreaseColumn[activePlayer.Battlefield.RowCorrespondency[Row]] = increaseCard;
                Player1.PlayerHand.RemoveCardFromDeck(card);

                activePlayer.Battlefield.UpdateBattlefieldInfo();
                rivalPlayer.Battlefield.UpdateBattlefieldInfo();
            }

            if (card is WeatherCard weatherCard)
            {
                PlayerBattlefield.WeatherRow[activePlayer.Battlefield.RowCorrespondency[Row]] = weatherCard;
                Player1.PlayerHand.RemoveCardFromDeck(card);

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
            {
                if (Player2.HasPassed == false)
                {
                    Player1.FinishTurn();
                    Player2.StartTurn();
                }
            }

            else
            {
                if (Player1.HasPassed == false)
                {
                    Player2.FinishTurn();
                    Player1.StartTurn();
                }
            }
        }

        else
        {
            throw new ArgumentException("Turno no posible");
        }
    }

    public void PickPLayerAsFirst(Player Player1, Player Player2)
    {
        System.Random random = new System.Random();
        int choice = random.Next(2);

        if (choice == 0)
        {
            Player1.StartTurn();
        }

        else
        {
            Player2.StartTurn();
        }

    }

    public void BothPLayersHasPassed()
    {
        if (Player1.GamesWon < 2 && Player2.GamesWon < 2)
        {
            NotifyObservers(GameEvents.FinishRound);
            FinishRound(Player1, Player2);
            NotifyObservers(GameEvents.StartRound);
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
            //NotifyObservers(GameEvents.DrawCard);

            Player1.PlayerHand.DrawCard();
            Player2.PlayerHand.DrawCard();
            NotifyObservers(GameEvents.DrawCard);

            for (int i = 0; i < Player1.PlayerHand.PlayerHand.Count; i++)
            {
                Debug.Log($"{Player1.PlayerName} tiene a la carta {Player1.PlayerHand.PlayerHand[i].Name} en la mano");
            }

            for (int i = 0; i < Player2.PlayerHand.PlayerHand.Count; i++)
            {
                Debug.Log($"{Player2.PlayerName} tiene a la carta {Player2.PlayerHand.PlayerHand[i].Name} en la mano");
            }
        }
    }

    private void SetRoundWinner(Player Player1, Player Player2)
    {
        if (Player1.Battlefield.TotalScore > Player2.Battlefield.TotalScore)
        {
            Player1.AddVictory();

            if (Player1.GamesWon == 2)
            {
                NotifyObservers(GameEvents.FinishGame);
                GameOver = true;
            }

            else
            {
                Player1.NewRound();
                Player2.NewRound();
                Player1.StartTurn();
            }
        }

        else if (Player2.Battlefield.TotalScore > Player1.Battlefield.TotalScore)
        {
            Player2.AddVictory();

            if (Player2.GamesWon == 2)
            {
                NotifyObservers(GameEvents.FinishGame);
                GameOver = true;
            }

            else
            {
                Player1.NewRound();
                Player2.NewRound();
                Player2.StartTurn();
            }
        }

        else
        {
            Player1.AddVictory();
            Player2.AddVictory();

            if (Player1.GamesWon == 2 || Player2.GamesWon == 2)
            {
                NotifyObservers(GameEvents.FinishGame);
                GameOver = true;
            }

            else
            {
                Player1.NewRound();
                Player2.NewRound();

                PickPLayerAsFirst(Player1, Player2);
                NotifyObservers(GameEvents.Start);
            }
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


