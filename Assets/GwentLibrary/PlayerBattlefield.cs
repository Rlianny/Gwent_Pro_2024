using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.ComponentModel.Design;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;

public class PlayerBattlefield
{
    public List<UnityCard>[] Battlefield { get; private set; } = new List<UnityCard>[3];
    public IncreaseCard[] IncreaseColumn { get; private set; } = new IncreaseCard[3];
    public static WeatherCard[] WeatherRow { get; private set; } = new WeatherCard[3];
    public int MeleeRowScore { get; private set; }
    public int RangedRowScore { get; private set; }
    public int SigeeRowScore { get; private set; }
    public int TotalScore { get; private set; }
    public static Dictionary<RowTypes, int> RowCorrespondency { get; private set; } = new()
    {
        {RowTypes.Melee, 0},
        {RowTypes.Ranged, 1},
        {RowTypes.Sigee, 2},
    };

    public PlayerBattlefield()
    {
        Battlefield[0] = new();
        Battlefield[1] = new();
        Battlefield[2] = new();

        UpdateBattlefieldInfo();
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
        MeleeRowScore = ScoreCalculatorWithModiffiers(0);
        RangedRowScore = ScoreCalculatorWithModiffiers(1);
        SigeeRowScore = ScoreCalculatorWithModiffiers(2);
        TotalScore = TotalScoreCalculator(MeleeRowScore, RangedRowScore, SigeeRowScore);
    }

    /// <summary>
    /// Este método calcula el poder total de una fila teniendo en cuenta factores como el clima o el aumento que afectan o no a la fila.
    /// </summary>
    /// <param name="rowPos">Fila de la cual se calculará el poder.</param>
    /// <returns>El poder total de la fila.</returns>
    private int ScoreCalculatorWithModiffiers(int rowPos)
    {
        if (Battlefield[rowPos].Count > 0)
        {
            if (WeatherRow[rowPos] == null && IncreaseColumn[rowPos] == null)
            {
                Debug.Log("No hay climas ni aumentos afectando a la fila" + rowPos.ToString());
                return ScoreCalculator(Battlefield[rowPos]);
            }

            else
            {
                if (WeatherRow[rowPos] != null)
                {
                    WeatherStatus(rowPos);
                }

                if (IncreaseColumn[rowPos] != null)
                {
                    IncreaseStatus(rowPos);
                }

                return ScoreCalculator(Battlefield[rowPos]);
            }

        }

        else return 0;
    }

    /// <summary>
    /// Este método aplica el efecto de Clima a una fila.
    /// </summary>
    /// <param name="rowPos">Fila a la cual se aplicará el clima.</param>
    private void WeatherStatus(int rowPos)
    {
        foreach (UnityCard unityCard in Battlefield[rowPos])
        {
            if (unityCard is SilverUnityCard silverUnityCard)
                silverUnityCard.ActualPower = 1;
        }

        Debug.Log($"Clima aplicado a la fila {rowPos}");
    }

    /// <summary>
    /// Este método aplica el efecto de Aumento a una fila.
    /// </summary>
    /// <param name="rowPos">Fila a la cual se aplicará el aumento.</param>
    private void IncreaseStatus(int rowPos)
    {
        foreach (UnityCard unityCard in Battlefield[rowPos])
        {
            if (unityCard is SilverUnityCard silverUnityCard)
            {
                if (silverUnityCard.ActualPower == 1 || silverUnityCard.ActualPower == silverUnityCard.Power)
                {
                    silverUnityCard.ActualPower++;
                }
            }
        }


        Debug.Log($"Aumento aplicado a la fila {rowPos}");
    }

    /// <summary>
    /// Este método elimina todas las cartas presnetes en el campo.
    /// </summary>
    public void ClearBattelfield()
    {
        for (int i = 0; i <= 2; i++)
        {
            Battlefield[i] = null;
            Battlefield[i] = new();
        }
        ClearWeatherRow();
        ClearIncreaseRow();
    }



    /// <summary>
    /// Limpia la fila del clima.
    /// </summary>
    public static void ClearWeatherRow()
    {
        WeatherRow = null;
        WeatherRow = new WeatherCard[3];
    }

    public void ClearIncreaseRow()
    {
        IncreaseColumn = null;
        IncreaseColumn = new IncreaseCard[3];
    }
}