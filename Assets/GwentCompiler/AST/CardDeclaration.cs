using UnityEngine;
using System;
public class CardDeclaration : IProgramNode
{
    public CardTypeDeclaration Type { get; private set; }
    public NameDeclaration Name { get; private set; }
    public CardFactionDeclaration Faction { get; private set; }
    public CardRangeDeclaration Range { get; private set; }
    public CardPowerDeclaration Power { get; private set; }
    public CardEffectDescriptionDeclaration EffectDescription { get; private set; }
    public OnActivation OnActivationField { get; private set; }
    public CardCharacterDescriptionDeclaration CharacterDescription { get; private set; }
    public CardQuoteDeclaration Quote { get; private set; }
    public Token CardLocation { get; private set; }

    public CardDeclaration(Token cardLocation)
    {
        CardLocation = cardLocation;
    }

    public bool SetComponent(ICardComponent component)
    {
        switch (component.GetType().Name)
        {
            case "CardTypeDeclaration":

                if (Type == null)
                {
                    Type = (CardTypeDeclaration)component;
                    return true;
                }
                return false;

            case "NameDeclaration":

                if (Name == null)
                {
                    Name = (NameDeclaration)component;
                    return true;
                }
                return false;

            case "CardFactionDeclaration":

                if (Faction == null)
                {
                    Faction = (CardFactionDeclaration)component;
                    return true;
                }
                return false;

            case "CardRangeDeclaration":

                if (Range == null)
                {
                    Range = (CardRangeDeclaration)component;
                    return true;
                }
                return false;

            case "CardPowerDeclaration":

                if (Power == null)
                {
                    Power = (CardPowerDeclaration)component;
                    return true;
                }
                return false;

            case "CardEffectDescriptionDeclaration":

                if (EffectDescription == null)
                {
                    EffectDescription = (CardEffectDescriptionDeclaration)component;
                    return true;
                }
                return false;

            case "OnActivation":

                if (OnActivationField == null)
                {
                    OnActivationField = (OnActivation)component;
                    return true;
                }
                return false;

            case "CardCharacterDescriptionDeclaration":

                if (CharacterDescription == null)
                {
                    CharacterDescription = (CardCharacterDescriptionDeclaration)component;
                    return true;
                }
                return false;

            case "CardQuoteDeclaration":

                if (Quote == null)
                {
                    Quote = (CardQuoteDeclaration)component;
                    return true;
                }
                return false;

        }
        return false;
    }
}