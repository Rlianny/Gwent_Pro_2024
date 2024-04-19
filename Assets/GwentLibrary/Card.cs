using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Security.Cryptography;
using System;

public abstract class Card
{
    string type;
    public string Type // tipo de carta
    {
        get { return TypeClassifier(type); }
        private set { type = value; }
    }
    public string Name { get; private set; }
    public string Faction { get; private set; }
    public string EffectDescription { get; private set; } // descripción del efecto de la carta
    public int EffectNumber { get; private set; } // número en correspondecia con el efecto de la carta
    public string CharacterDescription { get; private set; } // descripción del personaje representado por la carta
    public string Quote { get; private set; }

    /// <summary>
    /// Constructor de la clase Card.
    /// </summary>
    /// <param name="CardInfoArray">Array que contiene la información de la carta extraída de la base de datos.</param>
    public Card(string[] CardInfoArray)
    {
        type = CardInfoArray[0];
        Name = CardInfoArray[1];
        Faction = CardInfoArray[2];
        EffectDescription = CardInfoArray[5];
        EffectNumber = int.Parse(CardInfoArray[6]);
        CharacterDescription = CardInfoArray[7];
        Quote = CardInfoArray[8];
    }

    /// <summary>
    /// Constructor de la clase Card.
    /// </summary>
    /// <param name="card">Carta a partir de la cual se creará una carta con las mismas propiedades.</param>
    /// <remarks>Este constructor está pensado para crear una nueva carta de plata a partir de una carta ya existente.</remarks>
    public Card(Card card)
    {
        type = card.type;
        Name = card.Name;
        Faction = card.Faction;
        EffectDescription = card.EffectDescription;
        EffectNumber = card.EffectNumber;
        CharacterDescription = card.CharacterDescription;
        Quote = card.Quote;
    }

    /// <summary>
    /// Este método activa el efecto de la carta.
    /// </summary>
    /// <param name="ActivePlayer">Jugador que activa el efecto.</param>
    /// <param name="RivalPlayer">Jugador rival en el momento  de activar el efecto.</param>
    /// <param name="Card">Carta a la cual se le activa el efecto.</param>
    public void ActivateEffect(Player ActivePlayer, Player RivalPlayer, Card card)
    {
        Effect effect = Effect.EffectDictionary[EffectNumber];
        effect.TakeEffect(ActivePlayer, RivalPlayer, card);
    }

    /// <summary>
    /// Este método le otorga a la carta un efecto vacío.
    /// </summary>
    public void CancellCardEffect()
    {
        EffectNumber = 17;
    }

    /// <summary>
    /// Este método clasifica la carta según un string clave que obtiene de la base de datos de la colección de cartas.
    /// </summary>
    /// <param name="TipeLetter">String que codifica el tipo de cartas en la base de datos.</param>
    /// <returns>Tipo de carta.</returns>
    private static string TypeClassifier(string TipeLetter)
    {
        switch (TipeLetter)
        {
            case "L":
                return "Líder";

            case "O":
                return "Unidad Héroe";

            case "P":
                return "Unidad de Plata";

            case "A":
                return "Carta de Aumento";

            case "C":
                return "Carta de Clima";

            case "S":
                return "Señuelo";

            case "D":
                return "Carta de Despeje";

            case "Líder":
                return "Líder";

            case "Unidad Héroe":
                return "Unidad Héroe";

            case "Unidad de Plata":
                return "Unidad de Plata";

            case "Carta de Aumento":
                return "Carta de Aumento";

            case "Carta de Clima":
                return "Carta de Clima";

            case "Señuelo":
                return "Señuelo";

            case "Carta de Despeje":
                return "Carta de Despeje";

            default:
                throw new ArgumentException("La carta tiene un tipo no definido");
        }
    }
}