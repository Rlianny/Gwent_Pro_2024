using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Security.Cryptography;
using System;
using System.Diagnostics;
using System.ComponentModel.Design;
using UnityEditor;
using System.Data;

public abstract class Effect
{
    public static Dictionary<int, Effect> EffectDictionary { get; private set; } = new()
    {
        {0, new PersonalizedEffect()},
        {1, new IncreaseMeleeRowEffect()},
        {2, new IncreaseRangedRowEffect()},
        {3, new IncreaseSigeeRowEffect()},
        {4, new WeatherMeleeRowEffect()},
        {5, new WeatherRangedRowEffect()},
        {6, new WeatherSigeeRowEffect()},
        {7, new DrawEffect()},
        {8, new DeleteTheLeastEffect()},
        {9, new DeleteTheMostEfect()},
        {10, new CloseBondEffect()},
        {11, new ClearARowEffect()},
        {12, new ClearanceEffect()},
        {13, new AverageEffect()},
        {14, new DecoyEffect()},
        {15, new SupremacyEffect()},
        {16, new CancelEffect()},
        {17, new VoidEffect()},
    };

    protected static Dictionary<string, CompiledEffect> AllCompiledEffects { get; } = new();

    public static bool RegisterEffect(CompiledEffect compiledEffect)
    {
        if (!AllCompiledEffects.ContainsKey(compiledEffect.Name))
        {
            AllCompiledEffects.Add(compiledEffect.Name, compiledEffect);
            return true;
        }
        else return false;
    }

    public static CompiledEffect GetCompiledEffect(string name)
    {
        if (AllCompiledEffects.ContainsKey(name)) return AllCompiledEffects[name];
        else return null;
    }

    public static bool CheckEffectExistance(string effectName) => AllCompiledEffects.ContainsKey(effectName);

    /// <summary>
    /// Este método manipula el campo de batalla realizando el efecto de la carta.
    /// </summary>
    /// <param name="ActivePlayer">El jugador activo en el momento de la llamada del método.</param>
    /// <param name="RivalPlayer">El jugador rival al momento de la llamada del método.</param>
    /// <param name="card">La carta desde la cual se activa el efecto.</param>
    public abstract void TakeEffect(Player ActivePlayer, Player RivalPlayer, Card card);
}

public class PersonalizedEffect : Effect
{
    private CompiledEffect baseEffect;

    public override void TakeEffect(Player ActivePlayer, Player RivalPlayer, Card card)
    {
        foreach (var effect in card.OnActivation)
        {
            InterpretEffect(ActivePlayer, RivalPlayer, card, effect);
        }
    }

    private void InterpretEffect(Player ActivePlayer, Player RivalPlayer, Card card, EffectActivation effect, List<Card> parentSource = null)
    {
        PersonalizeEffect(effect.EffectName);
        UnityEngine.Debug.Log($"Se ejecutará el efecto {effect.EffectName}");

        List<Card> targets = GetTargets(ActivePlayer, RivalPlayer, card, effect);

        if (targets == null)
        {
            if (parentSource != null) targets = parentSource;
            else return;
        }

        Context context = new();

        Interpreter interpreter = new();
        interpreter.Environment.Assign(baseEffect.ContextId, context);
        interpreter.Environment.Assign(baseEffect.TargetsId, targets);

        foreach (var pair in effect.Parameters)
        {
            interpreter.Environment.Assign(pair.Key.Name, pair.Value);
        }

        interpreter.Execute(baseEffect.Block);

        if (effect.PostAction != null)
        {
            InterpretEffect(ActivePlayer, RivalPlayer, card, effect.PostAction, targets);
        }
    }

    private List<Card> GetTargets(Player ActivePlayer, Player RivalPlayer, Card card, EffectActivation effect, List<Card> parentSource = null)
    {
        List<Card> targets = new();
        List<Card> source = null;

        switch (effect.SelectorSource)
        {
            case "hand":
                source = Context.Hand;
                break;
            case "otherHand":
                source = Context.HandOfPlayer(RivalPlayer.PlayerID);
                break;
            case "deck":
                source = Context.Deck;
                break;
            case "otherDeck":
                source = Context.DeckOfPlayer(RivalPlayer.PlayerID);
                break;
            case "parent":
                source = parentSource;
                break;
            case "board":
                source = Context.BoardList;
                break;
            case "field":
                source = Context.FieldList;
                break;
            case "otherField":
                source = Context.FieldOfPlayerList(RivalPlayer.PlayerID);
                break;
        }

        if (source == null) return null;

        foreach (Card sourceCard in source)
        {
            if (sourceCard is SilverUnityCard && effect.SelectorPredicate.Invoke(sourceCard))
            {
                targets.Add(sourceCard);
                if (effect.SelectorSingle) break;
            }
        }

        return targets;
    }

    private void PersonalizeEffect(string effectName)
    {
        baseEffect = AllCompiledEffects[effectName];
    }
}

public abstract class IncreaseEffect : Effect
{

}

/// <summary>
/// Aumenta en 1 el poder de cada Unidad de plata en la fila Melee del jugador activo al momento de la llamada del método.
/// </summary>
public class IncreaseMeleeRowEffect : IncreaseEffect
{
    public override void TakeEffect(Player ActivePlayer, Player RivalPlayer, Card card)
    {
        Card toInvoke = null;

        if (ActivePlayer.Battlefield.GetIncreaseCardInRow(RowTypes.Melee) == null)
        {
            foreach (Card toFind in ActivePlayer.PlayerHand.GameDeck)
            {
                if (toFind is IncreaseCard increaseCard && increaseCard.Row == RowTypes.Melee)
                {
                    toInvoke = increaseCard;
                }
            }

        }

        if (toInvoke != null)
        {
            GameManager.gameManager.InvokeACard(toInvoke, RowTypes.Melee);
            return;
        }
    }
}

/// <summary>
/// Aumenta en 1 el poder de cada Unidad de plata en la fila Ranged del jugador activo al momento de la llamada del método.
/// </summary>
public class IncreaseRangedRowEffect : IncreaseEffect
{
    public override void TakeEffect(Player ActivePlayer, Player RivalPlayer, Card card)
    {
        Card toInvoke = null;

        if (ActivePlayer.Battlefield.GetIncreaseCardInRow(RowTypes.Ranged) == null)
        {
            foreach (Card toFind in ActivePlayer.PlayerHand.GameDeck)
            {
                if (toFind is IncreaseCard increaseCard && increaseCard.Row == RowTypes.Ranged)
                {
                    toInvoke = increaseCard;
                }
            }

        }

        if (toInvoke != null)
        {
            GameManager.gameManager.InvokeACard(toInvoke, RowTypes.Ranged);
            return;
        }
    }
}

/// <summary>
/// Aumenta en 1 el poder de cada Unidad de plata en la fila Sigee del jugador activo al momento de la llamada del método.
/// </summary>
public class IncreaseSigeeRowEffect : IncreaseEffect
{
    public override void TakeEffect(Player ActivePlayer, Player RivalPlayer, Card card)
    {
        Card toInvoke = null;

        if (ActivePlayer.Battlefield.GetIncreaseCardInRow(RowTypes.Siege) == null)
        {
            foreach (Card toFind in ActivePlayer.PlayerHand.GameDeck)
            {
                if (toFind is IncreaseCard increaseCard && increaseCard.Row == RowTypes.Siege)
                {
                    toInvoke = increaseCard;
                }
            }

        }

        if (toInvoke != null)
        {
            GameManager.gameManager.InvokeACard(toInvoke, RowTypes.Siege);
            return;
        }
    }
}

public abstract class WeatherEffect : Effect
{

}

/// <summary>
/// Disminuye a 1 el poder de cada Unidad de plata en la fila Melee de ambos jugadores.
/// </summary>
public class WeatherMeleeRowEffect : WeatherEffect
{
    public override void TakeEffect(Player ActivePlayer, Player RivalPlayer, Card card)
    {
        Card toInvoke = null;

        if (PlayerBattlefield.GetWeatherCardInRow(RowTypes.Melee) == null)
        {
            foreach (Card toFind in ActivePlayer.PlayerHand.GameDeck)
            {
                if (toFind is WeatherCard weatherCard && weatherCard.Row == RowTypes.Melee)
                {
                    toInvoke = weatherCard;
                }
            }
        }

        if (toInvoke != null)
        {
            GameManager.gameManager.InvokeACard(toInvoke, RowTypes.Melee);
            return;
        }

    }
}

/// <summary>
/// Disminuye a 1 el poder de cada Unidad de plata en la fila Ranged de ambos jugadores.
/// </summary>
public class WeatherRangedRowEffect : WeatherEffect
{
    public override void TakeEffect(Player ActivePlayer, Player RivalPlayer, Card card)
    {
        Card toInvoke = null;

        if (PlayerBattlefield.GetWeatherCardInRow(RowTypes.Ranged) == null)
        {
            foreach (Card toFind in ActivePlayer.PlayerHand.GameDeck)
            {
                if (toFind is WeatherCard weatherCard && weatherCard.Row == RowTypes.Ranged)
                {
                    toInvoke = weatherCard;
                }
            }
        }

        if (toInvoke != null)
        {
            GameManager.gameManager.InvokeACard(toInvoke, RowTypes.Ranged);
            return;
        }
    }
}

/// <summary>
/// Disminuye a 1 el poder de cada Unidad de plata en la fila Sigee de ambos jugadores.
/// </summary>
public class WeatherSigeeRowEffect : WeatherEffect
{
    public override void TakeEffect(Player ActivePlayer, Player RivalPlayer, Card card)
    {
        Card toInvoke = null;

        if (PlayerBattlefield.GetWeatherCardInRow(RowTypes.Siege) == null)
        {
            foreach (Card toFind in ActivePlayer.PlayerHand.GameDeck)
            {
                if (toFind is WeatherCard weatherCard && weatherCard.Row == RowTypes.Siege)
                {
                    toInvoke = weatherCard;
                }
            }
        }

        if (toInvoke != null)
        {
            GameManager.gameManager.InvokeACard(toInvoke, RowTypes.Siege);
            return;
        }
    }
}

/// <summary>
/// Roba una carta del mazo del jugador activo al momento de la llamada del método y la añade a la mano de dicho jugador.
/// </summary>
public class DrawEffect : Effect
{
    public override void TakeEffect(Player ActivePlayer, Player RivalPlayer, Card card)
    {
        ActivePlayer.PlayerHand.DrawCard();
    }
}

public abstract class DeleteEffect : Effect
{

}

/// <summary>
/// Elimina la Unidad de plata con menos poder del campo del jugador rival al momento de la llamada del método. 
/// <remarks> En caso de que el poder mínimo sea el mismo para varias Unidades de plata, solo se eliminará una de ellas. </remarks>
/// </summary>
public class DeleteTheLeastEffect : DeleteEffect
{
    public override void TakeEffect(Player ActivePlayer, Player RivalPlayer, Card card)
    {
        int minPower = int.MaxValue;
        int minPowerIndex = -1;
        int pos = -1;
        SilverUnityCard cardToDelete = null;

        for (int i = 0; i < 3; i++)
        {
            foreach (var unity in RivalPlayer.Battlefield.GetRowFromBattlefield(Tools.RowForIndex[i]))
            {
                if (unity is SilverUnityCard silverUnityCard && silverUnityCard.ActualPower < minPower)
                {
                    cardToDelete = silverUnityCard;
                    minPower = silverUnityCard.ActualPower;
                    minPowerIndex = i;
                    pos = 1;
                }
            }
        }

        if (pos != -1)
        {
            if (pos == 0)
            {
                if (cardToDelete != null)
                {
                    ActivePlayer.Battlefield.RemoveCardFromBattlefield(cardToDelete, Tools.RowForIndex[minPowerIndex]);
                }
            }
            else
            {
                if (cardToDelete != null)
                {
                    RivalPlayer.Battlefield.RemoveCardFromBattlefield(cardToDelete, Tools.RowForIndex[minPowerIndex]);
                }
            }

        }
    }
}

/// <summary>
/// Elimina la Unidad de plata con más poder de todo el campo de batalla. 
/// <remarks> En caso de que el poder máximo sea el mismo para varias Unidades de plata, solo se eliminará una de ellas. </remarks>
/// </summary>
public class DeleteTheMostEfect : DeleteEffect
{
    public override void TakeEffect(Player ActivePlayer, Player RivalPlayer, Card card)
    {
        int maxPower = int.MinValue;
        int maxPowerIndex = -1;
        int pos = -1;
        SilverUnityCard cardToDelete = null;

        for (int i = 0; i < 3; i++)
        {
            foreach (var unity in RivalPlayer.Battlefield.GetRowFromBattlefield(Tools.RowForIndex[i]))
            {
                if (unity is SilverUnityCard silverUnityCard && silverUnityCard.ActualPower > maxPower)
                {
                    cardToDelete = silverUnityCard;
                    maxPower = silverUnityCard.ActualPower;
                    maxPowerIndex = i;
                    pos = 0;
                }
            }
        }

        for (int i = 0; i < 3; i++)
        {
            foreach (var unity in ActivePlayer.Battlefield.GetRowFromBattlefield(Tools.RowForIndex[i]))
            {
                if (unity is SilverUnityCard silverUnityCard && silverUnityCard.ActualPower > maxPower)
                {
                    cardToDelete = silverUnityCard;
                    maxPower = silverUnityCard.ActualPower;
                    maxPowerIndex = i;
                    pos = 1;
                }
            }
        }

        if (pos != -1)
        {
            if (pos == 0)
            {
                if (cardToDelete != null)
                {
                    RivalPlayer.Battlefield.RemoveCardFromBattlefield(cardToDelete, Tools.RowForIndex[maxPowerIndex]);
                }
            }
            else
            {
                if (cardToDelete != null)
                {
                    ActivePlayer.Battlefield.RemoveCardFromBattlefield(cardToDelete, Tools.RowForIndex[maxPowerIndex]);
                }
            }

        }
    }
}

/// <summary>
/// Multiplica por n el poder de cada carta igual a la carta que llama al método, siendo n la cantidad de apariciones de dicha carta en el campo del jugador activo al momento de la llamada del método.
/// </summary>
public class CloseBondEffect : Effect
{
    public override void TakeEffect(Player ActivePlayer, Player RivalPlayer, Card card)
    {
        int n = 0;

        for (int i = 0; i < 3; i++)
        {
            List<UnityCard> cardList = ActivePlayer.Battlefield.GetRowFromBattlefield(Tools.RowForIndex[i]);

            if (cardList.Count > 0)
            {
                foreach (var unityCard in cardList)
                {
                    if (unityCard.Name == card.Name)
                        n++;
                }
            }
        }

        for (int i = 0; i < 3; i++)
        {
            List<UnityCard> cardList = ActivePlayer.Battlefield.GetRowFromBattlefield(Tools.RowForIndex[i]);

            if (cardList.Count > 0)
            {
                foreach (var unityCard in cardList)
                {
                    if (unityCard.Name == card.Name && unityCard is SilverUnityCard silverUnityCard)
                        silverUnityCard.ActualPower *= n;
                }
            }
        }

    }

}

/// <summary>
/// Elimina todas las Unidades de plata de la fila con menos unidades del campo. 
/// <remarks> En caso de que el número mínimo de unidades en una fila sea el mismo para varias filas, solo se limpiará una fila. </remarks>
/// </summary>
public class ClearARowEffect : Effect
{
    public override void TakeEffect(Player ActivePlayer, Player RivalPlayer, Card card)
    {
        List<UnityCard> listToClear = null;
        int minCount = int.MaxValue;


        for (int i = 0; i < 3; i++)
        {
            List<UnityCard> cardList = ActivePlayer.Battlefield.GetRowFromBattlefield(Tools.RowForIndex[i]);

            if (cardList.Count > 0 && cardList.Count < minCount && Tools.NumberOfSilversInRow(cardList) > 0)
            {
                minCount = cardList.Count;
                listToClear = cardList;
            }
        }

        for (int i = 0; i < 3; i++)
        {
            List<UnityCard> cardList = ActivePlayer.Battlefield.GetRowFromBattlefield(Tools.RowForIndex[i]);

            if (cardList.Count > 0 && cardList.Count < minCount && Tools.NumberOfSilversInRow(cardList) > 0)
            {
                minCount = cardList.Count;
                listToClear = cardList;
            }
        }

        if (listToClear != null)
        {
            for (int i = listToClear.Count - 1; i >= 0; i--)
            {
                if (listToClear[i] is SilverUnityCard)
                {
                    listToClear.RemoveAt(i);
                }
            }
        }
    }
}

/// <summary>
/// Elimina todos los climas del campo de batalla y devuelve a cada Unidad de plata su poder original.
/// </summary>
public class ClearanceEffect : Effect
{
    public override void TakeEffect(Player ActivePlayer, Player RivalPlayer, Card card)
    {
        PlayerBattlefield.ClearWeatherRow();

        for (int i = 0; i < 3; i++)
        {
            foreach (UnityCard unityCard in ActivePlayer.Battlefield.GetRowFromBattlefield(Tools.RowForIndex[i]))
            {
                if (unityCard is SilverUnityCard silverUnityCard)
                    silverUnityCard.ActualPower = silverUnityCard.Power;
            }

            foreach (UnityCard unityCard in RivalPlayer.Battlefield.GetRowFromBattlefield(Tools.RowForIndex[i]))
            {
                if (unityCard is SilverUnityCard silverUnityCard)
                    silverUnityCard.ActualPower = silverUnityCard.Power;
            }
        }
    }
}

/// <summary>
/// Calcula el promedio entre los poderes de todas las unidades del campo de batalla e iguala el poder actual de las Unidades de plata de todo el campo a ese promedio.
/// </summary>
public class AverageEffect : Effect
{
    public override void TakeEffect(Player ActivePlayer, Player RivalPlayer, Card card)
    {
        int average;

        int cardsInField = ActivePlayer.Battlefield.NumberOfUnitysOnBattlefield() + RivalPlayer.Battlefield.NumberOfUnitysOnBattlefield();
        average = (ActivePlayer.Battlefield.TotalScore + RivalPlayer.Battlefield.TotalScore) / cardsInField;

        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < ActivePlayer.Battlefield.GetRowFromBattlefield(Tools.RowForIndex[i]).Count; j++)
            {
                if (ActivePlayer.Battlefield.GetCardFromBattlefield(Tools.RowForIndex[i], j) is SilverUnityCard silverUnityCard)
                    silverUnityCard.ActualPower = average;
            }

            for (int k = 0; k < RivalPlayer.Battlefield.GetRowFromBattlefield(Tools.RowForIndex[i]).Count; k++)
            {
                if (RivalPlayer.Battlefield.GetCardFromBattlefield(Tools.RowForIndex[i], k) is SilverUnityCard silverUnityCard)
                    silverUnityCard.ActualPower = average;
            }
        }
    }
}

public class DecoyEffect : Effect
{
    public override void TakeEffect(Player ActivePlayer, Player RivalPlayer, Card card)
    {

    }
}

/// <summary>
/// Reduce a 1 el poder de todas las Unidades de plata en el campo del jugador rival al momento de la llamada del método.
/// </summary>
public class SupremacyEffect : Effect
{
    public override void TakeEffect(Player ActivePlayer, Player RivalPlayer, Card card)
    {
        for (int i = 0; i < 3; i++)
        {
            foreach (UnityCard unityCard in RivalPlayer.Battlefield.GetRowFromBattlefield(Tools.RowForIndex[i]))
            {
                if (unityCard is SilverUnityCard silverUnityCard)
                    silverUnityCard.ActualPower = 1;
            }
        }
    }
}

/// <summary>
/// Cancela el efecto de uno o ambos líderes.
/// </summary>
public class CancelEffect : Effect
{
    public override void TakeEffect(Player ActivePlayer, Player RivalPlayer, Card card)
    {
        if (ActivePlayer.PlayerLeader.EffectNumber == 16 && RivalPlayer.PlayerLeader.EffectNumber == 16)
        {
            ActivePlayer.PlayerLeader.CancellCardEffect();
            RivalPlayer.PlayerLeader.CancellCardEffect();
        }

        else
        {
            RivalPlayer.PlayerLeader.CancellCardEffect();
        }
    }
}

public class VoidEffect : Effect
{
    public override void TakeEffect(Player ActivePlayer, Player RivalPlayer, Card card)
    {

    }
}

