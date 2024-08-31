using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using System.Threading.Tasks;
using UnityEngine.UI;


public class UIHand : MonoBehaviour
{
    [SerializeField] Subject gameManager;
    public HorizontalLayoutGroup Hand;
    public GameObject CardPrefab;
    public CanvasGroup Interaction;
    public Button LeaderInteraction;
    public bool LeaderSkillIsEnable = true;
    public Button PassButton;
    public GameObject ExteriorPoint;
    private Vector3 OriginalPosition;

    async void Start()
    {
        Hand hand;
        OriginalPosition = Hand.transform.position;

        if (this.transform.parent.name == "Player1")
        {
            hand = GameData.Player1.PlayerHand;

            foreach (Card card in hand.PlayerHand)
            {
                var newCard = Instantiate(CardPrefab, new Vector3(0, 0, 0), Quaternion.identity);
                newCard.transform.SetParent(Hand.transform);
                UICard ui = newCard.GetComponent<UICard>();
                ui.PrintCard(card);

                Debug.Log(card.Name);

                await Task.Delay(400);
            }
        }

        else if (this.transform.parent.name == "Player2")
        {
            hand = GameData.Player2.PlayerHand;

            foreach (Card card in hand.PlayerHand)
            {
                var newCard = Instantiate(CardPrefab, new Vector3(0, 0, 0), Quaternion.identity);
                newCard.transform.SetParent(Hand.transform);
                UICard ui = newCard.GetComponent<UICard>();
                ui.PrintCard(card);

                Debug.Log(card.Name);

                await Task.Delay(400);
            }
        }

        else
        {
            Debug.Log("ERROR: " + this.transform.parent.name);
        }

    }

    void Update()
    {
        if (this.transform.parent.name == "Player1")
        {
            if (GameManager.Player1.IsActive == true)
            {
                this.Hand.transform.position = OriginalPosition;
                Interaction.blocksRaycasts = true;
                PassButton.interactable = true;

                if (LeaderSkillIsEnable)
                    LeaderInteraction.interactable = true;

                else
                    LeaderInteraction.interactable = false;
            }

            else
            {
                
                this.Hand.transform.position = ExteriorPoint.transform.position;
                Interaction.blocksRaycasts = false;
                LeaderInteraction.interactable = false;
                PassButton.interactable = false;
            }
        }

        if (this.transform.parent.name == "Player2")
        {
            if (GameManager.Player2.IsActive == true)
            {
                this.Hand.transform.position = OriginalPosition;
                Interaction.blocksRaycasts = true;
                PassButton.interactable = true;

                if (LeaderSkillIsEnable)
                    LeaderInteraction.interactable = true;

                else
                    LeaderInteraction.interactable = false;
            }

            else
            {
                
                this.Hand.transform.position = ExteriorPoint.transform.position;
                Interaction.blocksRaycasts = false;
                LeaderInteraction.interactable = false;
                PassButton.interactable = false;
            }
        }
    }




    async public void DrawCardUI(Card card)
    {
        await Task.Delay(400);

        if (Hand.GetComponent<HorizontalLayoutGroup>().transform.childCount < 10)
        {
            var newCard = Instantiate(CardPrefab, new Vector3(0, 0, 0), Quaternion.identity);
            newCard.transform.SetParent(Hand.transform);
            UICard ui = newCard.GetComponent<UICard>();
            ui.PrintCard(card);

            Debug.Log($"La carta {card.Name} ha sido añadida a la mano visual");
        }

        else
            Debug.Log("No es posible añadir más cartas a la mano visual");
    }

    async private void Wait()
    {
        await Task.Delay(2000);
    }
}
