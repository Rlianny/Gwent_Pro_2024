using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CardShowerManager : MonoBehaviour
{
    public static DeckCreator actualDeck;
    public GridLayoutGroup DeckShower;
    public TextMeshProUGUI StringToChangeDeck;
    public GameObject CardPrefab;
    public GameObject SlotPrefab;

    // Start is called before the first frame update
    void Start()
    {
        actualDeck = PlayerDeckManager.actualDeck;
        ShowDeck("All");
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ShowDeck(string type)
    {
        if (actualDeck != null)
        {
            CleanDeck();

            List<Card> cardsToShow = new List<Card>();

            foreach (Card card in actualDeck.CardDeck)
            {

                if (type == "Carta de Aumento")
                {
                    if (card.Type == CardTypes.Carta_de_Aumento)
                        cardsToShow.Add(card);

                    StringToChangeDeck.text = type;
                }

                if (type == "Carta de Clima")
                {
                    if (card.Type == CardTypes.Carta_de_Clima)
                        cardsToShow.Add(card);

                    StringToChangeDeck.text = type;
                }

                if (type == "All")
                {
                    if (actualDeck.CardDeck.Contains(card))
                        cardsToShow.Add(card);

                    StringToChangeDeck.text = "Todas las cartas";
                }

                if (type == "Melee")
                {
                    if (card is UnityCard unityCard && unityCard.Row.Contains(RowTypes.Melee))
                        cardsToShow.Add(card);

                    StringToChangeDeck.text = "Ataque cuerpo a cuerpo";
                }

                if (type == "Ranged")
                {
                    if (card is UnityCard unityCard && unityCard.Row.Contains(RowTypes.Ranged))
                        cardsToShow.Add(card);

                    StringToChangeDeck.text = "Ataque a Distancia";
                }

                if (type == "Sigee")
                {
                    if (card is UnityCard unityCard && unityCard.Row.Contains(RowTypes.Siege))
                        cardsToShow.Add(card);

                    StringToChangeDeck.text = "Asedio";
                }

                if (type == "Decoy")
                {
                    if (card.Name == "Mr. Poopybutthole")
                        cardsToShow.Add(card);

                    StringToChangeDeck.text = "Señuelo";
                }
            }

            List<string> CardsWithoutReps = new();
            foreach (Card card1 in cardsToShow)
            {
                if (!CardsWithoutReps.Contains(card1.Name))
                    CardsWithoutReps.Add(card1.Name);
            }

            for (int i = 0; i < CardsWithoutReps.Count; i++)
            {
                var newCard = Instantiate(CardPrefab, new Vector3(0, 0, 0), Quaternion.identity);       // se instancia el prefab de la carta de la UI
                UICard ui = newCard.GetComponent<UICard>();
                ui.PrintCard(getCardWithTheName(CardsWithoutReps[i], cardsToShow)); // se le asigna una carta a la que representar
                GameObject slot = Instantiate(SlotPrefab, new Vector3(0, 0, 0), Quaternion.identity);
                slot.transform.SetParent(DeckShower.transform);
                newCard.transform.SetParent(slot.transform.GetChild(0).transform);        // se le otorga un slot en la escena
            }
        }
    }

    private Card getCardWithTheName(string name, List<Card> cardsToShow)
    {
        foreach (Card card in cardsToShow)
        {
            if (card.Name == name)
                return card;
        }

        return null;
    }

    private void CleanDeck()
    {
        for (int i = 0; i < DeckShower.transform.childCount; i++)
        {
            GameObject slot = DeckShower.transform.GetChild(i).gameObject;
            Destroy(slot.gameObject);
        }
    }

}
