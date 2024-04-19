using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CreatingDeck : MonoBehaviour
{
    private CardsCollection cardsCollection;
    public static DeckCreator actualDeck;

    public GameObject CardPrefab;
    public GridLayoutGroup CollectionShower;
    public GridLayoutGroup DeckShower;
    public Image LeaderImage;
    public TextMeshProUGUI CardsInHand;
    public TextMeshProUGUI UnityCards;
    public TextMeshProUGUI TotalForce;
    public TextMeshProUGUI HeroCards;
    public TextMeshProUGUI StringToChangeCollection;
    public TextMeshProUGUI StringToChangeDeck;
    public Button InitButton;

    void Start()
    {
        // string path = System.IO.Path.Combine(Application.dataPath, "CardsCollection");
        string path = "CardsCollection";
        cardsCollection = new CardsCollection(CardsCreator.GetCardInfoList(path));

        InitButton.interactable = false;

        CardsInHand.text = "";
        UnityCards.text = "";
        TotalForce.text = "";
        HeroCards.text = "";
        HideImage();

        actualDeck = null;
    }


    void Update()
    {
        if (actualDeck != null)
        {
            CardsInHand.text = actualDeck.CardsTotalNumber.ToString();
            UnityCards.text = actualDeck.UnityCardsTotalNumber.ToString();
            TotalForce.text = actualDeck.UnityPowerTotalNumber.ToString();
            HeroCards.text = actualDeck.HeroCardsTotalNumber.ToString();

            if (actualDeck.CardsTotalNumber >= 25)
                InitButton.interactable = true;

            else
                InitButton.interactable = false;
        }
    }


    public void GenerateDeck(string Faction)
    {
        actualDeck = null;
        actualDeck = new DeckCreator(Faction, cardsCollection.AllFactions, cardsCollection.AllLeaders);

        LeaderImage.sprite = Resources.Load<Sprite>(actualDeck.DeckLeader.Name);
        ShowImage();
        ShowCollection("All");
        ShowDeck("All");
        StringToChangeCollection.text = "Todas las cartas";
        StringToChangeDeck.text = "Todas las cartas";
    }


    public void ShowCollection(string type)
    {
        if (actualDeck != null)
        {
            CleanCollection();

            List<Card> cardsToShow = new List<Card>();

            foreach (Card card in cardsCollection.Collection)
            {
                if (card.Type != "Líder")
                {

                    if (type == "Carta de Aumento" || type == "Carta de Clima")
                    {
                        if (card.Type == type && (card.Faction == "Neutral" || card.Faction == actualDeck.Faction))
                            cardsToShow.Add(card);

                        StringToChangeCollection.text = type;
                    }

                    if (type == "All")
                    {
                        if (card.Faction == "Neutral" || card.Faction == actualDeck.Faction)
                            cardsToShow.Add(card);

                        StringToChangeCollection.text = "Todas las cartas";
                    }

                    if (type == "Melee")
                    {
                        if (card is UnityCard unityCard && unityCard.Row.Contains("M") && (card.Faction == "Neutral" || card.Faction == actualDeck.Faction))
                            cardsToShow.Add(card);

                        StringToChangeCollection.text = "Ataque cuerpo a cuerpo";
                    }

                    if (type == "Ranged")
                    {
                        if (card is UnityCard unityCard && unityCard.Row.Contains("R") && (card.Faction == "Neutral" || card.Faction == actualDeck.Faction))
                            cardsToShow.Add(card);

                        StringToChangeCollection.text = "Ataque a distancia";
                    }

                    if (type == "Sigee")
                    {
                        if (card is UnityCard unityCard && unityCard.Row.Contains("S") && (card.Faction == "Neutral" || card.Faction == actualDeck.Faction))
                            cardsToShow.Add(card);

                        StringToChangeCollection.text = "Asedio";
                    }

                    if (type == "Decoy")
                    {
                        if (card.Name == "Mr. Poopybutthole" && (card.Faction == "Neutral" || card.Faction == actualDeck.Faction))
                            cardsToShow.Add(card);

                        StringToChangeCollection.text = "Señuelo";
                    }
                }
            }

            for (int i = 0; i < cardsToShow.Count; i++)
            {
                var newCard = Instantiate(CardPrefab, new Vector3(0, 0, 0), Quaternion.identity);       // se instancia el prefab de la carta de la UI
                UICard ui = newCard.GetComponent<UICard>();
                ui.PrintCard(cardsToShow[i]);     // se le asigna una carta a la que representar
                GameObject slot = CollectionShower.transform.GetChild(i).gameObject;
                newCard.transform.SetParent(slot.transform);        // se le otorga un slot en la escena
            }
        }
    }

    public void ShowDeck(string type)
    {
        if (actualDeck != null)
        {
            CleanDeck();

            List<Card> cardsToShow = new List<Card>();

            foreach (Card card in actualDeck.CardDeck)
            {


                if (type == "Carta de Aumento" || type == "Carta de Clima")
                {
                    if (card.Type == type)
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
                    if (card is UnityCard unityCard && unityCard.Row.Contains("M"))
                        cardsToShow.Add(card);

                    StringToChangeDeck.text = "Ataque cuerpo a cuerpo";
                }

                if (type == "Ranged")
                {
                    if (card is UnityCard unityCard && unityCard.Row.Contains("R"))
                        cardsToShow.Add(card);

                    StringToChangeDeck.text = "Ataque a Distancia";
                }

                if (type == "Sigee")
                {
                    if (card is UnityCard unityCard && unityCard.Row.Contains("S"))
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
                GameObject slot = DeckShower.transform.GetChild(i).gameObject;
                newCard.transform.SetParent(slot.transform);        // se le otorga un slot en la escena
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

    private void ShowImage()
    {
        // Mostrar la imagen

        LeaderImage.color = new Color(LeaderImage.color.r, LeaderImage.color.g, LeaderImage.color.b, 1);
    }

    private void HideImage()
    {
        // Ocultar la imagen
        LeaderImage.color = new Color(LeaderImage.color.r, LeaderImage.color.g, LeaderImage.color.b, 0);
    }

    private void CleanCollection()
    {
        for (int i = 0; i < CollectionShower.transform.childCount; i++)
        {
            GameObject slot = CollectionShower.transform.GetChild(i).gameObject;
            foreach (Transform child in slot.transform)
            {
                // Eliminar cada hijo
                Destroy(child.gameObject);
            }
        }
    }

    private void CleanDeck()
    {
        for (int i = 0; i < DeckShower.transform.childCount; i++)
        {
            GameObject slot = DeckShower.transform.GetChild(i).gameObject;
            foreach (Transform child in slot.transform)
            {
                // Eliminar cada hijo
                Destroy(child.gameObject);
            }
        }
    }


}
