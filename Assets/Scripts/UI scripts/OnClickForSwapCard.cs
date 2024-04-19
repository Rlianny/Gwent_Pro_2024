using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnClickForSwapCard : MonoBehaviour
{
    public void Swap()
    {
        if(gameObject.transform.parent.name == "Player1Hand")
        {
            if(SwappingCards.Player1Swaps < 2)
            {
                SwappingCards.Player1Swaps++;
                

                Card oldCard = gameObject.GetComponent<UICard>().MotherCard;
                Card newCard = Hand.RandomChoice(GameData.Player1.PlayerHand.GameDeck);
                gameObject.GetComponent<UICard>().PrintCard(newCard);
                GameData.Player1.PlayerHand.PlayerHand.Add(newCard);
                GameData.Player1.PlayerHand.PlayerHand.Remove(oldCard);
                GameData.Player1.PlayerHand.GameDeck.Remove(newCard);
            }
        }

        if(gameObject.transform.parent.name == "Player2Hand")
        {
            if(SwappingCards.Player2Swaps < 2)
            {
                SwappingCards.Player2Swaps++;

                Card oldCard = gameObject.GetComponent<UICard>().MotherCard;
                Card newCard = Hand.RandomChoice(GameData.Player2.PlayerHand.GameDeck);
                gameObject.GetComponent<UICard>().PrintCard(newCard);
                GameData.Player2.PlayerHand.PlayerHand.Add(newCard);
                GameData.Player2.PlayerHand.PlayerHand.Remove(oldCard);
                GameData.Player2.PlayerHand.GameDeck.Remove(newCard);
            }
        }
    }
}
