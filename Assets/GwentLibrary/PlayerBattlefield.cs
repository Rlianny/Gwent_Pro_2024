using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.ComponentModel.Design;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;

public class PlayerBattlefield
{
    private List<UnityCard>[] battlefield = new List<UnityCard>[3];
    private IncreaseCard[] increaseColumn = new IncreaseCard[3];
    private static WeatherCard[] weatherRow = new WeatherCard[3];
    public int MeleeRowScore;
    public int RangedRowScore;
    public int SigeeRowScore;
    public int TotalScore { get; private set; }
    public static Dictionary<RowTypes, int> RowCorrespondency { get; private set; } = new()
    {
        {RowTypes.Melee, 0},
        {RowTypes.Ranged, 1},
        {RowTypes.Sigee, 2},
    };

    public PlayerBattlefield()
    {
        battlefield[0] = new();
        battlefield[1] = new();
        battlefield[2] = new();

        UpdateBattlefieldInfo();
    }

    public UnityCard GetCardFromBattlefield(RowTypes row, int position)
    {
        return battlefield[RowCorrespondency[row]][position];
    }

    public IncreaseCard GetIncreaseCardInRow(RowTypes row)
    {
        return increaseColumn[RowCorrespondency[row]];
    }

    public static WeatherCard GetWeatherCardInRow(RowTypes row)
    {
        return weatherRow[RowCorrespondency[row]];
    }

    public List<UnityCard> GetRowFromBattlefield(RowTypes row)
    {
        return battlefield[RowCorrespondency[row]];
    }

    public void AddCardToIncreaseColumn(IncreaseCard increaseCard, RowTypes row)
    {
        increaseColumn[RowCorrespondency[row]] = increaseCard;
    }

    public static void AddCardToWeatherRow(WeatherCard weatherCard, RowTypes row)
    {
        weatherRow[RowCorrespondency[row]] = weatherCard;
    }

    public void AddCardToBattlefiel(UnityCard card, RowTypes row)
    {
        battlefield[RowCorrespondency[row]].Add(card);
    }

    public void RemoveCardFromBattlefield(UnityCard card, RowTypes row)
    {
        battlefield[RowCorrespondency[row]].Remove(card);
    }



    /// <summary>
    /// Este métodod calcula la puntuación de una fila específica sin tener en cuenta factores como climas u aumentos.
    /// </summary>
    /// <param name="Row">La fila a la que se le calculará el puntaje.</param>
    /// <returns>El score de la fila.</returns>
    private static int ScoreCalculator(List<UnityCard> Row)
    {
        int score = 0;

        foreach (UnityCard card in Row)
        {
            if (card is SilverUnityCard silverUnityCard)
                score += silverUnityCard.ActualPower;

            else
                score += card.Power;
        }

        return score;
    }

    /// <summary>
    /// Este método calcula la puntuación total del jugador sumando las puntuaciones de las tres filas en su campo de batalla.
    /// </summary>
    /// <param name="MeleeRowScore">Puntuación de la fila Melee.</param>
    /// <param name="RangedRowScore">Puntuación de la fila Ranged.</param>
    /// <param name="SigeeRowScore">Puntuación de la fila Sigee.</param>
    /// <returns>Sumatoria de los puntos de las unidades.</returns>
    private static int TotalScoreCalculator(int MeleeRowScore, int RangedRowScore, int SigeeRowScore)
    {
        int totalScore = MeleeRowScore + RangedRowScore + SigeeRowScore;

        return totalScore;
    }

    /// <summary>
    /// Actualiza los puntos de cada fila y el puntaje total.
    /// </summary>
    public void UpdateBattlefieldInfo()
    {
        MeleeRowScore = ScoreCalculatorWithModiffiers(RowTypes.Melee);
        RangedRowScore = ScoreCalculatorWithModiffiers(RowTypes.Ranged);
        SigeeRowScore = ScoreCalculatorWithModiffiers(RowTypes.Sigee);
        TotalScore = TotalScoreCalculator(MeleeRowScore, RangedRowScore, SigeeRowScore);
    }

    
    private int ScoreCalculatorWithModiffiers(RowTypes row)
    {
        if (GetRowFromBattlefield(row).Count > 0)
        {
            if (GetWeatherCardInRow(row) == null && GetIncreaseCardInRow(row) == null)
            {
                Debug.Log("No hay climas ni aumentos afectando a la fila" + row.ToString());
                return ScoreCalculator(GetRowFromBattlefield(row));
            }

            else
            {
                if (GetWeatherCardInRow(row) != null)
                {
                    WeatherStatus(row);
                }

                if (GetIncreaseCardInRow(row) != null)
                {
                    IncreaseStatus(row);
                }

                return ScoreCalculator(GetRowFromBattlefield(row));
            }

        }

        else return 0;
    }

    private void WeatherStatus(RowTypes row)
    {
        foreach (UnityCard unityCard in GetRowFromBattlefield(row))
        {
            if (unityCard is SilverUnityCard silverUnityCard)
                silverUnityCard.ActualPower = 1;
        }

        Debug.Log($"Clima aplicado a la fila {row}");
    }

    /// <summary>
    /// Este método aplica el efecto de Aumento a una fila.
    /// </summary>
    /// <param name="rowPos">Fila a la cual se aplicará el aumento.</param>
    private void IncreaseStatus(RowTypes row)
    {
        foreach (UnityCard unityCard in GetRowFromBattlefield(row))
        {
            if (unityCard is SilverUnityCard silverUnityCard)
            {
                if (silverUnityCard.ActualPower == 1 || silverUnityCard.ActualPower == silverUnityCard.Power)
                {
                    silverUnityCard.ActualPower++;
                }
            }
        }


        Debug.Log($"Aumento aplicado a la fila {row}");
    }

    /// <summary>
    /// Este método elimina todas las cartas presnetes en el campo.
    /// </summary>
    public void ClearBattelfield()
    {
        for (int i = 0; i <= 2; i++)
        {
            battlefield[i] = null;
            battlefield[i] = new();
        }
        ClearWeatherRow();
        ClearIncreaseRow();
    }



    /// <summary>
    /// Limpia la fila del clima.
    /// </summary>
    public static void ClearWeatherRow()
    {
        weatherRow = null;
        weatherRow = new WeatherCard[3];
    }

    public void ClearIncreaseRow()
    {
        increaseColumn = null;
        increaseColumn = new IncreaseCard[3];
    }

    public int NumberOfUnitysOnBattlefield()
    {
        int count = 0;

        foreach (List<UnityCard> cardList in battlefield)
        {
            count += cardList.Count;
        }

        return count;
    }
}