using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEditor;

public class DeckInteraction : MonoBehaviour
{
    public GridLayoutGroup Collection;
    public GridLayoutGroup Deck;

    private void Start()
    {
        Deck = transform.root.Find("DeckContainer").transform.GetChild(0).GetComponent<GridLayoutGroup>();
        Collection = transform.root.Find("CollectionContainer").transform.GetChild(0).GetComponent<GridLayoutGroup>();
    }

    public void AddOrRemoveCard()
    {
        GameObject manager = transform.root.Find("CreateDeckManager").gameObject;       // se accede al Manager de la escena
        CreatingDeck creatingDeck = manager.GetComponent<CreatingDeck>();       // se accede al script CreatingDeck

        GameObject prefab = creatingDeck.CardPrefab;        // se accede al prefab de las cartas contenido en creating deck

        UICard uiCard = this.GetComponent<UICard>();
        Card card = uiCard.MotherCard;      // se accede a las propiedades de la carta

        if (this.transform.parent.parent.parent.name == "DeckContainer")
        {
            if (CreatingDeck.actualDeck.CardActualAppearances(card) == 1)
            {
                CreatingDeck.actualDeck.RemoveCardToMyDeck(card);
                Destroy(transform.parent.transform.GetChild(0).gameObject);
            }

            else
            {
                CreatingDeck.actualDeck.RemoveCardToMyDeck(card);
            }
        }

        else if (this.transform.parent.parent.parent.name == "CollectionContainer")
        {
            if (CreatingDeck.actualDeck.CardActualAppearances(card) > 0)
            {
                CreatingDeck.actualDeck.DuplicateCard(card);
            }

            else
            {
                var newCard = Instantiate(prefab, new Vector3(0, 0, 0), Quaternion.identity);       // se instancia el prefab
                UICard ui = newCard.GetComponent<UICard>();
                ui.PrintCard(card);  // se establece como carta que será representada la carta que acabamos de soltar

                GameObject slot = GetEmptySlot();

                newCard.transform.SetParent(slot.transform);       // se le otorga el slot que tenía la carta arrastrada originalmente
                newCard.transform.position = slot.transform.position;     // se le otorga la posición original
                Debug.Log($"La carta {card.Name} ha sido añadida al deck");

                CreatingDeck.actualDeck.AddCardToMyDeck(card);      // se añade la carta a la lista de cartas del deck
            }
        }
    }

    private GameObject GetEmptySlot()
    {
        GameObject slot;

        for(int i = 0; i < Deck.transform.childCount; i++)
        {
            if(Deck.transform.GetChild(i).transform.childCount == 0)
            {
                slot = Deck.transform.GetChild(i).gameObject;
                return slot;
            }
        }

        return null;
    }
}
