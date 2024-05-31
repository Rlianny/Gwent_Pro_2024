using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Security.Cryptography;
using System;
using TMPro;

public abstract class Card
{
    public string type;
    public CardTypes Type // tipo de carta
    {
        get { return TypeClassifier(type); }
        private set { type = TypeClassifier(type).ToString().Replace('_', ' '); }
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
        Type = card.Type;
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
    private static CardTypes TypeClassifier(string TipeLetter)
    {
        switch (TipeLetter)
        {
            case "L":
                return CardTypes.Líder;

            case "O":
                return CardTypes.Unidad_Héroe;

            case "P":
                return CardTypes.Unidad_de_Plata;

            case "A":
                return CardTypes.Carta_de_Aumento;

            case "C":
                return CardTypes.Carta_de_Clima;

            case "S":
                return CardTypes.Señuelo;

            case "D":
                return CardTypes.Carta_de_Despeje;

            case "Líder":
                return CardTypes.Líder;

            case "Unidad Héroe":
                return CardTypes.Unidad_Héroe;

            case "Unidad de Plata":
                return CardTypes.Unidad_de_Plata;

            case "Carta de Aumento":
                return CardTypes.Carta_de_Aumento;

            case "Carta de Clima":
                return CardTypes.Carta_de_Clima;

            case "Señuelo":
                return CardTypes.Señuelo;

            case "Carta de Despeje":
                return CardTypes.Carta_de_Despeje;

            default:
                throw new ArgumentException("La carta tiene un tipo no definido");
        }
    }
}