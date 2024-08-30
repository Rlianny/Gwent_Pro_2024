using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerDeckManager : MonoBehaviour
{
    public static PlayerDeckManager playerDeckManager;
    private CardsCollection cardsCollection;
    public static DeckCreator actualDeck;
    public Image LeaderImage;
    public TextMeshProUGUI CardsInDeck;
    public TextMeshProUGUI UnityCards;
    public TextMeshProUGUI TotalForce;
    public TextMeshProUGUI HeroCards;
    public VerticalLayoutGroup FactionShower;
    public GameObject FactionPrefab;
    public Button InitButton;

    void Start()
    {
        playerDeckManager = this;

        // CharacterManager.Init(); // borrar despues
        cardsCollection = new CardsCollection(CardsCreator.GetCardInfoList(PathContainer.CardDataBaseDirectoryPath), CardsCreator.LoadAll(PathContainer.SerializedFilesDirectoryPath));
        InitButton.interactable = false;

        CardsInDeck.text = "";
        UnityCards.text = "";
        TotalForce.text = "";
        HeroCards.text = "";

        if (actualDeck == null)
            HideImage();

        foreach (var pair in cardsCollection.AllLeaders)
        {
            var newFaction = Instantiate(FactionPrefab, new Vector3(0, 0, 0), Quaternion.identity);
            newFaction.transform.SetParent(FactionShower.transform);
            string character = CharacterManager.Query(pair.Value.Name + " " + pair.Value.CharacterDescription);
            Image image = newFaction.transform.GetChild(0).GetComponent<Image>();
            image.sprite = Resources.Load<Sprite>(character);
            TextMeshProUGUI text = newFaction.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
            text.text = pair.Key;

        }

    }

    void Update()
    {
        if (actualDeck != null)
        {
            CardsInDeck.text = actualDeck.CardsTotalNumber.ToString();
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
        Debug.Log(Faction);
        actualDeck = null;
        actualDeck = new DeckCreator(Faction, cardsCollection.AllFactions, cardsCollection.AllLeaders);

        string character = CharacterManager.Query(actualDeck.DeckLeader.Name + " " + actualDeck.DeckLeader.CharacterDescription);
        LeaderImage.sprite = Resources.Load<Sprite>(character);

        ShowImage();

        foreach (Card card in cardsCollection.Collection)
        {
            if (card.Type != CardTypes.Líder && (card.Faction == "Neutral" || card.Faction == actualDeck.Faction))
            {
                actualDeck.AddCardToMyDeck(card);
                if (card is SilverUnityCard silverUnityCard)
                {
                    actualDeck.DuplicateCard(card);
                    actualDeck.DuplicateCard(card);
                }
            }

        }
    }

    private void ShowImage()
    {
        LeaderImage.color = new Color(LeaderImage.color.r, LeaderImage.color.g, LeaderImage.color.b, 1);
    }

    private void HideImage()
    {
        LeaderImage.color = new Color(LeaderImage.color.r, LeaderImage.color.g, LeaderImage.color.b, 0);
    }

    public void SaveDeck()
    {
        GameData.CreatePlayer();
    }
}
